namespace RestartAutomation
{
    public class CopyFiles
    {
        private readonly string _baseSrc;
        private readonly string _baseDest;
        private readonly List<string> _files;

        public CopyFiles(string source, string destination, List<string> filesToCopy)
        {
            _baseSrc = source;
            _baseDest = destination;
            _files = filesToCopy;
        }
        public void Start()
        {
            foreach (var item in _files)
            {
                Global.infoString += $"\nCopying {item}";
                Console.Write($"Copying {item}");
                this.CopyToDestination(_baseSrc + "\\" + item, _baseDest + "\\" + item);
                Global.infoString += $"             ...done";
                Console.Write($"             ...done\n");

            }
        }
        private void CopyToDestination(string _src, string _dest)
        {
            string[] originalFiles = Directory.GetFiles(_src, "*", SearchOption.AllDirectories);

            Array.ForEach(originalFiles, (originalFileLocation) =>
            {
                FileInfo originalFile = new FileInfo(originalFileLocation);
                FileInfo destFile = new FileInfo(originalFileLocation.Replace(_src, _dest));

                if (destFile.Exists)
                {
                    if (originalFile.LastWriteTimeUtc > destFile.LastWriteTimeUtc)
                    {
                        originalFile.CopyTo(destFile.FullName, true);
                    }
                }
                else
                {
                    Directory.CreateDirectory(destFile.DirectoryName!);
                    originalFile.CopyTo(destFile.FullName, false);
                }
            });
        }

    }
}