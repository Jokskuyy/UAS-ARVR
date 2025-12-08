using System.Collections.Generic;
using UnityEngine;

// Posisi relatif ruangan terhadap living room
public enum RoomSlot
{
    Left,
    Top,
    Right
}

// Satu opsi penempatan ruangan (slot + prefab + offset)
[System.Serializable]
public struct RoomOption
{
    public string name;
    public RoomSlot slot;
    public GameObject prefab;
    public Vector3 offsetFromLiving;
}

// Meng-generate layout rumah dengan living room di tengah
// dan kitchen/bedroom/bathroom diacak di slot Left/Top/Right tanpa saling tumpuk.
public class LayoutGenerator : MonoBehaviour
{
    [Header("Living Room (center)")]
    public GameObject livingRoomPrefab;

    [Header("Kitchen Prefabs")]
    public GameObject kitchenLeftPrefab;
    public GameObject kitchenTopPrefab;
    public GameObject kitchenRightPrefab;

    [Header("Bedroom Prefabs")]
    public GameObject bedroomLeftPrefab;
    public GameObject bedroomTopPrefab;
    public GameObject bedroomRightPrefab;

    [Header("Bathroom Prefabs")]
    public GameObject bathroomLeftPrefab;
    public GameObject bathroomTopPrefab;
    public GameObject bathroomRightPrefab;

    private RoomOption[] kitchenOptions;
    private RoomOption[] bedroomOptions;
    private RoomOption[] bathroomOptions;

    private readonly HashSet<RoomSlot> usedSlots = new HashSet<RoomSlot>();

    private void Start()
    {
        BuildOptionsArrays();
        GenerateLayout();
    }

    // Menyiapkan semua kombinasi ruangan-posisi dengan offset tetap
    private void BuildOptionsArrays()
    {
        kitchenOptions = new RoomOption[]
        {
            new RoomOption
            {
                name = "Kitchen_Left",
                slot = RoomSlot.Left,
                prefab = kitchenLeftPrefab,
                offsetFromLiving = new Vector3(-0.5f, 0f, -7.35f)
            },
            new RoomOption
            {
                name = "Kitchen_Top",
                slot = RoomSlot.Top,
                prefab = kitchenTopPrefab,
                offsetFromLiving = new Vector3(-9.05f, 0f, 0.2f)
            },
            new RoomOption
            {
                name = "Kitchen_Right",
                slot = RoomSlot.Right,
                prefab = kitchenRightPrefab,
                offsetFromLiving = new Vector3(-1.5f, 0f, 8.65f)
            }
        };

        bedroomOptions = new RoomOption[]
        {
            new RoomOption
            {
                name = "Bedroom_Left",
                slot = RoomSlot.Left,
                prefab = bedroomLeftPrefab,
                offsetFromLiving = new Vector3(-0.5f, 0f, -7.35f)
            },
            new RoomOption
            {
                name = "Bedroom_Top",
                slot = RoomSlot.Top,
                prefab = bedroomTopPrefab,
                offsetFromLiving = new Vector3(-9.05f, 0f, 0.2f)
            },
            new RoomOption
            {
                name = "Bedroom_Right",
                slot = RoomSlot.Right,
                prefab = bedroomRightPrefab,
                offsetFromLiving = new Vector3(-1.5f, 0f, 8.65f)
            }
        };

        bathroomOptions = new RoomOption[]
        {
            new RoomOption
            {
                name = "Bathroom_Left",
                slot = RoomSlot.Left,
                prefab = bathroomLeftPrefab,
                offsetFromLiving = new Vector3(-0.5f, 0f, -6.4f)
            },
            new RoomOption
            {
                name = "Bathroom_Top",
                slot = RoomSlot.Top,
                prefab = bathroomTopPrefab,
                offsetFromLiving = new Vector3(-8.2f, 0f, 0.2f)
            },
            new RoomOption
            {
                name = "Bathroom_Right",
                slot = RoomSlot.Right,
                prefab = bathroomRightPrefab,
                offsetFromLiving = new Vector3(-1.5f, 0f, 7.7f)
            }
        };
    }

    // Membuat ulang layout: clear lama, spawn living, lalu acak ruangan lain
    public void GenerateLayout()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        usedSlots.Clear();

        if (livingRoomPrefab == null)
        {
            Debug.LogError("Living Room Prefab is not assigned.");
            return;
        }

        GameObject livingGO = Instantiate(
            livingRoomPrefab,
            Vector3.zero,
            livingRoomPrefab.transform.rotation,
            transform
        );

        Vector3 livingPos = livingGO.transform.position;

        SpawnRandomRoom(livingPos, kitchenOptions);
        SpawnRandomRoom(livingPos, bedroomOptions);
        SpawnRandomRoom(livingPos, bathroomOptions);
    }

    // Memilih satu opsi ruangan secara acak dari slot yang masih kosong lalu spawn
    private void SpawnRandomRoom(Vector3 livingPos, RoomOption[] options)
    {
        if (options == null || options.Length == 0)
            return;

        List<RoomOption> available = new List<RoomOption>();
        foreach (var opt in options)
        {
            if (opt.prefab != null && !usedSlots.Contains(opt.slot))
                available.Add(opt);
        }

        if (available.Count == 0)
            return;

        int index = Random.Range(0, available.Count);
        RoomOption choice = available[index];

        Vector3 worldPos = livingPos + choice.offsetFromLiving;

        // Rotasi berdasarkan slot (bisa kamu sesuaikan sendiri)
        Quaternion rotation = GetRotationForSlot(choice.slot, choice.prefab);

        Instantiate(
            choice.prefab,
            worldPos,
            rotation,
            transform
        );

        usedSlots.Add(choice.slot);
    }

    // Menentukan rotasi prefab berdasarkan slot.
    // Kalau mau tetap pakai rotasi prefab, tinggal return choice.prefab.transform.rotation;
    private Quaternion GetRotationForSlot(RoomSlot slot, GameObject prefab)
    {
        // contoh: atur sesuai kebutuhan arah pintu / dinding
        switch (slot)
        {
            case RoomSlot.Left:
                return Quaternion.Euler(0f, -90f, 0f);
            case RoomSlot.Top:
                return Quaternion.Euler(0f, 0f, 0f);
            case RoomSlot.Right:
                return Quaternion.Euler(0f, 90f, 0f);
            default:
                return prefab.transform.rotation;
        }
    }
}