using System;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bundler
{
    class Program
    {
        public static string RelPathToSolutionRootFolder = "../../../../"; //relative path to the solution root folder from Bundler.exe
        public static string RootFolderInZip;
        public static void Main()
        {
            List<string> files = new List<string>();
            string version;
            version = GetVersion(RelPathToSolutionRootFolder + "Client/bin/Release/Client.exe");

            RootFolderInZip = "OurProject/"; //name of the folder created inside the zip file

            files.Add(RelPathToSolutionRootFolder + "Client/bin/Release/Client.exe");

            string outputFile = RelPathToSolutionRootFolder + "Client-" + version + ".zip"; //name of the output zip file

            Console.WriteLine("Compressing files");
            Compress(outputFile, files);
            Console.WriteLine("Finished");
        }

        public static string GetVersion(string file)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(file);

            return fvi.FileVersion;
        }

        public static void GetDependencies(string inFolder, string module, ref List<string> dependencyList, bool bRecursive = true)
        {
            string depName, modName;

            Assembly assembly = Assembly.LoadFrom(inFolder + module);

            foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
            {
                modName = assemblyName.Name + ".dll";
                depName = inFolder + modName;
                if (System.IO.File.Exists(depName) && !dependencyList.Exists(name => name == depName))
                {
                    dependencyList.Add(depName);
                    if (bRecursive)
                        GetDependencies(inFolder, modName, ref dependencyList, false);
                }
            }
        }

        public static List<string> GetFilesInFolder(string folder, bool bRecursive, string filter = "*.*")
        {
            List<string> files = new List<string>();

            if (bRecursive)
                files.AddRange(Directory.EnumerateFiles(folder, filter, SearchOption.AllDirectories));
            else
                files.AddRange(Directory.EnumerateFiles(folder));
            return files;
        }

        public static void Compress(string outputFilename, List<string> files)
        {
            uint numFilesAdded = 0;
            double totalNumFiles = (double)files.Count;
            using (FileStream zipToOpen = new FileStream(outputFilename, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (string file in files)
                    {
                        if (System.IO.File.Exists(file))
                        {
                            archive.CreateEntryFromFile(file, RootFolderInZip + file.Substring(RelPathToSolutionRootFolder.Length));
                            numFilesAdded++;
                        }
                        else Console.WriteLine("Couldn't find file: {0}", file);

                        Console.Write("\rProgress: {0:F2}%", 100.0 * ((double)numFilesAdded) / totalNumFiles);
                    }
                    Console.WriteLine("\nSaving {0} files in  {1}", numFilesAdded, Path.GetFullPath(outputFilename));
                }
            }
        }
    }
}
