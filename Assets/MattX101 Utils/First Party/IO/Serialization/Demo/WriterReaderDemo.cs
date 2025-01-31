using System;
using UnityEngine;

namespace Utils.IO.Serialization
{
    public class WriterReaderDemo : MonoBehaviour
    {
        private void Start()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\file.data";
            
            FileWriter writer = new FileWriter(path);
            writer.Write(1);
            writer.Write(3.15f);
            writer.Write(true);
            writer.Write('a');
            writer.Write("Hello World!");
            writer.Write(1.0f, 0.33f, 0.5f);
            writer.Close();

            FileReader reader = new FileReader(path);
            Debug.Log(reader.ReadInt());
            Debug.Log(reader.ReadFloat());
            Debug.Log(reader.ReadBool());
            Debug.Log(reader.ReadChar());
            Debug.Log(reader.ReadString());
            Debug.Log(reader.ReadRGB());
            reader.Close();
        }
    }
}
