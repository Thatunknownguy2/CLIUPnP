using Mono.Nat;

namespace CLIPnP.NATUtils
{
    internal class PortMapping
    {
        public readonly BaseMapping BaseMapping;
        public readonly string Name;
        public readonly int InternalPort;
        public readonly int ExternalPort;

        private readonly Mapping primaryMapping;
        private readonly Mapping secondaryMapping = null;
        private readonly INatDevice natDevice;

        public PortMapping(BaseMapping baseMapping, INatDevice natDevice)
        {
            BaseMapping = baseMapping;
            this.natDevice = natDevice;
            Name = baseMapping.Name;
            InternalPort = baseMapping.InternalPort;
            ExternalPort = baseMapping.ExternalPort;

            if (baseMapping.Protocol == Protocol.Both)
            {
                primaryMapping = new Mapping(Mono.Nat.Protocol.Tcp, baseMapping.InternalPort, baseMapping.ExternalPort);
                secondaryMapping = new Mapping(Mono.Nat.Protocol.Udp, baseMapping.InternalPort, baseMapping.ExternalPort);
            } else
            {
                primaryMapping = new Mapping((baseMapping.Protocol == Protocol.TCP) ? Mono.Nat.Protocol.Tcp : Mono.Nat.Protocol.Udp, baseMapping.InternalPort, baseMapping.ExternalPort);
            }
        }

        public void Enable()
        {
            BaseMapping.DefaultActive = true;

            natDevice.CreatePortMap(primaryMapping);
            
            if (secondaryMapping != null)
            {
                natDevice.CreatePortMap(secondaryMapping);
            }
        }

        public void Disable()
        {
            if (BaseMapping.DefaultActive)
            {
                BaseMapping.DefaultActive = false;

                natDevice.DeletePortMap(primaryMapping);

                if (secondaryMapping != null)
                {
                    natDevice.DeletePortMap(secondaryMapping);
                }
            }
        }
    }
}
