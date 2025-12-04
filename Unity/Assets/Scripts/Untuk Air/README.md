# 🌊 **Environment & Water System Module**

Modul ini menangani seluruh simulasi lingkungan terkait **banjir** di dalam game. Sistem ini mencakup pengaturan kenaikan air, efek objek rusak saat terendam, fisika mengapung, hingga perhitungan status bahaya global (air nyetrum / beracun) yang dapat diakses oleh Player dan NPC.

---

## 📂 **Script Manifest**

### **1. `FloodController.cs` — Core Manager (Singleton)**

* Mengatur ketinggian permukaan air (`Y-Axis`) dan seluruh state banjir.
* Menjadi sumber data utama untuk modul lain.
* **Wajib** ditempatkan pada objek *Water Plane*.

### **2. `HazardItem.cs` — Global Hazard State Logic**

* Menghitung jumlah sumber bahaya aktif:

  * **Elektrik** (TV, kulkas, kabel rusak)
  * **Kimia** (limbah, drum berbahaya)
* Dipasang pada setiap objek yang dapat menjadi sumber bahaya.
* Terintegrasi otomatis dengan sistem banjir.

### **3. `DissolvingItem.cs` — Item Durability**

* Menangani objek yang akan rusak / larut jika terendam terlalu lama.
* Memiliki HP dan sistem Damage Over Time.
* Cocok untuk dokumen penting atau barang misi.

### **4. `SimpleBuoyancy.cs` — Basic Floating Physics**

* Memberikan efek mengapung sederhana pada objek Rigidbody.
* Dapat digunakan untuk perahu, papan kayu, atau puing-puing.

### **5. `PlayerHealthTest.cs` — Debug Utility**

* Script dummy untuk simulasi damage ke Player.
* **Hanya untuk test lokal**, jangan dipakai di Scene Utama/Build.

---

# 📘 **API Reference**

## 1. **Mengambil Ketinggian Air**

```csharp
float surfaceY = FloodController.Instance.CurrentWaterLevel;

if (transform.position.y < surfaceY)
{
    // Karakter berada di dalam air
}
```

---

## 2. **Mengecek Bahaya Listrik**

```csharp
int electricSources = HazardItem.ActiveElectricHazards;

if (electricSources > 0)
{
    // Air sedang mengalirkan listrik
    // float damage = baseDamage * electricSources * Time.deltaTime;
}
```

---

## 3. **Mengecek Bahaya Racun**

```csharp
int toxicSources = HazardItem.ActiveToxicHazards;

if (toxicSources > 0)
{
    // Air sedang membawa racun
}
```

---

# 🛠️ **Setup Guide**

### **Step 1 — Setup Air (Wajib)**

1. Buat objek Plane 3D (Water Surface)
2. Beri material transparan (Air)
3. Tambahkan `FloodController.cs`
4. Atur parameter:

   * **Rise Speed** (contoh: `0.1`)
   * **Max Height** (contoh: `3.0`)
5. Opsional: uncheck **Show Debug Logs** untuk mengurangi spam console.

---

### **Step 2 — Setup Elektronik Berbahaya**

1. Pilih objek (contoh: TV)
2. Tambahkan `HazardItem.cs`
3. Atur:

   * **Hazard Type = Electric**

Jika objek diangkat keluar air, sistem otomatis memperbarui status bahaya global.

---

### **Step 3 — Setup Dokumen / Barang Sensitif**

1. Pilih objek (contoh: Map Kertas)
2. Tambahkan `DissolvingItem.cs`
3. Atur:

   * **Max HP**
   * **Damage Per Second**

Jika HP habis → objek akan **Destroy()** otomatis.

---

### **Step 4 — Setup Objek Mengapung**

1. Tambahkan **Rigidbody**
2. Pastikan **Use Gravity** aktif
3. Tambahkan `SimpleBuoyancy.cs`
4. Pastikan objek diletakkan di atas lantai saat *Start()*

---

# 🐞 **Troubleshooting**

### **1. Console terlalu banyak spam “Water Level Update”?**

➝ Klik objek WaterPlane → uncheck **Show Debug Logs**.

---

### **2. NullReferenceException pada FloodController**

➝ Pastikan hanya ada **1 objek** dalam Scene yang memiliki `FloodController`.

---

### **3. Objek mengapung tidak bergerak**

Cek:

* Apakah air sudah menyentuh objek?
* Apakah objek tidak menembus lantai saat Start?

---

### **4. Mau tes damage tanpa Player asli**

➝ Pakai `PlayerHealthTest.cs` pada Capsule untuk simulasi.

---

# ⚠️ **PENTING UNTUK MERGE**

> **Jangan mengubah logika internal `HazardItem.cs` tanpa diskusi.**
> Script ini mengatur *Global Hazard State* yang digunakan oleh seluruh sistem Player dan NPC.
