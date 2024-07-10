using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isSlowed;
    public bool isAttackedByMelee;
    private Transform targetPosition;
    private Vector3 direction;
    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    public float moveSpeed = 5f;
    public float attackRange = 0.5f;

    public Animator animator;

    void Start()
    {
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("WayEnemy");
        waypoints = new List<Transform>();
        foreach (GameObject obj in waypointObjects)
        {
            waypoints.Add(obj.transform);
        }
        waypoints.Sort((a, b) => a.name.CompareTo(b.name));

        if (waypoints.Count > 0)
        {
            targetPosition = waypoints[currentWaypointIndex];
            direction = (targetPosition.position - transform.position).normalized;
        }
        else
        {
            targetPosition = GameObject.FindWithTag("Castle").transform;
        }
    }

    void Update()
    {
        if (targetPosition != null && !isAttackedByMelee)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 previousPosition = transform.position; // Lưu vị trí trước khi di chuyển

        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        Vector3 directionToTarget = targetPosition.position - transform.position;
        if (directionToTarget.magnitude <= moveSpeed * Time.deltaTime)
        {
            ReachTarget();
        }

        // Tính toán hướng di chuyển
        Vector3 movementDirection = (transform.position - previousPosition).normalized;
        SetMovementAnimation(movementDirection); // Gọi hàm để đặt animation dựa trên hướng di chuyển
    }

    void SetMovementAnimation(Vector3 movementDirection)
    {
        if (animator == null) return; // Kiểm tra xem animator có tồn tại không

        if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
        {
            if (movementDirection.x > 0)
            {
                animator.SetTrigger("MoveRight");
            }
            else
            {
                animator.SetTrigger("MoveLeft");
            }
        }
        else
        {
            if (movementDirection.y > 0)
            {
                animator.SetTrigger("MoveUp");
            }
            else
            {
                animator.SetTrigger("MoveDown");
            }
        }
    }

    void ReachTarget()
    {
        if (currentWaypointIndex < waypoints.Count - 1)
        {
            currentWaypointIndex++;
            targetPosition = waypoints[currentWaypointIndex];
            direction = (targetPosition.position - transform.position).normalized;
        }
        else
        {
            targetPosition = GameObject.FindWithTag("Castle").transform;
            direction = (targetPosition.position - transform.position).normalized;
        }
    }

    public void SlowDown(float slowAmount)
    {
        if (!isSlowed)
        {
            moveSpeed -= slowAmount;
            isSlowed = true;
            Debug.Log("Enemy Slowed: " + moveSpeed);
        }
    }

    public void ResetSpeed(float originalSpeed)
    {
        if (isSlowed)
        {
            moveSpeed = originalSpeed;
            isSlowed = false;
            Debug.Log("Enemy Speed Restored: " + moveSpeed);
        }
    }


    public void stopWalking()
    {
        isAttackedByMelee = true;
        // Add other actions when stopping walking
    }

    public void setIsAttacked(bool value)
    {
        isAttackedByMelee = value;
    }
}
