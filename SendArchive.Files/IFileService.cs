using System;
using System.Collections.Generic;

namespace SendArchive.Files
{
    /// <summary>
    /// Interface for describing the operation of the service with files
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Method get list files
        /// </summary>
        /// <param name="callback">Delegate to return a list files</param>
        void GetFiles(Action<List<FileSpecification>> callback);

        /// <summary>
        /// Method to open the folder where the file is stored
        /// </summary>
        /// <param name="path">Path to the file</param>
        void OpenRepositoryFile(string path);
    }
}