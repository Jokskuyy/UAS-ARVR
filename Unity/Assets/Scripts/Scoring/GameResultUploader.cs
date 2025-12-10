using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// Mengirim data hasil game ke backend Flask untuk diprediksi scorenya.
public class GameResultUploader : MonoBehaviour
{
    [Header("Backend Settings")]
    [SerializeField] private string backendUrl = "https://jianjoyland-uas-arvr.hf.space/predict-score";

    // Panggil coroutine ini ketika game selesai
    public IEnumerator SendGameResult(
        float timeFindGrandma,
        float timeFinishGame,
        float playerHealthEnd,
        int priorityItemsSaved,
        int totalPriorityItems)
    {
        GameResultPayload payload = new GameResultPayload
        {
            time_find_grandma = timeFindGrandma,
            time_finish_game = timeFinishGame,
            player_health_end = playerHealthEnd,
            priority_items_saved = priorityItemsSaved,
            total_priority_items = totalPriorityItems
        };

        string json = JsonUtility.ToJson(payload);
        Debug.Log($"[ScoreDebug] Request JSON: {json}");

        using (UnityWebRequest www = new UnityWebRequest(backendUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            bool hasError = www.result == UnityWebRequest.Result.ConnectionError ||
                            www.result == UnityWebRequest.Result.ProtocolError;
#else
            bool hasError = www.isNetworkError || www.isHttpError;
#endif

            if (hasError)
            {
                Debug.LogError($"[ScoreDebug] HTTP error: {www.responseCode} - {www.error}");
                Debug.LogError($"[ScoreDebug] Response text: {www.downloadHandler.text}");
                yield break;
            }

            string responseText = www.downloadHandler.text;
            Debug.Log($"[ScoreDebug] Response JSON: {responseText}");

            ScoreResponse response = JsonUtility.FromJson<ScoreResponse>(responseText);
            Debug.Log($"[ScoreDebug] Score from backend = {response.score}");

            // --- TAMBAH 4 BARIS INI UNTUK MUNCULKAN PANEL ---
            if (ScoreUIManager.Instance != null)
            {
                ScoreUIManager.Instance.TampilkanScoreAkhir(response.score);
            }
        }
    }

    [System.Serializable]
    private class GameResultPayload
    {
        public float time_find_grandma;
        public float time_finish_game;
        public float player_health_end;
        public int priority_items_saved;
        public int total_priority_items;
    }

    [System.Serializable]
    private class ScoreResponse
    {
        public float score;
    }
}