using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Processors List", menuName = "Sprite Processing/Processors List")]
public class SpriteProcessorsList : ScriptableObject
{
    [SerializeField] private List<KeyValuePair<ActionBehaviour.Action, SpriteActionProcessor>> _processors;

    public IReadOnlyList<KeyValuePair<ActionBehaviour.Action, SpriteActionProcessor>> Processors => _processors;

    public bool TryGetProcessor(ActionBehaviour.Action action, out SpriteActionProcessor processor)
    {
        processor = _processors.FirstOrDefault(kvp => kvp.key == action)?.value;
        return processor != null;
    }
}
