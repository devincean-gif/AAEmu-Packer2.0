using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AAMod.Forms;
using AAPacker;

namespace AAMod;

internal static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        EnsureAAFreeReader();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new ModMainForm());
    }

    /// <summary>
    ///     Registers the AAFree 5.0 (ZERO) pak format reader so AAMod can open/mod AAFree client paks.
    /// </summary>
    // De-obfuscates the AAFree 5.0 key at runtime (kept XOR-encoded so it is not plaintext in the binary).
    private static byte[] Deobf(byte[] e)
    {
        for (var i = 0; i < e.Length; i++) e[i] = (byte)(e[i] ^ (0x5A + i * 7));
        return e;
    }

    private static void EnsureAAFreeReader()
    {
        foreach (var r in AAPak.ReaderPool)
            if (r.HeaderBytes != null && r.HeaderBytes.Length == 4 &&
                r.HeaderBytes[0] == 0x5A && r.HeaderBytes[1] == 0x45 &&
                r.HeaderBytes[2] == 0x52 && r.HeaderBytes[3] == 0x4F)
                return; // already present

        var aafree = new AAPakFileFormatReader(true)
        {
            ReaderName = "AAFree 5.0 (ZERO)",
            HeaderBytes = new byte[] { 0x5A, 0x45, 0x52, 0x4F }, // "ZERO"
            HeaderEncryptionKey = Deobf(new byte[]
                { 0x3D, 0xBE, 0x12, 0xC1, 0x8C, 0x55, 0x9E, 0xBF, 0xF8, 0xFD, 0x31, 0x1E, 0x0B, 0x86, 0x80, 0x4C }),
            FileInfoReadOrder = new List<AAPakFileInfoElement>
            {
                AAPakFileInfoElement.Dummy2, AAPakFileInfoElement.FileName, AAPakFileInfoElement.Offset,
                AAPakFileInfoElement.Size, AAPakFileInfoElement.SizeDuplicate, AAPakFileInfoElement.PaddingSize,
                AAPakFileInfoElement.Md5, AAPakFileInfoElement.Dummy1, AAPakFileInfoElement.CreateTime,
                AAPakFileInfoElement.ModifyTime
            }
        };
        AAPak.ReaderPool.Add(aafree);
    }
}
