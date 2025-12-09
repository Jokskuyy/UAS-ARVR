using UnityEngine;

public enum ItemType
{
    Elektronik,
    BarangPenting,
    Racun,
    Dokumen
}

public class addItemPoint : MonoBehaviour
{
    [Header("Item Settings")]
    [Tooltip("Pilih jenis item yang akan ditambahkan")]
    public ItemType itemType = ItemType.Elektronik;

    [Header("Trigger Settings")]
    [Tooltip("Tag yang akan men-trigger penambahan item")]
    public string triggerTag = "Finish";

    [Tooltip("Hancurkan object setelah di-trigger?")]
    public bool destroyAfterTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            // Panggil fungsi sesuai item type yang dipilih
            AddItemByType();

            // Hancurkan object jika diset true
            if (destroyAfterTrigger)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Menambahkan item sesuai tipe yang dipilih di Inspector
    /// </summary>
    private void AddItemByType()
    {
        switch (itemType)
        {
            case ItemType.Elektronik:
                AddElektronik();
                break;
            case ItemType.BarangPenting:
                AddBarangPenting();
                break;
            case ItemType.Racun:
                AddRacun();
                break;
            case ItemType.Dokumen:
                AddDokumen();
                break;
        }
    }
    
    public void AddElektronik()
    {
        if (ScoreUIManager.Instance != null)
        {
            ScoreUIManager.Instance.AddElektronik();
            Debug.Log("Item Elektronik ditambahkan!");
        }
        else
        {
            Debug.LogWarning("ScoreUIManager.Instance tidak ditemukan! Pastikan ScoreUIManager ada di scene.");
        }
    }

    public void AddBarangPenting()
    {
        if (ScoreUIManager.Instance != null)
        {
            ScoreUIManager.Instance.AddBarangPenting();
            Debug.Log("Item Barang Penting ditambahkan!");
        }
        else
        {
            Debug.LogWarning("ScoreUIManager.Instance tidak ditemukan! Pastikan ScoreUIManager ada di scene.");
        }
    }

    public void AddRacun()
    {
        if (ScoreUIManager.Instance != null)
        {
            ScoreUIManager.Instance.AddRacun();
            Debug.Log("Item Racun ditambahkan!");
        }
        else
        {
            Debug.LogWarning("ScoreUIManager.Instance tidak ditemukan! Pastikan ScoreUIManager ada di scene.");
        }
    }

    public void AddDokumen()
    {
        if (ScoreUIManager.Instance != null)
        {
            ScoreUIManager.Instance.AddDokumen();
            Debug.Log("Item Dokumen ditambahkan!");
        }
        else
        {
            Debug.LogWarning("ScoreUIManager.Instance tidak ditemukan! Pastikan ScoreUIManager ada di scene.");
        }
    }
}
