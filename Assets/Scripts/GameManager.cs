using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player playerScript;
    public UIManager uiManagerScript;
    public Sniper sniperScript;
    public FlameShooter flameShooterScript;
    public Shotgun shotgunScript;
    public TurretRotation turretRotationScript;
    public GameObject goal;

    void Update()
    {
        WinCondition();
    }

    public void WinCondition()
    {
        if (playerScript != null && uiManagerScript != null && goal != null)
        {
            if (Vector3.Distance(playerScript.transform.position, goal.transform.position) < 1f)
            {
                uiManagerScript.WinGame();

                if (sniperScript != null) sniperScript.enabled = false;
                if (flameShooterScript != null) flameShooterScript.enabled = false;
                if (shotgunScript != null) shotgunScript.enabled = false;
                if (turretRotationScript != null) turretRotationScript.enabled = false;
                
                playerScript.enabled = false;
                enabled = false;
            }
        }
    }
}