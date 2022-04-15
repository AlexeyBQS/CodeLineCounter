using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLineCounter.Counter
{
    public class FileData
    {
        public FileData(string fullPath)
        {
            FullPath = fullPath;
        }

        public string FullPath { get; set; } = default!;

        public string Path => FullPath[..(FullPath.LastIndexOf('\\') + 1)];
        public string Name => FullPath[(FullPath.LastIndexOf('\\') + 1)..];
        public string Extension => FullPath[FullPath.LastIndexOf('.')..];
        public string FullName => Name + Extension;

        public DateTime LastChange { get; set; } = default!;
        public int LineCount { get; set; } = default!;
        public int CharacterCount { get; set; } = default!;

        public bool Update()
        {
            if (File.GetLastWriteTimeUtc(FullPath) != LastChange)
            {
                LastChange = File.GetLastWriteTimeUtc(FullPath);

                LineCount = File.ReadAllLines(FullPath).Length;
                CharacterCount = File.ReadAllText(FullPath).Length;

                return true;
            }

            return false;
        }
    }
}
