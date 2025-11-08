using System.Linq;
using UnityEngine;
using VContainer;

public class Door : CellComponent,
    IUpdatable
{
    public UpdateOrder UpdateOrder => UpdateOrder.Entity;

    [Inject]
    private Level _level;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Timer _roundTimer;

    [SerializeField]
    private CellEntity _cellEntity;

    protected override void Awake()
    {
        base.Awake();
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (_cellEntity == null)
        {
            _cellEntity = GetComponent<CellEntity>();
        }
    }

    public void UpdateManual()
    {
        if (_cellEntity.Has<Passable>())
        {
            return;
        }

        var entities = _level.Entities;
        var player = entities.FirstOrDefault(entity => entity.Has<Player>());
        var dog = entities.FirstOrDefault(entity => entity.Has<Dog>());

        var enemiesExist = entities.Where(entity => entity.Has<Enemy>()).Any();        

        if (!_roundTimer.IsRunning && !enemiesExist)
        {
            _cellEntity.Add<Passable>();
            _spriteRenderer.color = Color.green - new Color(0f,0f,0f,0.5f);            
            return;
        }
        _spriteRenderer.color = Color.red - new Color(0f, 0f, 0f, 0.5f);
    }
}
