using System;
using System.Runtime.InteropServices;

namespace JITK.Core.SJITHook
{
    public sealed class ClrjitAddrProvider : VTableAddrProvider
    {
        [DllImport("Clrjit.dll", CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
        private static extern IntPtr getJit();

        public override IntPtr VTableAddr
        {
            get
            {
                IntPtr pVTable = getJit();
                if (pVTable == IntPtr.Zero)
                    throw new Exception("Could not retrieve address for getJit");

                return pVTable;
            }
        }
    }
}
