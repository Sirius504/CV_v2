using UnityEngine;
using System.Linq;
using System;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Level _level;
    [SerializeField] private Metronome _metronome;
    [SerializeField] private WinLoseConditions _winLose;


    private void Start()
    {
        var monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var initializables = monoBehaviours.OfType<IInitializable>().OrderBy(i => i.InitOrder);

        // initializables
        foreach (var initializable in initializables.Where(i => i.InitOrder <= InitOrder.Entity))
        {
            initializable.Init();
        }

        Injector.Instance.AllowResolve();

        foreach (var initializable in initializables.Where(i => i.InitOrder > InitOrder.Entity))
        {
            initializable.Init();
        }

        _player.OnDestroyEvent += OnPlayerDestroy;
    }

    private void OnDestroy()
    {
        if (_player != null)
            _player.OnDestroyEvent -= OnPlayerDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        _player?.UpdateManual();
        _metronome.UpdateManual();
        _winLose.UpdateManual();
    }
    private void OnPlayerDestroy(MonoBehaviour player)
    {
        _player.OnDestroyEvent -= OnPlayerDestroy;
        _player = null;
    }
}