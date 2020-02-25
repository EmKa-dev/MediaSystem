using MediaSystem.Communications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MediaSystem.MediaServer
{
    public class LocalContentReader
    {
        public DataMediaType MediaType { get; set; }
        public int NumberOfFiles { get; private set; }

        private string _folderPath;

        /// <summary>
        /// Reads the media contents inside the specified folder
        /// </summary>
        /// <param name="path"></param>
        public LocalContentReader(string path)
        {
            _folderPath = path;

            //TODO: Better exception-handling and flow (don't throw exceptions in most cases, print an error instead)

            if (Directory.Exists($"{_folderPath}") && Directory.Exists(@$"{_folderPath}\Cover") && Directory.Exists(@$"{_folderPath}\Content"))
            {
                try
                {
                    NumberOfFiles = Directory.GetFiles(@$"{_folderPath}\Content").Length;
                    MediaType = GetMediaType();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                //Log error or exception, folder path or folder structure faulty.
            }

        }

        private DataMediaType GetMediaType()
        {
            //Collection of possible extension to check files against
            string[] imagetypes = { ".jpg", ".jpeg", ".png", ".bmp"};
            string[] videotypes = { ".mp4", ".avi" };
            string[] musictypes = { ".mp3", ".wav" };

            //Get filenames in folder
            List<string> filenames = Directory.GetFiles(@$"{_folderPath}\Content").ToList();

            //Only want the extensions to check against, so remove filename except for extension
            for (int i = 0; i < filenames.Count; i++)
            {
                filenames[i] = Path.GetExtension(filenames[i]);
            }

            //Check our extensions against possible extensions
            //to determine what type of files
            if (filenames.TrueForAll(imagetypes.Contains))
            {
                return DataMediaType.Image;
            }
            else if (filenames.TrueForAll(videotypes.Contains))
            {
                return DataMediaType.Video;
            }
            else if (filenames.TrueForAll(musictypes.Contains))
            {
                return DataMediaType.Audio;
            }

            return 0;
        }

        /// <summary>
        /// If more than one image exists, one will be chosen randomly.
        /// </summary>
        /// <returns></returns>
        public byte[] GetCoverImageData()
        {
            string[] filenames = Directory.GetFiles(@$"{_folderPath}\Cover");

            if (filenames.Length > 0)
            {
                try
                {
                    return File.ReadAllBytes(filenames[new Random().Next(0, filenames.Length)]);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return null;
        }

        public byte[] GetSpecifiedFileData(string filename)
        {
            try
            {
                return File.ReadAllBytes(@$"{_folderPath}\Content\{filename}");
            }
            catch (Exception)
            {
                throw;
                //return null;
            }
        }

        public List<MediaFileInfo> GetContentInfo()
        {
            
            string[] filepaths = Directory.GetFiles(@$"{_folderPath}\Content");

            List<MediaFileInfo> mflist = new List<MediaFileInfo>();

            foreach (var filepath in filepaths)
            {
                var mf = new MediaFileInfo();

                string filename = Path.GetFileName(filepath);

                mf.FileName = filename;

                mf.FileMetaData = new MetaData
                {
                    Title = filename,
                    Author = "-",
                    //TODO 
                    RunTime = null
                };

                mflist.Add(mf);
            }

            return mflist;
        }
    }
}
