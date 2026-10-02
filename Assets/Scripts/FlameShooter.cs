using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FlameShooter : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    [SerializeField] private float coneAngle = 45f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int flameCount = 5;

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

        enemyScript = GetEnemyInCone();

        if (enemyScript != null)
        {
            Vector3 dir = enemyScript.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

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