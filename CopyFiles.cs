using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestartAutomation
{
    public class CopyFiles
    {
        private readonly string _src;
        private readonly string _dest;
        
        public CopyFiles(string source, string destination)
        {
            _src = source;
            _dest = destination;
            
        }

        public void Start(){
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