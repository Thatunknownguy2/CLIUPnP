namespace CLIPnP.Helper
{
    internal interface ICommand
    {
        public string Name { get; }
        public string Description { get; }
        public string Usage { get; }

        public void Execute(string[] args);
    }
}
