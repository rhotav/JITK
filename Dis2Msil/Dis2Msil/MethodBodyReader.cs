using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

//il = new byte[] { 0x02, 0x28, 0x1C, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x02, 0x03, 0x7D, 0x11, 0x00, 0x00, 0x04, 0x2A };
//02-28-1C-00-00-0A-00-00-02-03-7D-11-00-00-04-2A
//TEST VALUE

namespace Dis2Msil
{
    //https://www.codeproject.com/Articles/14058/Parsing-the-IL-of-a-Method-Body
    public class MethodBodyReader
    {
        public List<ILInstruction> instructions = null;
        protected byte[] il = null;
        private MethodInfo mi = null;

        private int ReadInt16(byte[] _il, ref int position)
        {
            return ((il[position++] | (il[position++] << 8)));
        }
        private ushort ReadUInt16(byte[] _il, ref int position)
        {
            return (ushort)((il[position++] | (il[position++] << 8)));
        }
        private int ReadInt32(byte[] _il, ref int position)
        {
            return (((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18));
        }
        private ulong ReadInt64(byte[] _il, ref int position)
        {
            return (ulong)(((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18) | (il[position++] << 0x20) | (il[position++] << 0x28) | (il[position++] << 0x30) | (il[position++] << 0x38));
        }
        private double ReadDouble(byte[] _il, ref int position)
        {
            return (((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18) | (il[position++] << 0x20) | (il[position++] << 0x28) | (il[position++] << 0x30) | (il[position++] << 0x38));
        }
        private sbyte ReadSByte(byte[] _il, ref int position)
        {
            return (sbyte)il[position++];
        }
        private byte ReadByte(byte[] _il, ref int position)
        {
            return il[position++];
        }
        private Single ReadSingle(byte[] _il, ref int position)
        {
            return ((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18);
        }

        /// <summary>
        /// Constructs the array of ILInstructions according to the IL byte code.
        /// </summary>
        /// <param name="module"></param>
        private void ConstructInstructions(Module module)
        {
            try
            {
                Globals.LoadOpCodes();
                byte[] il = this.il;
                int position = 0;
                instructions = new List<ILInstruction>();
                while (position < il.Length)
                {
                    ILInstruction instruction = new ILInstruction();

                    // get the operation code of the current instruction
                    OpCode code = OpCodes.Nop;
                    ushort value = il[position++];
                    if (value != 0xfe)
                    {
                        code = Globals.singleByteOpCodes[value];
                    }
                    else
                    {
                        value = il[position++];
                        code = Globals.multiByteOpCodes[value];
                        value = (ushort)(value | 0xfe00);
                    }
                    instruction.Code = code;
                    instruction.Offset = position - 1;
                    int metadataToken = 0;
                    // get the operand of the current operation
                    switch (code.OperandType)
                    {
                        case OperandType.InlineBrTarget:
                            metadataToken = ReadInt32(il, ref position);
                            metadataToken += position;
                            instruction.Operand = metadataToken;
                            break;
                        case OperandType.InlineField:
                            metadataToken = ReadInt32(il, ref position);
                            instruction.Operand = module.ResolveField(metadataToken);
                            break;
                        case OperandType.InlineMethod:
                            metadataToken = ReadInt32(il, ref position);
                            try
                            {
                                instruction.Operand = module.ResolveMethod(metadataToken);
                            }
                            catch
                            {
                                instruction.Operand = module.ResolveMember(metadataToken);
                            }
                            break;
                        case OperandType.InlineSig:
                            metadataToken = ReadInt32(il, ref position);
                            instruction.Operand = module.ResolveSignature(metadataToken);
                            break;
                        case OperandType.InlineTok:
                            metadataToken = ReadInt32(il, ref position);
                            try
                            {
                                instruction.Operand = module.ResolveType(metadataToken);
                            }
                            catch
                            {
                                instruction.Operand = metadataToken;
                            }
                            break;
                        case OperandType.InlineType:
                            metadataToken = ReadInt32(il, ref position);
                            // now we call the ResolveType always using the generic attributes type in order
                            // to support decompilation of generic methods and classes

                            // thanks to the guys from code project who commented on this missing feature
                            try
                            {
                                if (mi == null)
                                {
                                    instruction.Operand = metadataToken.ToString("x8");
                                    break;
                                }
                                instruction.Operand = module.ResolveType(metadataToken, mi.DeclaringType.GetGenericArguments(), mi.GetGenericArguments());
                            }
                            catch
                            {
                                instruction.Operand = metadataToken;
                            }
                            break;
                        case OperandType.InlineI:
                            {
                                instruction.Operand = ReadInt32(il, ref position);
                                break;
                            }
                        case OperandType.InlineI8:
                            {
                                instruction.Operand = ReadInt64(il, ref position);
                                break;
                            }
                        case OperandType.InlineNone:
                            {
                                instruction.Operand = null;
                                break;
                            }
                        case OperandType.InlineR:
                            {
                                instruction.Operand = ReadDouble(il, ref position);
                                break;
                            }
                        case OperandType.InlineString:
                            {
                                metadataToken = ReadInt32(il, ref position);
                                instruction.Operand = module.ResolveString(metadataToken);
                                break;
                            }
                        case OperandType.InlineSwitch:
                            {
                                int count = ReadInt32(il, ref position);
                                int[] casesAddresses = new int[count];
                                for (int i = 0; i < count; i++)
                                {
                                    casesAddresses[i] = ReadInt32(il, ref position);
                                }
                                int[] cases = new int[count];
                                for (int i = 0; i < count; i++)
                                {
                                    cases[i] = position + casesAddresses[i];
                                }
                                break;
                            }
                        case OperandType.InlineVar:
                            {
                                instruction.Operand = ReadUInt16(il, ref position);
                                break;
                            }
                        case OperandType.ShortInlineBrTarget:
                            {
                                instruction.Operand = ReadSByte(il, ref position) + position;
                                break;
                            }
                        case OperandType.ShortInlineI:
                            {
                                instruction.Operand = ReadSByte(il, ref position);
                                break;
                            }
                        case OperandType.ShortInlineR:
                            {
                                instruction.Operand = ReadSingle(il, ref position);
                                break;
                            }
                        case OperandType.ShortInlineVar:
                            {
                                instruction.Operand = ReadByte(il, ref position);
                                break;
                            }
                        default:
                            {
                                throw new Exception("Unknown operand type.");
                            }
                    }
                    instructions.Add(instruction);
                }
            }
            catch { }
        }


        public object GetRefferencedOperand(Module module, int metadataToken)
        {
            AssemblyName[] assemblyNames = module.Assembly.GetReferencedAssemblies();
            for (int i = 0; i < assemblyNames.Length; i++)
            {
                Module[] modules = Assembly.Load(assemblyNames[i]).GetModules();
                for (int j = 0; j < modules.Length; j++)
                {
                    try
                    {
                        Type t = modules[j].ResolveType(metadataToken);
                        return t;
                    }
                    catch
                    {
                        return typeof(MissingMethodException);
                    }

                }
            }
            return null;

        }

        public string GetBodyCode()
        {
            string result = "";
            if (instructions != null)
            {
                for (int i = 0; i < instructions.Count; i++)
                {
                    result += instructions[i].GetCode() + "\n";
                }
            }
            return result;

        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        } //https://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array/321404


        public MethodBodyReader(Module module, string strilArray)
        {
            Globals.LoadOpCodes();
            if (module != null)
            {
                byte[] ilArray = StringToByteArray(strilArray.Replace("-", ""));
                il = ilArray;
                ConstructInstructions(module);
            }
        }

        public MethodBodyReader(Module module, byte[] ilArray)
        {
            if (module != null)
            {

                il = ilArray;

                ConstructInstructions(module);
            }
        }
    }
}