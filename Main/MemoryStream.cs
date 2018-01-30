
namespace Main
{
    class MemoryStream
    {
        public byte[] Buffer { get; set; }
        public byte[] GetBuffer() { return Buffer; }

        public MemoryStream() { }
        public MemoryStream(byte[] buffer, int start, int length)
        {
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
                result[i] = buffer[start + i];
            Buffer = result;
        }
    }
}
