using ActionBehaviour;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Processor", menuName = "Sprite Processing/Attack Processor")]
public class AttackProcessor : SpriteActionProcessor
{
    [SerializeField] private Sprite[] attackSprites;
    [SerializeField] private float _duration = .1f;

    public override float Duration => _duration;
 
    public override void ProcessAction(SpriteRenderer spriteRenderer, IEvent attackEvent)
    {
        var _sprite = attackSprites[Random.Range(0, attackSprites.Length)];
        spriteRenderer.sprite = _sprite;
    }
}