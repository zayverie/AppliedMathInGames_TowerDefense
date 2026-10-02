using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    public Transform enemy;
    public Sniper sniperScript;

    [SerializeField] private float speed = 50f;
    [SerializeField] private float hitArea = 1.5f;
    [SerializeField] private float bulletLifetime = 5f;

    void Start()
    {
        if (sniperScript == null)
        {
            sniperScript = FindAnyObjectByType<Sniper>();
        }

        if (enemy == null)
        {
            Enemy foundEnemy = FindAnyObjectByType<Enemy>();
            if (foundEnemy != null)
            {
                enemy = foundEnemy.transform;
            }
        }

        Destroy(gameObject, bulletLifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        SniperHitCheck();
    }

    private void SniperHitCheck()
    {
        if (sniperScript != null && !sniperScript.InSight(transform.position))
        {
            Destroy(gameObject);
            return;
        }

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy e in enemies)
        {
            if (e != null && (transform.position - e.transform.position).sqrMagnitude <= hitArea)
            {
                Debug.Log("enemy hit by sniper projectile!");
                e.EnemyDies();
                Destroy(e.gameObject);
                Destroy(gameObject);

                // if (sceneRestarterScript != null)
                // {
                //     sceneRestarterScript.RestartScene();
                // }
            }
        }
    }
}