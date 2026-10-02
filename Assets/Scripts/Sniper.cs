using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Sniper : MonoBehaviour
{
    public float range = 20f;
    public Transform enemy;
    public GameObject bulletPrefab;
    public float fireRate = 1f;

    private float nextFireTime;
    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    void Update()
    {
        DrawSniperLine();

        enemy = GetEnemyInSight();

        if (enemy != null)
        {
            Vector3 dir = enemy.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            if (Time.time >= nextFireTime)
            {
                FireSniperBullet();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private Transform GetEnemyInSight()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy e in enemies)
        {
            if (e != null && InSight(e.transform.position))
            {
                return e.transform;
            }
        }
        return null;
    }

    public bool InSight(Vector3 position)
    {
        Vector3 toTarget = position - transform.position;
        toTarget.y = 0f;

        if (toTarget.magnitude > range)
            return false;

        float dot = Vector3.Dot(transform.forward, toTarget.normalized);
        return dot >= 0.98f;
    }

    public void FireSniperBullet()
    {
        Vector3 firePoint = transform.position + transform.forward * 1f;
        GameObject sniperBullet = Instantiate(bulletPrefab, firePoint, transform.rotation);

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