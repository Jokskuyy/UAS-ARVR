
# Proyek UAS ARVR

Dokumentasi ini mencakup pembaruan terbaru terkait integrasi UI Layer, mekanisme scoring, dan koneksi ke backend Machine Learning yang telah di-deploy.

## Struktur Folder Unity Terbaru

Berikut adalah susunan folder dan file dalam proyek saat ini:

```
Unity/
├── Assets/
│   ├── Image/                # (BARU) Berisi aset gambar (sprite) untuk ikon item di UI
│   ├── Prefabs/              
│   ├── Scenes/               
│   │   └── InfoTotal+ScoreUILayer.unity # (BARU) Scene utama untuk testing gameplay dan kalkulasi UI
│   ├── Scripts/
│   │   ├── GerakanPlayer1.cs # (BARU) Script interaksi player (Grab/Move) untuk testing logika item
│   │   ├── LayoutGenerator/  # 
│   │   ├── Scoring/          
│   │   │   ├── GameManager.cs      
│   │   │   └── GameResultUploader.cs 
│   │   ├── UIManager/        # (BARU) Folder khusus manajemen UI
│   │   │   ├── FinishScript.cs       # Trigger zona finish & kirim data akhir
│   │   │   ├── NenekTriggerDummy.cs  # Dummy trigger untuk mencatat waktu penemuan nenek
│   │   │   ├── PlayerHealth.cs       # Mengatur logika pengurangan darah & update bar UI
│   │   │   └── ScoreUIManager.cs     # Manager utama tampilan angka & panel skor ke layar
│   │   └── Untuk Air/        
│   └── Settings/             
├── Packages/                 
└── ProjectSettings/          
```

## Pembaruan 

**1. Penambahan Aset Visual (Assets/Image/)**
- Menambahkan folder Image yang berisi sprite/ikon untuk UI (Dokumen, Barang Elektronik, Barang Penting, Racun, Timer, dll) agar tampilan layer UI lebih informatif.

**2. Scene Baru (InfoTotal+ScoreUILayer.unity)**
- Menambahkan scene baru yang siap dimainkan (playable).
- Scene ini sudah terintegrasi dengan sistem kalkulasi item, pengurangan health, dan panel total skor akhir.

**3. Script Interaksi Player (GerakanPlayer1.cs)**
- Menambahkan script GerakanPlayer1.cs untuk simulasi interaksi Grab (mengambil item).
- Catatan: Script ini berfungsi sebagai debugger untuk memastikan logika UI berjalan (item hilang saat diambil -> skor bertambah). Pada implementasi final, script ini dapat digantikan dengan script XR Interaction yang sebenarnya.

**4. Implementasi UI Manager (Scripts/UIManager/)**
Menambahkan folder khusus untuk mengelola logika antarmuka:
- ScoreUIManager.cs: Script sentral yang menampilkan total item yang diambil, timer, health bar, dan memunculkan Panel Game Over.
- FinishScript.cs: Script dummy trigger pada zona evakuasi. Untuk mengecek saat pemain masuk, script ini akan memicu perhitungan skor akhir.
- NenekTriggerDummy.cs: Script dummy untuk mendeteksi kapan pemain menemukan NPC Nenek dan mencatat waktunya ke sistem.
- PlayerHealth.cs: Menghitung pengurangan darah berdasarkan variabel bahaya (listrik/racun) dan memperbarui Slider Health di UI.

5. Pembaruan Logika GameManager.cs
- Singleton Pattern: Menambahkan public static GameManager Instance agar script ini mudah diakses dari script UI manapun.
- Auto-Fetch Data (OnGameEnd): Fungsi OnGameEnd diperbarui agar tidak lagi membutuhkan parameter manual. Script ini sekarang otomatis mengambil data (Sisa Darah, Total Item) langsung dari ScoreUIManager saat game berakhir.
- Catatan: Variabel Health Nenek saat ini masih di-set default 100.

6. Integrasi Backend Publik (GameResultUploader.cs)
- Public URL: Mengubah endpoint API dari localhost menjadi URL publik Hugging Face agar bisa diakses dari mana saja tanpa menyalakan server lokal.
- URL Baru: https://jianjoyland-uas-arvr.hf.space/predict-score
-  Menambahkan logika untuk otomatis memanggil ScoreUIManager.TampilkanScoreAkhir() setelah menerima respon skor prediksi dari server Python.

⚙️ Cara Menjalankan Scene
- Buka Scene InfoTotal+ScoreUILayer.unity.
- Pastikan koneksi internet aktif (untuk mengirim data ke Backend Hugging Face).
- Tekan Play.
- Gunakan kontrol (WASD/Mouse) untuk bergerak dan Klik Kanan untuk mengambil item.
- Masuk ke area Finish atau biarkan darah habis untuk melihat Panel Skor Akhir.

