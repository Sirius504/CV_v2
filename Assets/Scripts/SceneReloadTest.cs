using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneReloadTest : MonoBehaviour
{
    [SerializeField] private Button button;
    private AsyncOperation operation;

    private void Start()
    {
        DontDestroyOnLoad(transform.parent);
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        operation.completed += LoadNextScene;
        //SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    private void LoadNextScene(AsyncOperation operation)
    {
        StartCoroutine(WaitAndLoad());
    }

    private IEnumerator WaitAndLoad()
    {
        yield return null;
        operation = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
