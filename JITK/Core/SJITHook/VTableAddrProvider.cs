using System;

namespace JITK.Core.SJITHook
{
    public abstract class VTableAddrProvider
    {
        internal delegate IntPtr GetJit();
        public abstract IntPtr VTableAddr { get; }
    }
}
