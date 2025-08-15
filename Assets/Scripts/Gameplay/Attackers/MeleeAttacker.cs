using UnityEngine;

public class MeleeAttacker : CellComponent, IAttacker
{
    [SerializeField] private int _damage;
    public int Damage => _damage;

    public void OnAttack(IAttackable target)
    {
        //throw new System.NotImplementedException();
    }
}
