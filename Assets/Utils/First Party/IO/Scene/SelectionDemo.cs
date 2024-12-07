# if UNITY_EDITOR
using UnityEngine;
using TMPro;

namespace Utils.IO
{
    public class SelectionDemo : MonoBehaviour
    {
        private IOSelection _ioSelection;

        [SerializeField]
        private TMP_Text _filePath, _folderPath, _savePath;

        void Awake()
        {
            _ioSelection = new IOSelection();
        }

        public void SelectFile()
        {
            _filePath.text = _ioSelection.SelectSingleFile("*");
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
#endif