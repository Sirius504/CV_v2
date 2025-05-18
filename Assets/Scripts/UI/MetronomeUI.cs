using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MetronomeUI : MonoEntity, IUpdatable
{
    [SerializeField] private Image _image;

    [Inject]
    private Metronome _metronome;

    public UpdateOrder UpdateOrder => UpdateOrder.UI;


    public void UpdateManual()
    {
        var elapsed = Time.time - _metronome.BeginningTime;
        var currentTickTime = elapsed % _metronome.TickDuration;
        _image.fillAmount = currentTickTime / _metronome.TickDuration; // normalize
    }
}