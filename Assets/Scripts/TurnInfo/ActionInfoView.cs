using System.Collections.Generic;
using UnityEngine;

public class ActionInfoView : MonoEntity
{
    [SerializeField] private Arrow _arrowPrefab;
    private readonly List<Arrow> _arrows = new(4);

    public InitOrder InitOrder => InitOrder.UI;


    public void Set(ActionInfo[] actionInfos)
    {
        while (_arrows.Count > actionInfos.Length)
        {
            var arrowToRemove = _arrows[^1];
            _arrows.RemoveAt(_arrows.Count - 1);
            Destroy(arrowToRemove.gameObject);
        }

        while (_arrows.Count < actionInfos.Length)
        {
            var newArrow = Instantiate(_arrowPrefab, transform);
            _arrows.Add(newArrow);
        }

        for (var i = 0; i < actionInfos.Length; i++)
        {
            _arrows[i].Set(actionInfos[i]);
        }
    }
}