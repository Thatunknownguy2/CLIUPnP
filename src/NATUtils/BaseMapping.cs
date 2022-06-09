namespace CLIPnP.NATUtils
{
    internal class BaseMapping
    {
        public string Name { get; set; }
        public Protocol Protocol { get; set; }
        public int ExternalPort { get; set; }
        public int InternalPort { get; set; }
        public bool DefaultActive { get; set; }
    }

    internal enum Protocol
    {
        TCP,
        UDP,
        Both
    }
}
