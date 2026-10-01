using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform[] shooters;
    [SerializeField] private float rotSpeed = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotateToEnemy();
        
    }
    public void rotateToEnemy()
    {
        if (enemy == null || shooters == null) return;

        // Loop through each shooter in the array
        foreach (Transform shooter in shooters)
        {
            if (shooter == null) continue; // Skip empty slots

            // Direction from THIS shooter to the enemy
            Vector3 dir = enemy.position - shooter.position;

            // Angle on the XZ plane
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            // Rotate THIS specific shooter
            Quaternion targetRot = Quaternion.Euler(0f, angle, 0f);
            shooter.rotation = Quaternion.Slerp(
                shooter.rotation, 
                targetRot, 
                rotSpeed * Time.deltaTime

            /*var dot = Vector3.Dot(transform.forward.normalized, dir.normalized);
            Debug.Log(dot);*/
            );
        }
    }
}
