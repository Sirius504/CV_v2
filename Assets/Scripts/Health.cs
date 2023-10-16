using UnityEngine;

[RequireComponent(typeof(ICellHabitant))]
public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    public int Amount { get; private set; }
    public int MaxHealth => _maxHealth;

    private ICellHabitant _entity;

    private void Start()
    {
        Amount = _maxHealth;
        _entity = GetComponent<ICellHabitant>();
    }

    public void TakeDamage(int damage)
    {
        Amount = Mathf.Max(0, Amount - damage);
        if (Amount == 0)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        Amount = Mathf.Min(_maxHealth, Amount + amount);
    }
}