using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script untuk mengganti scene
/// Bisa dipanggil dari button, trigger, atau script lain
/// </summary>
public class ChangeScene : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Nama scene yang akan di-load (harus ada di Build Settings)")]
    public string sceneName = "";

    [Tooltip("Atau gunakan index scene dari Build Settings")]
    public int sceneIndex = -1;

    [Header("Optional Settings")]
    [Tooltip("Delay sebelum pindah scene (dalam detik)")]
    public float delayBeforeLoad = 0f;

    /// <summary>
    /// Load scene berdasarkan nama yang diset di Inspector
    /// </summary>
    public void LoadSceneByName()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name kosong! Isi nama scene di Inspector.");
            return;
        }

        if (delayBeforeLoad > 0)
        {
            Invoke(nameof(LoadSceneByNameDelayed), delayBeforeLoad);
        }
        else
        {
            LoadSceneByNameDelayed();
        }
    }

    /// <summary>
    /// Load scene berdasarkan index yang diset di Inspector
    /// </summary>
    public void LoadSceneByIndex()
    {
        if (sceneIndex < 0)
        {
            Debug.LogError("Scene index tidak valid! Isi scene index di Inspector (0 atau lebih).");
            return;
        }

        if (delayBeforeLoad > 0)
        {
            Invoke(nameof(LoadSceneByIndexDelayed), delayBeforeLoad);
        }
        else
        {
            LoadSceneByIndexDelayed();
        }
    }

    /// <summary>
    /// Load scene dengan nama custom (bisa dipanggil dari script lain)
    /// </summary>
    /// <param name="customSceneName">Nama scene yang ingin di-load</param>
    public void LoadSceneByCustomName(string customSceneName)
    {
        if (string.IsNullOrEmpty(customSceneName))
        {
            Debug.LogError("Custom scene name kosong!");
            return;
        }

        Debug.Log($"Loading scene: {customSceneName}");
        SceneManager.LoadScene(customSceneName);
    }

    /// <summary>
    /// Reload scene yang sedang aktif
    /// </summary>
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log($"Reloading scene: {currentScene.name}");
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// Quit aplikasi (untuk build)
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Internal methods untuk delayed loading
    private void LoadSceneByNameDelayed()
    {
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    private void LoadSceneByIndexDelayed()
    {
        Debug.Log($"Loading scene by index: {sceneIndex}");
        SceneManager.LoadScene(sceneIndex);
    }
}
