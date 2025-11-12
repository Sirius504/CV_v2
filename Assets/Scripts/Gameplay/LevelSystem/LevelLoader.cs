using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public async void LoadLevelAsync(int level)
    {
        await SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
    }
}
