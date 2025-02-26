using ActionBehaviour;

public interface IActionTelegraph : IDestroyable
{
    public ActionInfo? ActionInfo { get; }
}