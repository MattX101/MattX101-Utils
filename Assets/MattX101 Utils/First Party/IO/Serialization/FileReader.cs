using System.IO;
using UnityEngine;

namespace Utils.IO.Serialization
{
    public class FileReader
    {
        private readonly BinaryReader _reader;

        private readonly bool _validFile = false;

        public FileReader(string path)
        {
            if (File.Exists(path))
            {
                _validFile = true;

                _reader = new BinaryReader(
                    new FileStream(
                        path,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read),
                    System.Text.Encoding.UTF8
                );
            }
        }

        public void Close()
        {
            _reader.Dispose();
        }

        // Integers
        public byte ReadByte() => _validFile ? _reader.ReadByte() : (byte)0;
        public sbyte ReadSByte() => _validFile ? _reader.ReadSByte() : (sbyte)0;

        public short ReadShort() => _validFile ? _reader.ReadInt16() : (short)0;
        public ushort ReadUShort() => _validFile ? _reader.ReadUInt16() : (ushort)0;

        public int ReadInt() => _validFile ? _reader.ReadInt32() : 0;
        public uint ReadUInt() => _validFile ? _reader.ReadUInt32() : 0;

        public long ReadLong() => _validFile ? _reader.ReadInt64() : 0;
        public ulong ReadULong() => _validFile ? _reader.ReadUInt64() : 0;

        // Decimals
        public float ReadFloat() => _validFile ? _reader.ReadSingle() : 0.0f;

        public double ReadDouble() => _validFile ? _reader.ReadDouble() : 0.0d;

        // Color
        public Color ReadRGB() => _validFile ? new(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle(), 1) : Color.black;
        public Color ReadRGBA() => _validFile ? new(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle()) : Color.black;

        // Bool
        public bool ReadBool() => _validFile ? _reader.ReadBoolean() : false;

        // Characters
        public char ReadChar() => _validFile ? _reader.ReadChar() : ' ';

        public string ReadString() => _validFile ? _reader.ReadString() : "";
    }
}
