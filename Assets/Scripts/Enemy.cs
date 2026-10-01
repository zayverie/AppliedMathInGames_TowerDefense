using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum CurveType
    {
        Quadratic,
        Cubic
    }

    public CurveType curveType;
    public Vector3 initialPosition, p1, p2, p3; // Control points for the curve
    public float velocity = 5f;
    public EnemySpawner enemySpawner;
    public ShotgunBullet shotgunBullet;
    public SniperBullet sniperBullet;
    public Flame flame;
    public float duration = 10f; // Duration to traverse the curve
    public float elapsedTime = 0f; // Time elapsed since the start of the movement
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(enemySpawner == null)
        {
            enemySpawner = FindAnyObjectByType<EnemySpawner>();
        }

        if(shotgunBullet == null)
        {
            shotgunBullet = FindAnyObjectByType<ShotgunBullet>();
        }

        if(sniperBullet == null)
        {
            sniperBullet = FindAnyObjectByType<SniperBullet>();
        }

        if(flame == null)
        {
            flame = FindAnyObjectByType<Flame>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration); // Normalized time (0 to 1)

        transform.position = curveType == CurveType.Quadratic
            ? Ease.QuadraticBezier(initialPosition, p1 * 1.8f, p2, t)
            : Ease.CubicBezier(initialPosition, p1 * 3.6f, p2 * 1.8f, p3, t);
    
        if((transform.position - enemySpawner.sharedPoint.position).magnitude < 0.1f)
        {
            Destroy(gameObject);
            Debug.Log("Enemy reached the goal!");
        }
        
    }

    public void OnDrawGizmos()
    {
        var previousLine = initialPosition;
        for(int i = 0; i <= 10; i++)
        {
            var t = i / 10f;
            var currentLine = curveType == CurveType.Quadratic
                ? Ease.QuadraticBezier(initialPosition, p1 * 1.8f, p2, t)
                : Ease.CubicBezier(initialPosition, p1 * 3.6f, p2 * 1.8f, p3, t);
            Gizmos.DrawLine(previousLine, currentLine);
            previousLine = currentLine;
        }
    }
}