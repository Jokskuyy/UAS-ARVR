using UnityEngine;
using UnityEngine.UI;
using TMPro; // Wajib untuk TextMeshPro

public class ScoreUIManager : MonoBehaviour
{
    public static ScoreUIManager Instance;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;       // Tempat naruh Panel Hitam
    public TextMeshProUGUI textFinalScore; // Tempat naruh Angka Skor Akhir

    [Header("Container Kiri")]
    public Slider healthBarSlider;
    public TextMeshProUGUI textTimer;
    public TextMeshProUGUI textTimerNenek; 

    [Header("Counter Barang")]
    public TextMeshProUGUI textCountDokumen;     
    public TextMeshProUGUI textCountElektronik;  
    public TextMeshProUGUI textCountBarangPenting;     
    public TextMeshProUGUI textCountRacun;       

    // Data Tersimpan
    public int CountDokumen { get; private set; }
    public int CountElektronik { get; private set; }
    public int CountBarangPenting { get; private set; }
    public int CountRacun { get; private set; }
    public float CurrentHealth { get; private set; } = 100f;

    private float startTime;
    private bool timerBerjalan = true;

    private void Awake() { Instance = this; }

    private void Start()
    {
        startTime = Time.time;
        if(healthBarSlider) { healthBarSlider.maxValue = 100; healthBarSlider.value = 100; }
        UpdateUI();
        if(textTimerNenek) textTimerNenek.text = "00:00";
        
        // Pastikan panel mati pas mulai
        if(gameOverPanel) gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (timerBerjalan && textTimer != null)
        {
            float w = Time.time - startTime;
            string minutes = Mathf.Floor(w / 60).ToString("00");
            string seconds = (w % 60).ToString("00");
            textTimer.text = $"{minutes}:{seconds}";
        }
    }

    // public void TampilkanScoreAkhir(float scoreDariBackend)
    // {
    //     // 1. Munculkan Panel
    //     if(gameOverPanel) gameOverPanel.SetActive(true);

    //     // 2. Tulis Scorenya (Format 2 angka desimal)
    //     if(textFinalScore) textFinalScore.text = scoreDariBackend.ToString("F2"); 

    //     // 3. Matikan Timer & Munculkan Mouse
    //     StopTimer();
    //     Cursor.lockState = CursorLockMode.None;
    //     Cursor.visible = true;
    // }
    // // Di dalam SCRIPT ScoreUIManager.cs
    public void TampilkanScoreAkhir(float scoreDariBackend) {
    Debug.Log($"[UI_CHECK] Menerima skor: {scoreDariBackend}.");
    
    // --- CEK VARIABEL DENGAN LOG ---
    if (gameOverPanel == null)
    {
        Debug.LogError("[UI_CHECK] GAGAL: Variabel gameOverPanel TIDAK TERISI di Inspector! (NULL)");
    }
    else
    {
        Debug.Log("[UI_CHECK] SUKSES: Variabel gameOverPanel TERISI. Mengaktifkan Panel...");
        gameOverPanel.SetActive(true); // <--- Baris yang harus jalan
    }
    
    // 2. Tulis Scorenya (Format 2 angka desimal)
    if(textFinalScore) textFinalScore.text = scoreDariBackend.ToString("F2"); 

    // 3. Matikan Timer & Munculkan Mouse
    StopTimer();
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    }
    

    public void StopTimer() 
    { 
        timerBerjalan = false; 
    }

    // --- FUNGSI UPDATE WAKTU NENEK ---
    public void SetWaktuNenek(float waktuDetik)
    {
        if (textTimerNenek != null)
        {
            string minutes = Mathf.Floor(waktuDetik / 60).ToString("00");
            string seconds = (waktuDetik % 60).ToString("00");
            textTimerNenek.text = $"{minutes}:{seconds}";
        }
    }
    public void AddDokumen() 
    { 
        CountDokumen++; 
        UpdateUI(); 
    }

    public void AddElektronik() 
    { 
        CountElektronik++; 
        UpdateUI(); 
    }

    public void AddBarangPenting() 
    { 
        CountBarangPenting++; 
        UpdateUI(); 
    }

    public void AddRacun() 
    { 
        CountRacun++; 
        UpdateUI(); 
    }

    public void UpdateHealth(float val)
    {
        CurrentHealth = val;
        if (healthBarSlider) healthBarSlider.value = CurrentHealth;
    }

    private void UpdateUI()
    {
        if (textCountDokumen) textCountDokumen.text = CountDokumen.ToString();
        if (textCountElektronik) textCountElektronik.text = CountElektronik.ToString();
        if (textCountBarangPenting) textCountBarangPenting.text = CountBarangPenting.ToString();
        if (textCountRacun) textCountRacun.text = CountRacun.ToString();
    }
}
