public interface ITickable : IDestroyable
{
    public OrderInTick OrderInTick => OrderInTick.TickMiddle;
    public bool ReceiveTickIfCreatedDuringTick => false;
    public void OnTick(uint tick);
}