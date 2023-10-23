using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Arrow : MonoBehaviour
{
    private static readonly Dictionary<Vector2Int, Quaternion> _directionQuaternions = new()
    {
        { Vector2Int.right, Quaternion.Euler(0f, 0f, 270f)},
        { Vector2Int.down, Quaternion.Euler(0f, 0f, 180f)},
        { Vector2Int.left, Quaternion.Euler(0f, 0f, 90f)},
        { Vector2Int.up, Quaternion.Euler(0f, 0f, 0f)},
    };

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Set(ActionInfo actionInfo)
    {
        transform.localRotation = _directionQuaternions[actionInfo.To - actionInfo.From];
        spriteRenderer.color = ActionInfoSettings.GetColor(actionInfo.Action);
    }
}