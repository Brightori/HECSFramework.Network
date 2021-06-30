namespace HECSFramework.Network
{
    public interface IReplicable
    {
        int LevelOfReplication { get; }
    }

    public interface INotReplicable
    {
    }
}
