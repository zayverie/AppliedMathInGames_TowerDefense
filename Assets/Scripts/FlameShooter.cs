using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FlameShooter : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    [SerializeField] private float coneAngle = 45f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int flameCount = 5;
    [SerializeField] private float turnSpeed = 6f;

    public GameObject flamePrefab;
    public Enemy enemyScript;

    private float nextFireTime;
    private LineRenderer lr;
    private float halfConeAngle;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 3;
        halfConeAngle = coneAngle * 0.5f;
    }

    void Update()
    {
        DrawCone();

        // 1. Assign the enemy found in the cone
        enemyScript = GetEnemyInCone();

        // 2. If an enemy is in the cone, rotate to follow and shoot
        if (enemyScript != null)
        {
            Vector3 dir = enemyScript.transform.position - transform.position;
            dir.y = 0f;

            if (dir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            }

            if (Time.time >= nextFireTime)
            {
                FireFlame();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private Enemy GetEnemyInCone()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && IsInCone(enemy.transform.position))
            {
                return enemy;
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

    public void FireFlame()
    {
        Vector3 firePoint = transform.position + transform.forward * 1f;

        float angleSequence = coneAngle / flameCount;
        float angle = -coneAngle / 2f;

        for (int i = 0; i < flameCount; i++)
        {
            Quaternion rot = transform.rotation * Quaternion.Euler(0, angle, 0);

            GameObject flame = Instantiate(flamePrefab, firePoint, rot);
            flame.GetComponent<Flame>().flameShooterScript = this;

            angle += angleSequence;
        }
    }
}