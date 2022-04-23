using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeLineCounter.Counter
{
    public class LineCounter
    {
        public LineCounter(LineCounterHandler changeLineCounter)
        {
            ChangeLineCounter += changeLineCounter;
        }

        public event LineCounterHandler ChangeLineCounter = null!;

        public CancellationTokenSource CancellationTokenSource { get; set; } = new();

        public string ProjectPath { get; set; } = default!;
        public int FileCount { get; set; } = default!;

        public List<string> IgnorableFolders { get; set; } = default!;
        public List<string> IgnorableFiles { get; set; } = default!;
        public List<string> IgnorableExtensions { get; set; } = default!;
        public List<FileData> Files { get; private set; } = default!;

        public List<FileData> TrackedFiles
        {
            get
            {
                return Files?
                    .Where(file => IgnorableFolders?.All(ignorableFolder => !file.FullPath.Contains(ProjectPath + ignorableFolder)) ?? true)
                    .Where(file => IgnorableExtensions?.All(ignorableExtension => file.Extension != ignorableExtension) ?? true)
                    .Where(file => IgnorableFiles?.All(ignorableFile => !file.FullPath.Contains(ProjectPath + ignorableFile)) ?? true)
                    .ToList()
                    ?? default!;
            }
        }

        private int _lineCount = default!;
        public int LineCount
        {
            get => _lineCount;
            private set
            {
                if (_lineCount == value) return;

                _lineCount = value;
                ChangeLineCounter?.Invoke(this, new LineCounterEventArgs());
            }
        }

        private int _characterCount = default!;
        public int CharacterCount
        {
            get => _characterCount;
            private set
            {
                if (_characterCount == value) return;

                _characterCount = value;
                ChangeLineCounter?.Invoke(this, new LineCounterEventArgs());
            }
        }

        private bool ForceUpdateCount { get; set; } = false;

        private static IEnumerable<FileData> GetFiles(string path)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Select(x => new FileData(x));
        }

        private static int GetFileCount(string path)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Length;
        }

        private async void RunChangeCountFilesCheckerAsync()
        {
            while (true)
            {
                if (CancellationTokenSource.IsCancellationRequested) return;

                if (FileCount != GetFileCount(ProjectPath) || ForceUpdateCount)
                {
                    FileCount = GetFileCount(ProjectPath);
                    Files = GetFiles(ProjectPath).ToList();

                    ForceUpdateCount = false;
                }

                await Task.Delay(1000);
            }
        }

        public async void StartAsync()
        {
            RunChangeCountFilesCheckerAsync();

            while (true)
            {
                if (CancellationTokenSource.IsCancellationRequested) return;

                if (TrackedFiles?.Select(file => file.Update()).Any(x => x) ?? false)
                {
                    LineCount = TrackedFiles?.Select(file => file.LineCount).Sum() ?? 0;
                    CharacterCount = TrackedFiles?.Select(file => file.CharacterCount).Sum() ?? 0;
                }

                await Task.Delay(100);
            }
        }

        public void ForceUpdate()
        {
            ForceUpdateCount = true;
        }

        public void Stop()
        {
            CancellationTokenSource.Cancel();
        }
    }
}
