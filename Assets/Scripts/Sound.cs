using ActionBehaviour;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource _source;
    private EventBinding<MovementEvent> movementBinding;
    private EventBinding<AttackEvent> attackBinding;

    private float _basePitch;
    private float _baseVolume;

    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _movementSound;
    [SerializeField] private float _pitchDeviation = .4f;
    [SerializeField] private float _volumeDeviation = .2f;


    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _basePitch = _source.pitch;
        _baseVolume = _source.volume;

        var entity = GetComponentInParent<ICellEntity>();
        bool movementPredicate(MovementEvent @event) => @event.entity == entity;
        movementBinding = new EventBinding<MovementEvent>(OnMovement, movementPredicate);
        EventBus<MovementEvent>.Register(movementBinding);

        var attacker = GetComponentInParent<IAttacker>();
        bool attackPredicate(AttackEvent @event) => @event.attacker == attacker;
        attackBinding = new EventBinding<AttackEvent>(OnAttack, attackPredicate);
        EventBus<AttackEvent>.Register(attackBinding);
    }

    private void OnMovement()
    {
        RandomizePitchAndVolume();
        _source.clip = _movementSound;
        _source.Play();
    }

    private void OnAttack()
    {
        RandomizePitchAndVolume();
        _source.clip = _attackSound;
        _source.Play();
    }

    private void RandomizePitchAndVolume()
    {
        _source.pitch = _basePitch + Random.Range(-1, 1) * _pitchDeviation;
        _source.volume = _baseVolume + Random.Range(-1, 1) * _volumeDeviation;
    }

    private void OnDestroy()
    {
        EventBus<MovementEvent>.Deregister(movementBinding);
        EventBus<AttackEvent>.Deregister(attackBinding);
    }
}
