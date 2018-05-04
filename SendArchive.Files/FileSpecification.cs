namespace SendArchive.Files
{
    /// <summary>
    /// Class for storing data about selected files
    /// </summary>
    public class FileSpecification
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
    }
}