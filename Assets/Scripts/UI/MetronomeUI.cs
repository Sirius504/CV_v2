using UnityEngine;
using UnityEngine.UI;

public class MetronomeUI : MonoEntity, IInjectable<Metronome>, IUpdatable
{
    [SerializeField] private Image _image;

    private Metronome _metronome;

    public UpdateOrder UpdateOrder => UpdateOrder.UI;

    public void Inject(Metronome metronome)
    {
        _metronome = metronome;
    }

    public void UpdateManual()
    {
        var elapsed = Time.time - _metronome.BeginningTime;
        var currentTickTime = elapsed % _metronome.TickDuration;
        _image.fillAmount = currentTickTime / _metronome.TickDuration; // normalize
    }
}