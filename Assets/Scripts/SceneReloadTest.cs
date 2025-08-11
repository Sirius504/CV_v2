using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneReloadTest : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
