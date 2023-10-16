using UnityEngine;
using System.Linq;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Level _level;
    [SerializeField] private Metronome _metronome;


    private void Start()
    {
        var monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        // initializables
        foreach (var initializable in monoBehaviours
            .OfType<IInitializable>()
            .OrderBy(i => i.InitOrder))
        {
            initializable.Init(monoBehaviours);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _player.UpdateManual();
        _metronome.UpdateManual();
    }
}
