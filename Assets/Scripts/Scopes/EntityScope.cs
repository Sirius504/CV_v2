using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EntityScope : LifetimeScope
{
    [SerializeField] private Plotter plotter;
    [SerializeField] private Enemy enemy;
    [SerializeField] private CellEntity cellEntity;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<Attack>(Lifetime.Transient);
        builder.Register<Move>(Lifetime.Transient);
        builder.Register<Sequence>(Lifetime.Transient);
        builder.Register<Selector>(Lifetime.Transient);
        builder.Register<Blocking>(Lifetime.Transient);
        builder.Register<RangedAttack>(Lifetime.Transient);

        var attacker = GetComponent<IAttacker>();
        var attackable = GetComponent<IAttackable>();
        builder.RegisterComponent(attacker).AsSelf();
        builder.RegisterComponent(attackable).AsSelf();
        builder.RegisterComponent(plotter).AsSelf();
        builder.RegisterComponent(enemy).AsImplementedInterfaces();
        builder.RegisterComponent(cellEntity).AsImplementedInterfaces();
    }
}
