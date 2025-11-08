using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellEntity : MonoComponent, ICellEntity
{
    [SerializeField] private List<CellComponent> _components;

    private IEnumerable<ICellComponent> Components => _components;

    public InitOrder InitOrder => InitOrder.Entity;

    protected override void Awake()
    {
        base.Awake();
        SearchComponents();
    }

    private void OnValidate()
    {
        SearchComponents();
    }


    private void SearchComponents()
    {
        foreach (var component in GetComponents<CellComponent>())
        {
            if (_components.Contains(component)) continue;
            _components.Add(component);
        }

        foreach (var component in GetComponentsInChildren<CellComponent>())
        {
            if (_components.Contains(component)) continue;
            _components.Add(component);
        }
    }

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

    public void Add<T>() where T : CellComponent
    {
        var newComponent = gameObject.AddComponent<T>();
        _components.Add(newComponent);
    }
}
