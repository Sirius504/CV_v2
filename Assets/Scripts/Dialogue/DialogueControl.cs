using UnityEngine;
using VContainer;
using Yarn.Unity;

public class DialogueControl : MonoBehaviour
{
    [Inject]
    private DialogueRunner dialogueRunner;
    [SerializeField]
    private string startDialogueNode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            dialogueRunner.StartDialogue(startDialogueNode);
        }
    }
}
