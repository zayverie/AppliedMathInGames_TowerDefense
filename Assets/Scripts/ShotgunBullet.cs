using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public Transform enemy;
    public Shotgun shotgunScript;
    public SceneRestarter sceneRestarterScript;

    [SerializeField] private float speed = 18f;
    [SerializeField] private float hitArea = 1.5f;
    [SerializeField] private float bulletLifetime = 3f;

    void Start()
    {
        if (enemy == null)
        {
            Enemy foundEnemy = FindAnyObjectByType<Enemy>();
            if (foundEnemy != null) enemy = foundEnemy.transform;
        }

        if (sceneRestarterScript == null)
        {
            sceneRestarterScript = FindAnyObjectByType<SceneRestarter>();
        }
        
        if (shotgunScript == null)
        {
            shotgunScript = FindAnyObjectByType<Shotgun>();
        }

        Destroy(gameObject, bulletLifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        PelletHitCheck();
    }

    private void PelletHitCheck()
    {
        if (shotgunScript != null && !shotgunScript.IsInCone(transform.position))
        {
            Destroy(gameObject);
            return;
        }

        if (enemy != null && Vector3.Distance(transform.position, enemy.position) <= hitArea)
        {
            Debug.Log("enemy hit by shotgun pellet!");
            Destroy(gameObject);

            if (sceneRestarterScript != null)
            {
                sceneRestarterScript.RestartScene();
            }
        }
    }
}