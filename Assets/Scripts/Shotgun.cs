using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Shotgun : MonoBehaviour
{
    [SerializeField] private float range = 8f;
    [SerializeField] private float coneAngle = 60f;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private int pelletCount = 5;

    public GameObject bulletPrefab;
    public Transform enemy;

    private float nextFireTime;
    private LineRenderer lr;
    private float halfConeAngle;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 3;
        halfConeAngle = coneAngle / 2f;
    }

    void Update()
    {
        enemy = GetEnemyInCone();

        if (enemy != null)
        {
            Vector3 dir = enemy.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            if (Time.time >= nextFireTime)
            {
                FireShotgun();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void LateUpdate()
    {
        DrawCone();
    }

    private Transform GetEnemyInCone()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy e in enemies)
        {
            if (e != null && IsInCone(e.transform.position))
            {
                return e.transform;
            }
        }
        return null;
    }

    public bool IsInCone(Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - transform.position;
        dir.y = 0f;

        if (dir.magnitude > range)
            return false;

        float pAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float tAngle = transform.eulerAngles.y;

        float delta = Mathf.Abs(Mathf.DeltaAngle(tAngle, pAngle));

        return delta <= halfConeAngle;
    }

    public void DrawCone()
    {
        Vector3 origin = transform.position;
        Vector3 leftDir = Quaternion.Euler(0, -halfConeAngle, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, halfConeAngle, 0) * transform.forward;

        lr.SetPosition(0, origin);
        lr.SetPosition(1, origin + leftDir * range);
        lr.SetPosition(2, origin + rightDir * range);
    }

    public void FireShotgun()
    {
        Vector3 firePoint = transform.position + transform.forward * 1f;
        float angleSequence = coneAngle / pelletCount;
        float startAngle = -halfConeAngle;

        for (int i = 0; i < pelletCount; i++)
        {
            Quaternion rot = transform.rotation * Quaternion.Euler(0, startAngle, 0);
            GameObject bullet = Instantiate(bulletPrefab, firePoint, rot);

            ShotgunBullet bulletScript = bullet.GetComponent<ShotgunBullet>();
            if (bulletScript != null)
            {
                bulletScript.shotgunScript = this;
            }

            startAngle += angleSequence;
        }
    }
}