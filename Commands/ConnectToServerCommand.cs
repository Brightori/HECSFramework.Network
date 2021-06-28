using HECSFramework.Core;

namespace Commands
{
    public struct ConnectToServerCommand : ICommand, IGlobalCommand
	{
		public string Address;
		public string ServerKey;
		public string ServerPort;
		public string LocalPort;
	}
}