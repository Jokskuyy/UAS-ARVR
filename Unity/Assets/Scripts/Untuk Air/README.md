# 🌊 Module: Environment & Water System

## 📖 Overview

Modul ini bertanggung jawab atas seluruh simulasi lingkungan terkait banjir. Modul ini menangani kenaikan air secara fisik, kerusakan objek (dokumen basah), fisika benda mengapung, dan perhitungan global status bahaya (air nyetrum/beracun) yang dapat diakses oleh modul Player dan NPC.

---

## 📂 File Manifest (Script List)

Berikut adalah daftar skrip yang terdapat dalam modul ini beserta fungsinya:

1.  **`FloodController.cs` (Core Manager)**
    - _Role:_ Singleton Manager yang mengatur ketinggian air (`Y-Axis`) dan state banjir.
    - _Usage:_ Wajib dipasang di objek Plane Air.
2.  **`HazardItem.cs` (Global State Logic)**
    - _Role:_ Menghitung jumlah sumber bahaya (Listrik/Kimia) yang sedang aktif terendam air.
    - _Usage:_ Dipasang di objek elektronik (TV, Kulkas) atau drum limbah.
3.  **`DissolvingItem.cs` (Object Logic)**
    - _Role:_ Menangani barang yang memiliki "HP" dan akan hancur jika terendam air terlalu lama.
    - _Usage:_ Dipasang di Dokumen Penting atau Barang Berharga.
4.  **`SimpleBuoyancy.cs` (Physics Logic)**
    - _Role:_ Memberikan efek mengapung sederhana pada objek Rigidbody saat menyentuh permukaan air.
    - _Usage:_ Dipasang di perahu, kayu, atau puing-puing.
5.  **`PlayerHealthTest.cs` (Debug Tool)**
    - _Role:_ Script dummy untuk simulasi pengurangan darah Player.
    - _Note:_ **HANYA UNTUK TESTING LOKAL. JANGAN GUNAKAN DI SCENE UTAMA/BUILD.**

---

### 1. Mendapatkan Ketinggian Air (Water Level)

Gunakan ini untuk mengecek apakah kaki karakter kalian menyentuh air.

```csharp
// Mengambil posisi Y permukaan air saat ini (float)
float surfaceY = FloodController.Instance.CurrentWaterLevel;

// Contoh Implementasi di Script Player/NPC:
if (transform.position.y < surfaceY)
{
    // Logic: Karakter sedang basah / berjalan dalam air
    // Bisa digunakan untuk trigger animasi "Slow Walk" atau "Panic"
}
```
