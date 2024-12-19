namespace Utils.Colors.Model
{
    public struct HEX
    {
        private string _hex;
        public string Hex
        {
            readonly get => _hex;
            internal set => _hex = value;
        }

        public HEX(string hex)
        {
            _hex = hex;
        }
    }
}
