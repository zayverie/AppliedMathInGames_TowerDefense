using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public Transform player;
    public Shotgun shotgunScript;
    public SceneRestarter sceneRestarterScript;

    [SerializeField] private float speed = 18f;
    [SerializeField] private float hitArea = 1.5f;
    [SerializeField] private float bulletLifetime = 3f;

    void Start()
    {
        if (player == null)
        {
            Player foundPlayer = FindAnyObjectByType<Player>();
            if (foundPlayer != null) player = foundPlayer.transform;
        }

        if (sceneRestarterScript == null)
        {
            sceneRestarterScript = FindAnyObjectByType<SceneRestarter>();
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

        if (player != null && Vector3.Distance(transform.position, player.position) <= hitArea)
        {
            Debug.Log("Player hit by shotgun pellet!");
            Destroy(gameObject);

            if (sceneRestarterScript != null)
            {
                sceneRestarterScript.RestartScene();
            }
        }
    }
}