using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZGF.Revit
{
    static class FileSystemHelper
    {

        /// <summary>
        /// Selects all files recursively from the initial directory downward.
        /// </summary>
        /// <param name="initialDirectory">The directory from which to begin retrieving the files.</param>
        /// <returns>System.Collections.Generic.List of FileInfo objects</returns>
        public static List<FileInfo> GetFilesRecursive(string initialDirectory)
        {
            return GetFilesRecursive(initialDirectory, string.Empty);
        }
        
        /// <summary>
        /// Selects all files recursively from the initial directory downward.
        /// </summary>
        /// <param name="initialDirectory">The directory from which to begin retrieving the files.</param>
        /// <param name="pattern"> 
        /// The search string to match against the names of files in path. The parameter cannot end in two periods ("..") or contain two periods ("..") followed by System.IO.Path.DirectorySeparatorChar or System.IO.Path.AltDirectorySeparatorChar, nor can it contain any of the characters in System.IO.Path.InvalidPathChars.
        ///</param>
        /// <returns>System.Collections.Generic.List of FileInfo objects</returns>
        public static List<FileInfo> GetFilesRecursive(string initialDirectory, string pattern)
        {
            if (pattern == string.Empty) pattern = "*.*";
            
            // 1. Store results in the file results list.
            List<FileInfo> result = new List<FileInfo>();

            // 2. Store a stack of our directories.
            Stack<string> stack = new Stack<string>();

            // 3. Add initial directory.
            stack.Push(initialDirectory);

            // 4. Continue while there are directories to process
            while (stack.Count > 0)
            {
                // A. Get top directory
                string dir = stack.Pop();

                try
                {
                    // B. Add all files at this directory to the result List.

                    string[] tmpfiles = Directory.GetFiles(dir, pattern);

                    for (int i = 0; i < tmpfiles.Length; i++)
                    {
                        try
                        {
                            result.Add(new System.IO.FileInfo(tmpfiles[i]));
                        }
                        catch
                        {

                        }
                    }

                    // C. Add all directories at this directory.
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                    // D.Could not open the directory
                }
            }
            return result;
        }

        /// <summary>
        /// Selects all folders recursively from the initial directory downward.
        /// </summary>
        /// <param name="initialDirectory">The directory from which to begin retrieving the folders.</param>
        /// <returns>System.Collections.Generic.List of DirectoryInfo objects</returns>
        public static List<DirectoryInfo> GetDirectoriesRecursive(string initialDirectory)
        {
            return GetDirectoriesRecursive(initialDirectory, string.Empty);
        }

        /// <summary>
        /// Selects all folders recursively from the initial directory downward.
        /// </summary>
        /// <param name="initialDirectory">The directory from which to begin retrieving the folders.</param>
        /// <param name="pattern"> 
        /// The search string to match against the names of folders to retrieve.
        ///</param>
        /// <returns>System.Collections.Generic.List of DirectoryInfo objects</returns>
        public static List<DirectoryInfo> GetDirectoriesRecursive(string initialDirectory, string pattern)
        {
            if (pattern == string.Empty) pattern = "*";
            
            // 1. Store Result as List<>
            List<DirectoryInfo> result = new List<DirectoryInfo>();

            // 2. Store a Stack of Directories
            Stack<string> stack = new Stack<string>();

            // 3. Add Initial Directory
            stack.Push(initialDirectory);

            // 4. Loop while there are directories to process
            while (stack.Count > 0)
            {

                // A. Get top most directory
                string dir = stack.Pop();

                try
                {
                    // B. Get sub-directories
                    string[] tmpDirs = Directory.GetDirectories(dir); //, pattern);

                    // C. Add Sub-directories
                    foreach (string d in tmpDirs)
                    {
                        stack.Push(d);
                        // This code is specific to Launcher: Just looks for folder names that match 'pattern'
                        if (d.EndsWith("\\" + pattern))
                            result.Add(new DirectoryInfo(d));
                    }

                }
                catch
                {
                    // D. Could not open sub-directory
                }

            }

            return result;
        }
    }
}
