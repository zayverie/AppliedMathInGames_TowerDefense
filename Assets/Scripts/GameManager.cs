using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Enemy enemyScript;
    public UIManager uiManagerScript;
    public Sniper sniperScript;
    public FlameShooter flameShooterScript;
    public Shotgun shotgunScript;
    public GameObject sharedPoint;

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        WinCondition();
    }

    public void WinCondition()
    {
        if (enemyScript == null)
        {
            enemyScript = FindAnyObjectByType<Enemy>();
        }

        if (enemyScript != null && uiManagerScript != null && sharedPoint != null)
        {
            if ((enemyScript.transform.position - sharedPoint.transform.position).magnitude < 1f)
            {
                uiManagerScript.WinGame();

                if (sniperScript != null) sniperScript.enabled = false;
                if (flameShooterScript != null) flameShooterScript.enabled = false;
                if (shotgunScript != null) shotgunScript.enabled = false;
                if (enemyScript != null) enemyScript.enabled = false;
                if (uiManagerScript != null) uiManagerScript.enabled = false;

                Time.timeScale = 0f;

                enabled = false;
            }
        }
    }
}