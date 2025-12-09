using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    public float damagePerSecond = 10f;
    bool isInWater = false;

    void Update()
    {
        if (isInWater)
        {
            //PlayerHealth.Instance.TakeDamage(damagePerSecond * Time.deltaTime);
            //NPCHealth.Instance.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }

    public void SetWater(bool status)
    {
        isInWater = status;
    }
}
