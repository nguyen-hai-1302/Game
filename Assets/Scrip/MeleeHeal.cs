using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHeal : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
            return; // Bỏ qua nếu đã chết

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            //anim.SetTrigger("Hurt");
        }
    }

    private void Die()
    {
        //anim.SetTrigger("Die");
        Destroy(gameObject, 0.3f);
    }
}
