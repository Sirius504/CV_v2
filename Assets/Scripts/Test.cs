using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Grid grid;
    private Vector3Int direction = Vector3Int.right;
    private Vector3Int cellPosition;
    private float period = 1f;

    private int index = 0;
    private readonly Vector3Int[] directions = new Vector3Int[]
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left
    };

    private void Start()
    {
        cellPosition = grid.WorldToCell(transform.position);
        StartCoroutine(MoveInCircles());
    }

    private IEnumerator MoveInCircles()
    {
        while(true)
        {
            index = ++index % directions.Length;
            direction = directions[index];
            cellPosition += direction;
            transform.position = grid.CellToWorld(cellPosition);
            yield return new WaitForSeconds(period);
        }
    }
}
