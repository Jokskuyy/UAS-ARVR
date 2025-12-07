using UnityEngine;

// Mengumpulkan data hasil permainan dan mengirim ke backend untuk scoring.
public class GameManager : MonoBehaviour
{
<<<<<<< HEAD
    //Tambahan agar langsung bisa ambil dari ScoreUIManager
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


=======
>>>>>>> 716a9cb2f7a85c9172926284f027285522941f5b
    [Header("Networking")]
    [SerializeField] private GameResultUploader resultUploader;

    [Header("Scoring Settings")]
    [SerializeField] private int totalPriorityItems = 4;

    private float floodStartTime;
    private float grandmaFoundTime;
    private float gameEndTime;

    private int priorityItemsSaved;
    private float playerHealthEnd;
    private float grandmaHealthEnd;

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

<<<<<<< HEAD
    // // Panggil ini ketika game benar-benar selesai (menang / kalah)
    // public void OnGameEnd(float playerHp, float grandmaHp, int savedItems)
    // {
    //     if (gameEnded) return;

    //     gameEnded = true;

    //     gameEndTime = Time.time;
    //     playerHealthEnd = Mathf.Clamp(playerHp, 0f, 100f);
    //     grandmaHealthEnd = Mathf.Clamp(grandmaHp, 0f, 100f);
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
    //             grandmaHealthEnd,
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

        float finalPlayerHealth = 100f;
        float finalGrandmaHealth = 100f; // Default 100 (Karena belum ada sistem damage nenek)
        int finalPriorityItems = 0;

        // --- 2. AMBIL DATA DARI UI MANAGER ---
        if (ScoreUIManager.Instance != null)
        {
            // Ambil Darah Player
            finalPlayerHealth = ScoreUIManager.Instance.CurrentHealth;
            
            // Ambil Jumlah Barang (Dokumen + Barang Penting)
            finalPriorityItems = ScoreUIManager.Instance.CountDokumen + ScoreUIManager.Instance.CountBarangPenting;
            
            // Stop Timer Visual
            ScoreUIManager.Instance.StopTimer();
        }

        // --- 3. RAPIKAN DATA (CLAMP) ---
        finalPlayerHealth = Mathf.Clamp(finalPlayerHealth, 0f, 100f);
        finalPriorityItems = Mathf.Clamp(finalPriorityItems, 0, totalPriorityItems);

        // Hitung Waktu
        float timeFindGrandma = (grandmaFound ? grandmaFoundTime : gameEndTime) - floodStartTime;
        float timeFinishGame = gameEndTime - floodStartTime;

        Debug.Log($"[AUTO-FETCH] Final HP: {finalPlayerHealth}, Saved Items: {finalPriorityItems}");

        // --- 4. KIRIM KE UPLOADER ---
=======
    // Panggil ini ketika game benar-benar selesai (menang / kalah)
    public void OnGameEnd(float playerHp, float grandmaHp, int savedItems)
    {
        if (gameEnded) return;

        gameEnded = true;

        gameEndTime = Time.time;
        playerHealthEnd = Mathf.Clamp(playerHp, 0f, 100f);
        grandmaHealthEnd = Mathf.Clamp(grandmaHp, 0f, 100f);
        priorityItemsSaved = Mathf.Clamp(savedItems, 0, totalPriorityItems);

        float timeFindGrandma = (grandmaFound ? grandmaFoundTime : gameEndTime) - floodStartTime;
        float timeFinishGame = gameEndTime - floodStartTime;

        Debug.Log($"[ScoreDebug] timeFindGrandma={timeFindGrandma}, timeFinishGame={timeFinishGame}, " +
                  $"playerHP={playerHealthEnd}, grandmaHP={grandmaHealthEnd}, " +
                  $"prioritySaved={priorityItemsSaved}/{totalPriorityItems}");

>>>>>>> 716a9cb2f7a85c9172926284f027285522941f5b
        if (resultUploader != null)
        {
            StartCoroutine(resultUploader.SendGameResult(
                timeFindGrandma,
                timeFinishGame,
<<<<<<< HEAD
                finalPlayerHealth,   // Pakai variable lokal yg baru
                finalGrandmaHealth,  // Pakai variable lokal (100)
                finalPriorityItems,  // Pakai variable lokal hasil penjumlahan
=======
                playerHealthEnd,
                grandmaHealthEnd,
                priorityItemsSaved,
>>>>>>> 716a9cb2f7a85c9172926284f027285522941f5b
                totalPriorityItems
            ));
        }
        else
        {
            Debug.LogWarning("[ScoreDebug] ResultUploader is not assigned on GameManager.");
        }
    }
}