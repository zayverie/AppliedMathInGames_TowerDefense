using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp = 20f;
    public float currentHp;
    public UIManager uiManagerScript;

    public int coins = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        if(uiManagerScript == null)
        {
            uiManagerScript = FindObjectOfType<UIManager>();
        }

        if(uiManagerScript != null)
        {
            uiManagerScript.UpdateHealthBar(currentHp, maxHp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHp <= 0)
        {
            uiManagerScript.LoseGame();
            Debug.Log("Player has been defeated!");
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); // Ensure HP doesn't 

        if(uiManagerScript != null)
        {
            uiManagerScript.UpdateHealthBar(currentHp, maxHp);
        }
    }
}
