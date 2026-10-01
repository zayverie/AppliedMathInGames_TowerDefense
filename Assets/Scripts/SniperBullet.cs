using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    public Transform player;
    public Sniper sniperScript;
    public SceneRestarter sceneRestarterScript;

    [SerializeField] private float speed = 50f;
    [SerializeField] private float hitArea = 1.5f;
    [SerializeField] private float bulletLifetime = 5f;

    void Start()
    {
        if (sniperScript == null)
        {
            sniperScript = FindAnyObjectByType<Sniper>();
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

        Destroy(gameObject, bulletLifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));
        SniperHitCheck();
    }

    private void SniperHitCheck()
    {
        if (sniperScript != null && !sniperScript.InSight(transform.position))
        {
            Destroy(gameObject);
            return;
        }

        if (player != null && (transform.position - player.position).sqrMagnitude <= hitArea)
        {
            Debug.Log("Player hit by sniper projectile!");
            Destroy(gameObject);

            if (sceneRestarterScript != null)
            {
                sceneRestarterScript.RestartScene();
            }
        }
    }
}