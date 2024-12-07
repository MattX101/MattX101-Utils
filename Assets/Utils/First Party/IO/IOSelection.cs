using UnityEngine;
using SFB;

namespace Utils.IO
{
    public sealed class IOSelection
    {
        private static readonly string DefaultDirectory = Paths.GetPath(Paths.Desktop);

        // Single File Selection
        public string SelectSingleFile(string extension)
        {
            extension ??= "*";

            string[] paths = StandaloneFileBrowser.OpenFilePanel(
                "Select File",
                DefaultDirectory,
                extension,
                false);

            return GetPath(paths);
        }

        public string SelectSingleFile(ExtensionFilter[] extensions)
        {
            extensions ??= new ExtensionFilter[1]
            {
                new("All Files", "*")
            };

            string[] paths = StandaloneFileBrowser.OpenFilePanel(
                "Select File",
                DefaultDirectory,
                extensions,
                false);

            return GetPath(paths);
        }

        // Multi File Selection
        public string SelectMultiFile(string extension)
        {
            extension ??= "*";

            string[] paths = StandaloneFileBrowser.OpenFilePanel(
                "Select File",
                DefaultDirectory,
                extension,
                true);

            return GetPath(paths);
        }

        public string SelectMultiFile(ExtensionFilter[] extensions)
        {
            extensions ??= new ExtensionFilter[1]
            {
                new("All Files", "*")
            };

            string[] paths = StandaloneFileBrowser.OpenFilePanel(
                "Select File",
                DefaultDirectory,
                extensions,
                true);

            return GetPath(paths);
        }

        // Single Folder Selection
        public string SelectFolder(bool multiSelect = false)
        {
            string[] paths = StandaloneFileBrowser.OpenFolderPanel(
                "Select Folder",
                DefaultDirectory,
                multiSelect);

            return GetPath(paths);
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
            return StandaloneFileBrowser.SaveFilePanel(
                "Save As",
                DefaultDirectory,
                defaultFileName,
                filter);
        }

        public string SelectSavePath(string defaultFileName, ExtensionFilter[] filters)
        {
            return StandaloneFileBrowser.SaveFilePanel(
                "Save As",
                DefaultDirectory,
                defaultFileName,
                filters);
        }
    }
}