﻿using System.IO;

namespace CleverTapSDK.Private
{
	public static class EditorUtils
	{
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copyChangedOnly = true, bool copySubDirs = true)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension == ".meta")
                    continue;

                string destPath = Path.Combine(destDirName, file.Name);
                if (copyChangedOnly)
                {
                    bool overwrite = false;
                    bool exists = File.Exists(destPath);
                    if (exists)
                    {
                        overwrite = File.GetLastWriteTime(destPath) > File.GetLastWriteTime(destPath);
                    }

                    if (!exists || overwrite)
                        file.CopyTo(destPath, overwrite);
                }
                else
                {
                    file.CopyTo(destPath, true);
                }
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}