using System;

namespace MediaSystem.Communications
{
    [Serializable]
    public class MediaFileInfo
    {
        /// <summary>
        /// Name of the file with extension
        /// </summary>
        public string FileName { get; set; }

        public MetaData FileMetaData { get; set; }
    }

    [Serializable]
    public class MetaData
    {
        public string Title { get; set; }
        public string Author { get; set; }
        /// <summary>
        /// Runtime in seconds for videos and music. Nullable in case not applicable (like in images).
        /// </summary>
        public uint? RunTime { get; set; }
    }
}
