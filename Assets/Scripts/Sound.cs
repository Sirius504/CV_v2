using ActionBehaviour;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource _source;
    private IActioning _target;
    private IDestroyable _destroyable;

    private float _basePitch;
    private float _baseVolume;

    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _movementSound;
    [SerializeField] private float _pitchDeviation = .4f;
    [SerializeField] private float _volumeDeviation = .2f;


    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _target = GetComponentInParent<IActioning>();
        _destroyable = GetComponentInParent<IDestroyable>();
        _target.OnAction += OnTargetAction;
        _destroyable.OnDestroyEvent += OnTargetDestroyed;
        _basePitch = _source.pitch;
        _baseVolume = _source.volume;
    }

    private void OnTargetDestroyed(IDestroyable dest)
    {
        _target.OnAction -= OnTargetAction;
        dest.OnDestroyEvent -= OnTargetDestroyed;
    }

    private void OnTargetAction(ActionInfo actionInfo)
    {
        _source.clip = actionInfo.Action switch
        {
            Action.Movement => _movementSound,
            Action.Attack => _attackSound,
            _ => throw new System.NotImplementedException()
        };

        _source.pitch = _basePitch + Random.Range(-1, 1) * _pitchDeviation;
        _source.volume = _baseVolume + Random.Range(-1, 1) * _volumeDeviation;
        _source.Play();
    }
}
