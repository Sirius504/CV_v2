using System.Threading;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class CustomLinePresenter : DialoguePresenterBase
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private DialogueRunner _dialogueRunner;
    [SerializeField] private TextMeshProUGUI _textLine;

    public override YarnTask OnDialogueCompleteAsync()
    {
        _canvasGroup.alpha = 0f;
        return YarnTask.CompletedTask;
    }

    public override YarnTask OnDialogueStartedAsync()
    {
        _canvasGroup.alpha = 1f;
        return YarnTask.CompletedTask;
    }

    public override async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
    {
        _textLine.text = line.RawText;
        await YarnTask.WaitUntilCanceled(token.NextLineToken).SuppressCancellationThrow();
        //throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _dialogueRunner.RequestNextLine();
        }
    }

    public override YarnTask<DialogueOption> RunOptionsAsync(DialogueOption[] dialogueOptions, CancellationToken cancellationToken)
    {
        return YarnTask<DialogueOption?>.FromResult(null);
    }
}
