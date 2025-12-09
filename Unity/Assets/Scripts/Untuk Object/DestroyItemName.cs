using UnityEngine;

public class DestroyItemName : MonoBehaviour
{
    [Header("Destroy Settings")]
    [Tooltip("Prefix of GameObject names to destroy")] 
    public string namePrefix;

    [Tooltip("Root transform to search under. If empty, uses this.transform.")]
    public Transform searchRoot;

    [Tooltip("Destroy on Start (otherwise call DestroyMatching manually)")]
    public bool destroyOnStart = true;
    public bool destroyObject = false;

    void Start()
    {
        if (destroyOnStart)
        {
            DestroyMatching();
        }
    }
    void Update()
    {
        if (destroyObject)
        {
            DestroyMatching();
        }
    }

    /// <summary>
    /// Destroys all child GameObjects under the specified root whose names start with the given prefix.
    /// </summary>
    public void DestroyMatching()
    {
        if (string.IsNullOrEmpty(namePrefix))
        {
            Debug.LogWarning("[DestroyItemName] Name prefix is empty. Nothing will be destroyed.");
            return;
        }

        Transform root = searchRoot != null ? searchRoot : transform;
        Transform[] allChildren = root.GetComponentsInChildren<Transform>(true);
        int destroyedCount = 0;
        foreach (Transform t in allChildren)
        {
            if (t == root) continue;
            if (t.name.StartsWith(namePrefix))
            {
                Destroy(t.gameObject);
                destroyedCount++;
            }
        }
        Debug.Log($"[DestroyItemName] Destroyed {destroyedCount} GameObjects with prefix '{namePrefix}' under '{root.name}'.");
    }
}
