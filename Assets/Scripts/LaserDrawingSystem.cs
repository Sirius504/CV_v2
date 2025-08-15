using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class LaserDrawingSystem : SystemBase<LaserDrawingSystem, ILaserDrawer>,
    IInitializable,
    IUpdatable
{
    [SerializeField] private LineRenderer _laserPrefab;
    [Inject]
    private Level _level;

    private Gradient _originalGradient;

    private readonly Dictionary<ILaserDrawer, LineRenderer> _lasers = new();
    private readonly Dictionary<LineRenderer, Gradient> _gradients = new();

    public UpdateOrder UpdateOrder => UpdateOrder.Animation;

    public InitOrder InitOrder => InitOrder.Animation;

    protected override void RegisterMany(IEnumerable<ILaserDrawer> entities)
    {
        foreach (var entity in entities)
        {
            Register(entity);
        }
    }
    public void Init()
    {
        _originalGradient = _laserPrefab.colorGradient;
    }

    private void Register(ILaserDrawer entity)
    {
        var laser = Instantiate(_laserPrefab);
        var gradient = new Gradient();

        gradient.SetKeys(_originalGradient.colorKeys, _originalGradient.alphaKeys);

        _lasers.Add(entity, laser);
        _gradients.Add(laser, gradient);
        entity.OnDestroyEvent += OnDrawerDestroyed;
    }

    private void OnDrawerDestroyed(IDestroyable entity)
    {
        var drawer = (ILaserDrawer)entity;
        var laser = _lasers[drawer];
        _lasers.Remove(drawer);
        _gradients.Remove(laser);
        Destroy(laser);
        drawer.OnDestroyEvent -= OnDrawerDestroyed;
    }

    public void UpdateManual()
    {
        foreach((var drawer, var laser) in _lasers)
        {
            var enabled = drawer.Source != null && drawer.Target != null;
            laser.gameObject.SetActive(enabled);
            if (!enabled) continue;

            laser.SetPosition(0, _level.GetEntitiyWorldPosition(drawer.Source));
            laser.SetPosition(1, _level.GetEntitiyWorldPosition(drawer.Target));

            var gradient = _gradients[laser];

            var newAlpha = new GradientAlphaKey[2]
            {
                new GradientAlphaKey() { alpha = drawer.Progress, time = 0f },
                new GradientAlphaKey() { alpha = drawer.Progress, time = 1f }
            };

            gradient.SetKeys(gradient.colorKeys, newAlpha);
            laser.colorGradient = gradient;
        }
    }

}
