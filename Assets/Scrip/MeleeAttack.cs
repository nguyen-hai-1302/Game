using UnityEngine;

public class Melee : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float lineOfSight; // Phạm vi nhìn thấy enemy

    [Header("Attack")]
    private Transform enemy;
    private Vector3 basePoint; // Điểm cơ sở để quay về
    public float attackRange;
    public float speedAttack;
    private float nextAttackTime; // Thời gian tiếp theo có thể tấn công
    private bool isAttacking = false;

    private void Start()
    {
        basePoint = transform.position; // Gán vị trí hiện tại của người chơi
    }

    void Update()
    {
        FindClosestEnemy();

        if (enemy != null)
        {
            float distanceFromEnemy = Vector2.Distance(enemy.position, transform.position);

            // Nếu khoảng cách trong phạm vi nhìn thấy nhưng ngoài phạm vi tấn công
            if (distanceFromEnemy < lineOfSight && distanceFromEnemy > attackRange)
            {
                isAttacking = false;
                // Di chuyển về phía người chơi
                transform.position = Vector2.MoveTowards(transform.position, enemy.position, speed * Time.deltaTime);

                // Điều chỉnh hướng của kẻ địch về phía người chơi
                if (transform.position.x < enemy.position.x)
                {
                    transform.localScale = new Vector2(-1, 1);
                }
                else
                {
                    transform.localScale = new Vector2(1, 1);
                }
            }
            // Nếu khoảng cách trong phạm vi tấn công và có thể tấn công
            else if (distanceFromEnemy <= attackRange && nextAttackTime < Time.time)
            {
                isAttacking = true;
                nextAttackTime = Time.time + speedAttack; // Cập nhật thời gian tiếp theo có thể tấn công
                Attack();
            }
        }
        else
        {
            // Di chuyển về điểm cơ sở nếu không có kẻ địch
            isAttacking = false;
            transform.position = Vector2.MoveTowards(transform.position, basePoint, speed * Time.deltaTime);

            // Điều chỉnh hướng của kẻ địch về phía điểm cơ sở
            if (transform.position.x < basePoint.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }
        }
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject go in enemies)
        {
            float distance = Vector2.Distance(transform.position, go.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = go.transform;
            }
        }

        enemy = closestEnemy;
    }

    void Attack()
    {
        // Thêm logic tấn công ở đây (ví dụ: giảm máu của kẻ địch)
        Debug.Log("Tấn công kẻ địch!");
    }

    // Vẽ phạm vi nhìn thấy và phạm vi tấn công trong chế độ Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
