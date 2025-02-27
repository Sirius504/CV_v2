using UnityEngine;
using UnityEngine.UI;

public class MetronomeUI : MonoEntity, IInjectable<Metronome>
{
    [SerializeField] private Image _image;

    private Metronome _metronome;
    public void Inject(Metronome metronome)
    {
        _metronome = metronome;
    }

    private void Update()
    {
        var elapsed = Time.time - _metronome.BeginningTime;
        var currentTickTime = elapsed % _metronome.TickDuration;
        _image.fillAmount = currentTickTime / _metronome.TickDuration; // normilize
    }
}
