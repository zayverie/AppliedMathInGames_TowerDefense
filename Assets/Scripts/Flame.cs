using UnityEngine;
using UnityEngine.SceneManagement;

public class Flame : MonoBehaviour
{
    public Transform player;
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

        if (player == null)
        {
            Player foundPlayer = FindAnyObjectByType<Player>();
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
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

        if (player != null && (transform.position - player.position).sqrMagnitude <= hitArea)
        {
            Debug.Log("Player hit by projectile!");
            Destroy(gameObject);
            sceneRestarterScript.RestartScene();
        }
    }
}