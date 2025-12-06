using UnityEngine;

public class TestSendScore : MonoBehaviour
{
    [SerializeField] private GameResultUploader uploader;

    private void Start()
    {
        // kirim sekali saat Play, dengan data dummy
        StartCoroutine(uploader.SendGameResult(
            timeFindGrandma: 80f,
            timeFinishGame: 240f,
            playerHealthEnd: 85f,
            grandmaHealthEnd: 90f,
            priorityItemsSaved: 3,
            totalPriorityItems: 4
        ));
    }
}