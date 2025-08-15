public interface ILaserDrawer : IDestroyable
{
    ICellEntity Source { get; }
    ICellEntity Target { get; }
    float Progress { get; }
}