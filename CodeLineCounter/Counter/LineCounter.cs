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

        public IEnumerable<string> Ignorable { get; set; } = default!;
        public IEnumerable<string> Extentions { get; set; } = default!;
        public IEnumerable<string> FilePaths { get; set; } = default!;

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

        private static IEnumerable<string> GetFiles(string path, IEnumerable<string> extentions, IEnumerable<string> ignore)
        {
            if (path == null) return default!;

            IEnumerable<string> files = extentions == null
                ? Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                : Enumerable.Range(0, extentions.Count())
                    .Select(i => Directory.GetFiles(path, extentions.ToArray()[i], SearchOption.AllDirectories))
                    .SelectMany(i => i);

            files = files.Where(file => !ignore.Any(ign => ign.Contains(path + file)));

            return files;
        }

        private async void CheckEditCountFiles()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (GetFiles(ProjectPath, Extentions, Ignorable).Count() != (FilePaths?.Count() ?? 0))
                    {
                        FilePaths = GetFiles(ProjectPath, Extentions, Ignorable);
                    }

                    Task.Delay(1000);
                }
            }, CancellationTokenSource.Token);
        }

        public async void Start()
        {
            await Task.Run(() =>
            {
                CheckEditCountFiles();

                while (true)
                {
                    try
                    {
                        LineCount = FilePaths?.Select(file => File.ReadAllText(file).Split('\n').Length).Sum() ?? 0;
                        CharacterCount = FilePaths?.Select(file => File.ReadAllText(file).Length).Sum() ?? 0;
                    }
                    catch (Exception e) { }
                    finally { Task.Delay(1000); }
                }
            }, CancellationTokenSource.Token);
        }
    }
}
