using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Sniper : MonoBehaviour
{
    public float range = 20f;
    public Transform player;
    public GameObject bulletPrefab;
    public Player playerScript;
    public float fireRate = 1f;
    private float nextFireTime;
    private LineRenderer lr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }
    void Start()
    {
        playerScript = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawSniperLine();
        if (player != null && playerScript != null && InSight(player.position))
        {
            if (Time.time >= nextFireTime)
            {
                FireSniperBullet();
                nextFireTime = Time.time + fireRate;
            }
        }
    }
    public bool InSight(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) > range)
        {
            return false;
        }

        Vector3 toPlayer = (player.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward.normalized, toPlayer);
        return dot >= 0.98f;  // ~11° tolerance
    }

    public void FireSniperBullet()
    {
        Vector3 firePoint = transform.position + transform.forward * 1f;
        
        Quaternion bulletRotation = transform.rotation * Quaternion.Euler(90f, 0f, 0f);
        GameObject sniperBullet = Instantiate(bulletPrefab, firePoint, bulletRotation);
        
        SniperBullet bulletScript = sniperBullet.GetComponent<SniperBullet>();
        if (bulletScript != null)
        {
            bulletScript.sniperScript = this;
        }
    }
    public void DrawSniperLine()
    {
        Vector3 origin = transform.position;
        Vector3 endPoint = origin + transform.forward * range;

        lr.SetPosition(0, origin);
        lr.SetPosition(1, endPoint);
    }
    
}
