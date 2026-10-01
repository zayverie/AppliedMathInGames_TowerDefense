using UnityEngine;
using UnityEngine.SceneManagement;

public class Flame : MonoBehaviour
{
    public Transform enemy;
    public FlameShooter flameShooterScript;
    public SceneRestarter sceneRestarterScript;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float hitArea = 1.5f;
    [SerializeField] private float flameLifetime = 5f;

    void Start()
    {
        if (flameShooterScript == null)
        {
            flameShooterScript = FindAnyObjectByType<FlameShooter>();
        }

        if (enemy == null)
        {
            Enemy foundEnemy = FindAnyObjectByType<Enemy>();
            if (foundEnemy != null)
            {
                enemy = foundEnemy.transform;
            }
        }
        if (sceneRestarterScript == null)
        {
            sceneRestarterScript = FindAnyObjectByType<SceneRestarter>();
        }

        Destroy(gameObject, flameLifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        HitOrOutsideCone();
    }

    private void HitOrOutsideCone()
    {
        if (flameShooterScript != null && !flameShooterScript.IsInCone(transform.position))
        {
            Destroy(gameObject);
            return;
        }

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy e in enemies)
        {
            if (e != null && (transform.position - e.transform.position).sqrMagnitude <= hitArea)
            {
                Debug.Log("enemy hit by flame!");
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