using UnityEngine;
using SFB;

namespace Utils.IO
{
    public sealed class IOSelection
    {
        private static readonly string DefaultDirectory = Paths.GetPath(Paths.Desktop);

        public string SelectFile(string extension, bool multiSelect = false)
        {
            extension ??= "*";

            return GetPath(StandaloneFileBrowser.OpenFilePanel("Select File", DefaultDirectory, extension, multiSelect));
        }

        public string SelectFile(ExtensionFilter[] extensions, bool multiSelect = false)
        {
            extensions ??= new ExtensionFilter[1]
            {
                new("All Files", "*")
            };

            return GetPath(StandaloneFileBrowser.OpenFilePanel("Select File", DefaultDirectory, extensions, multiSelect));
        }

        public string SelectFolder(bool multiSelect = false)
        {
            return GetPath(StandaloneFileBrowser.OpenFolderPanel("Select Folder", DefaultDirectory, multiSelect));
        }

        private string GetPath(string[] paths)
        {
            if (paths.Length == 0)
            {
                Debug.LogWarning("No valid path was selected!");

                return null;
            }

            Debug.Log(paths[0]);

            return paths[0];
        }

        public string SelectSavePath(string defaultFileName, string filter)
        {
            return StandaloneFileBrowser.SaveFilePanel("Save As", DefaultDirectory, defaultFileName, filter);
        }

        public string SelectSavePath(string defaultFileName, ExtensionFilter[] filters)
        {
            return StandaloneFileBrowser.SaveFilePanel("Save As", DefaultDirectory, defaultFileName, filters);
        }
    }
}