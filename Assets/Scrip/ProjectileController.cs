using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;
    public int damage = 6;
    public ArcherTower.TowerType towerType;
    private float slowAmount;
    private float slowTime;

    public void Seek(Transform target, int damage, ArcherTower.TowerType towerType, float slowAmount, float slowTime)
    {
        this.target = target;
        this.damage = damage;
        this.towerType = towerType;
        this.slowAmount = slowAmount;
        this.slowTime = slowTime;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // If the target is lost, destroy the projectile
            return;
        }

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        // Check if the projectile is close enough to the target
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        // Do damage to the target or whatever action you want when the projectile hits
        Destroy(gameObject);
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        if (towerType == ArcherTower.TowerType.Magic)
        {
            EnemyController enemyController = target.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                StartCoroutine(SlowEnemy(enemyController, slowAmount, slowTime)); 
            }
        }
    }

    IEnumerator SlowEnemy(EnemyController enemy, float slowAmount, float duration)
    {
        enemy.SlowDown(slowAmount);
        yield return new WaitForSeconds(duration);
        enemy.ResetSpeed(slowAmount);
    }
}
