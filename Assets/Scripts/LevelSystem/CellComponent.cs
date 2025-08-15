using UnityEngine;

public abstract class CellComponent : MonoComponent,
    IInitializable,
    ICellComponent
{
    protected ICellEntity _entity;

    public virtual InitOrder InitOrder => InitOrder.Entity;
    public ICellEntity Entity => _entity;

    public virtual void Init()
    {
        _entity = FindThroughParents<ICellEntity>(transform);
        if (_entity == null)
        {
            Debug.LogError($"CellComponent {GetType()} where CellEntity couldn't be found.");
        }
    }

    protected T FindThroughParents<T>(Transform current)
    {
        T result;
        do
        {
            if (current.TryGetComponent(out result))
            {
                return result;
            }
            current = current.transform.parent;
        }
        while (current != null);
        return result;
    }
}
