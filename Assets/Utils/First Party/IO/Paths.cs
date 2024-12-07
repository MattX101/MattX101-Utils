using System;
using System.IO;

namespace Utils.IO
{
    public static class Paths
    {
        public static string CurrentDirectory => Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());

        public static string[] Drives => Directory.GetLogicalDrives();

        public const Environment.SpecialFolder CommonDesktop = Environment.SpecialFolder.CommonDesktopDirectory;
        public const Environment.SpecialFolder CommonDocuments = Environment.SpecialFolder.CommonDocuments;
        public const Environment.SpecialFolder CommonPictures = Environment.SpecialFolder.CommonPictures;
        public const Environment.SpecialFolder CommonVideos = Environment.SpecialFolder.CommonVideos;
        public const Environment.SpecialFolder CommonMusic = Environment.SpecialFolder.CommonMusic;

        public const Environment.SpecialFolder UserProfile = Environment.SpecialFolder.UserProfile;

        public const Environment.SpecialFolder Desktop = Environment.SpecialFolder.DesktopDirectory;

        public const Environment.SpecialFolder MyComputer = Environment.SpecialFolder.MyComputer;
        public const Environment.SpecialFolder MyDocuments = Environment.SpecialFolder.MyDocuments;
        public const Environment.SpecialFolder MyPictures = Environment.SpecialFolder.MyPictures;
        public const Environment.SpecialFolder MyVideos = Environment.SpecialFolder.MyVideos;
        public const Environment.SpecialFolder MyMusic = Environment.SpecialFolder.MyMusic;

        public const Environment.SpecialFolder ApplicationData = Environment.SpecialFolder.ApplicationData;

        public const Environment.SpecialFolder ProgramFiles = Environment.SpecialFolder.ProgramFiles;
        public const Environment.SpecialFolder ProgramFilesX86 = Environment.SpecialFolder.ProgramFilesX86;

        public const Environment.SpecialFolder CommonApplicationData = Environment.SpecialFolder.CommonApplicationData;

        public static string GetPath(Environment.SpecialFolder path)
        {
            return Environment.GetFolderPath(path);
        }
    }
}
