using ActionBehaviour;
using UnityEngine;

[CreateAssetMenu(fileName = "Block Processor", menuName = "Sprite Processing/Block Processor")]
public class BlockProcessor : SpriteActionProcessor
{
    [SerializeField] private Sprite blockSprite;
    [SerializeField] private float _duration = .1f;

    public override float Duration => _duration;
 
    public override void ProcessAction(SpriteRenderer spriteRenderer, IEvent _)
    {
        spriteRenderer.sprite = blockSprite;
    }

    public override void OnTick(uint tick)
    {
        base.OnTick(tick);
        
    }
}