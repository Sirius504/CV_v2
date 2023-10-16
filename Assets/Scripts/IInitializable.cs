public interface IInitializable
{ 
    public int InitOrder { get; }
    public void Init();
}