using System.IO;

namespace Utils.IO.Serialization
{
    public class FileWriter
    {
        private readonly BinaryWriter _writer;

        public FileWriter(string path)
        {
            _writer = new BinaryWriter(
                new FileStream(
                    path, 
                    FileMode.Create, 
                    FileAccess.Write, 
                    FileShare.Write), 
                System.Text.Encoding.UTF8
            );
        }

        public void Close()
        {
            _writer.Dispose();
        }

        // Integers
        public void Write(byte value) => _writer.Write(value);
        public void Write(sbyte value) => _writer.Write(value);

        public void Write(short value) => _writer.Write(value);
        public void Write(ushort value) => _writer.Write(value);

        public void Write(int value) =>_writer.Write(value);
        public void Write(uint value) =>_writer.Write(value);

        public void Write(long value) => _writer.Write(value);
        public void Write(ulong value) => _writer.Write(value);

        // Decimals
        public void Write(float value) => _writer.Write(value);

        public void Write(double value) => _writer.Write(value);

        // Color
        public void Write(float r, float g, float b)
        {
            _writer.Write(r);
            _writer.Write(g);
            _writer.Write(b);
        }

        public void Write(float r, float g, float b, float a)
        {
            _writer.Write(r);
            _writer.Write(g);
            _writer.Write(b);
            _writer.Write(a);
        }

        // Bool
        public void Write(bool value) => _writer.Write(value);

        // Characters
        public void Write(char value) => _writer.Write(value);

        public void Write(string value) => _writer.Write(value);
    }
}
