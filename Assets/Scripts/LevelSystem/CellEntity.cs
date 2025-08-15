using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellEntity : MonoComponent, ICellEntity
{
    [SerializeField] private List<CellComponent> _components;

    private IEnumerable<ICellComponent> Components => _components;

    public InitOrder InitOrder => InitOrder.Entity;


    public bool Has<T>() where T : ICellComponent
    {
        return Components.Any(entity => entity is T);
    }

    public bool TryGet<T>(out T value) where T : ICellComponent
    {
        var matchingComponent = Components.FirstOrDefault(entity => entity is T);
        if (matchingComponent != null)
        {
            value = (T)matchingComponent;
            return true;
        }
        value = default;
        return false;
    }
}
