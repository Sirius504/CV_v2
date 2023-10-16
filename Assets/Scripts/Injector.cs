using System.Collections.Generic;
using UnityEngine;

public class Injector : SingletonBehaviour<Injector>
{
    [SerializeField] private Level _level;
    [SerializeField] private Metronome _metronome;

    private HashSet<MyMono> _resolveRequests;
    private bool _resolveAllowed = false;

    protected override void Awake()
    {
        base.Awake();
        _level = _level != null ? _level : FindObjectOfType<Level>();

        _resolveRequests = new HashSet<MyMono>();
    }

    private void Update()
    {
        if (!_resolveAllowed)
        {
            return;
        }

        ResolveAll();
    }

    public void RequestResolve(MyMono mb)
    {
        if (!_resolveRequests.Contains(mb))
        {
            _resolveRequests.Add(mb);
        }
        else
        {
            Debug.LogWarning($"Trying to request dependencies resolution for {mb.name}, but it was already requested.");
        }
    }

    /// <summary>
    /// Call this, when all systems were initialized;
    /// </summary>
    public void AllowResolve()
    {
        _resolveAllowed = true;
        ResolveAll();
    }

    private void ResolveAll()
    {
        foreach (var request in _resolveRequests)
        {
            Resolve(request);
        }
        _resolveRequests.Clear();
    }

    private void Resolve(MyMono mb)
    {
        if (mb is ICellHabitant entity)
        {
            _level.Add(entity);
            entity.Init(_level);
        }

        if (mb is ITickable tickable)
        {
            _metronome.Add(tickable);
        }
    }
}