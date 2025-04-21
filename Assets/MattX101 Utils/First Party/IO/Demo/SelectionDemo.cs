using UnityEngine;
using TMPro;

namespace Utils.IO
{
    public sealed class SelectionDemo : MonoBehaviour
    {
        private IOSelection _ioSelection;

        [SerializeField]
        private TMP_Text _filePath, _folderPath, _savePath;

        private void Awake()
        {
            _ioSelection = new IOSelection();
        }

        public void SelectFile()
        {
            _filePath.text = _ioSelection.SelectFile("*");
        }

        public void SelectFolder()
        {
            _folderPath.text = _ioSelection.SelectFolder();
        }

        public void SelectSavePath()
        {
            _savePath.text = _ioSelection.SelectSavePath("Save", ".txt");
        }
    }
}