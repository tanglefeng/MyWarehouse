using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Kengic.Was.CrossCutting.MiniDumper
{
    public sealed class MiniDumper
    {
        public static bool Write(string fileName, MiniDumpType miniDumpType = MiniDumpType.MiniDumpWithFullMemory)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                MiniDumpExceptionInformation exp;
                exp.ThreadId = UnsafeNativeMethods.GetCurrentThreadId();
                exp.ClientPointers = false;
                exp.ExceptioonPointers = Marshal.GetExceptionPointers();
                var bRet = (fs.SafeFileHandle != null) && UnsafeNativeMethods.MiniDumpWriteDump(
                    UnsafeNativeMethods.GetCurrentProcess(),
                    UnsafeNativeMethods.GetCurrentProcessId(),
                    fs.SafeFileHandle.DangerousGetHandle(),
                    miniDumpType,
                    ref exp,
                    IntPtr.Zero,
                    IntPtr.Zero);
                return bRet;
            }
        }
    }
}