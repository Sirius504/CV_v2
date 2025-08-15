using UnityEngine;

[RequireComponent(typeof(ICellEntity))]
public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    public int Amount { get; private set; }
    public int MaxHealth => _maxHealth;


    private void Start()
    {
        Amount = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Amount = Mathf.Max(0, Amount - damage);
        if (Amount == 0)
        {
            // TODO: gain more control over object destruction
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        Amount = Mathf.Min(_maxHealth, Amount + amount);
    }
}