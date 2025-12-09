using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float waterDamagePerSecond = 10f;

    bool isInWater = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isInWater)
        {
            currentHealth -= waterDamagePerSecond * Time.deltaTime;

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetWater(bool status)
    {
        isInWater = status;
    }
}
