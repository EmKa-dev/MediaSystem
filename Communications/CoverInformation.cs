using System;

namespace MediaSystem.Communications
{
    [Serializable]
    public class CoverInformation
    {
        public byte[] CoverImageData { get; set; }

        public string MediaType { get; set; }
        public int NumberofFiles { get; set; }

        public CoverInformation(string mediatype, int numberoffiles, byte[] image = null)
        {
            CoverImageData = image;
            MediaType = mediatype;
            NumberofFiles = numberoffiles;
        }
    }
}
