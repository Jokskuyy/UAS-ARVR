using UnityEngine;

// Mengumpulkan data hasil permainan dan mengirim ke backend untuk scoring.
public class GameManager : MonoBehaviour
{
    //Tambahan agar langsung bisa ambil dari ScoreUIManager
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    [Header("Networking")]
    [SerializeField] private GameResultUploader resultUploader;

    [Header("Scoring Settings")]
    [SerializeField] private int totalPriorityItems = 4;

    private float floodStartTime;
    private float grandmaFoundTime;
    private float gameEndTime;

    private int priorityItemsSaved;
    private float playerHealthEnd;

    private bool grandmaFound;
    private bool gameEnded;

    private void Start()
    {
        floodStartTime = Time.time;
    }

    // Panggil ini ketika nenek ditemukan pertama kali
    public void OnGrandmaFound()
    {
        if (grandmaFound) return;

        grandmaFound = true;
        grandmaFoundTime = Time.time;
    }

    // // Panggil ini ketika game benar-benar selesai (menang / kalah)
    // public void OnGameEnd(float playerHp, float grandmaHp, int savedItems)
    // {
    //     if (gameEnded) return;

    //     gameEnded = true;

    //     gameEndTime = Time.time;
    //     playerHealthEnd = Mathf.Clamp(playerHp, 0f, 100f);
    //     priorityItemsSaved = Mathf.Clamp(savedItems, 0, totalPriorityItems);

    //     float timeFindGrandma = (grandmaFound ? grandmaFoundTime : gameEndTime) - floodStartTime;
    //     float timeFinishGame = gameEndTime - floodStartTime;

    //     Debug.Log($"[ScoreDebug] timeFindGrandma={timeFindGrandma}, timeFinishGame={timeFinishGame}, " +
    //               $"playerHP={playerHealthEnd}, grandmaHP={grandmaHealthEnd}, " +
    //               $"prioritySaved={priorityItemsSaved}/{totalPriorityItems}");

    //     if (resultUploader != null)
    //     {
    //         StartCoroutine(resultUploader.SendGameResult(
    //             timeFindGrandma,
    //             timeFinishGame,
    //             playerHealthEnd,
    //             priorityItemsSaved,
    //             totalPriorityItems
    //         ));
    //     }
    //     else
    //     {
    //         Debug.LogWarning("[ScoreDebug] ResultUploader is not assigned on GameManager.");
    //     }
    // }
    //Diupdate agar otomatis ambil dari UI
    public void OnGameEnd()
    {
        if (gameEnded) return;
        gameEnded = true;
        gameEndTime = Time.time;

        // 1. GAME MANAGER OTOMATIS AMBIL DATA DARI UI
        float finalHP = 100f;
        int totalSaved = 0;

        if (ScoreUIManager.Instance != null)
        {
            // Ambil Darah Terakhir
            finalHP = ScoreUIManager.Instance.CurrentHealth;
            
            // LOGIKA GABUNGAN: Dokumen + Berlian dijumlah disini
            totalSaved = ScoreUIManager.Instance.CountDokumen + ScoreUIManager.Instance.CountBarangPenting;
            
            // Stop Timer di UI
            ScoreUIManager.Instance.StopTimer();
        }

        // 2. HITUNG WAKTU & RAPIKAN DATA
        finalHP = Mathf.Clamp(finalHP, 0f, 100f);
        totalSaved = Mathf.Clamp(totalSaved, 0, totalPriorityItems);

        float timeFindGrandma = (grandmaFound ? grandmaFoundTime : gameEndTime) - floodStartTime;
        float timeFinishGame = gameEndTime - floodStartTime;

        Debug.Log($"[AUTO-FETCH] HP: {finalHP}, Items: {totalSaved} (Doc+BarangPenting)");

        // 3. KIRIM KE UPLOADER 
        if (resultUploader != null)
        {
            StartCoroutine(resultUploader.SendGameResult(
                timeFindGrandma,
                timeFinishGame,
                finalHP,
                totalSaved,
                totalPriorityItems
            ));
        }
    }
}