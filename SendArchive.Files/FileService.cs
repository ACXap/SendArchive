using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SendArchive.Files
{
    /// <summary>
    /// Class to implement the interface
    /// </summary>
    public class FileService : IFileService
    {
        #region Public Method

        /// <summary>
        /// Method for obtaining a list of files
        /// </summary>
        /// <param name="callback">Delegate for return a list files</param>
        public void GetFiles(Action<List<FileSpecification>> callback)
        {
            Microsoft.Win32.OpenFileDialog myDialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                DereferenceLinks = false
            };

            if (myDialog.ShowDialog() != true || myDialog.FileNames.Length == 0)
            {
                return;
            }

            var countFiles = myDialog.FileNames.Length;
            List<FileSpecification> listFile = new List<FileSpecification>(countFiles);

            foreach (var file in myDialog.FileNames)
            {
                if (File.Exists(file))
                {
                    FileInfo fi = new FileInfo(file);
                    var f = new FileSpecification
                    {
                        Name = fi.Name,
                        Path = fi.FullName,
                        Size = fi.Length
                    };
                    listFile.Add(f);
                }
            }

            callback(listFile);
        }

        /// <summary>
        /// Method for open folder file
        /// </summary>
        /// <param name="path">Path file</param>
        public void OpenRepositoryFile(string path)
        {
            if (File.Exists(path))
            {
                Process.Start(new ProcessStartInfo("explorer", string.Format("/e, /select, \"{0}\"", path)));
            }
        }
       
        #endregion Public Method
    }
}