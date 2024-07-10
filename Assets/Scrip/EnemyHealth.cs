using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int coin;
    public Animator ani;

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
    }

    private void Die()
    {
        // Thực hiện các hành động khi enemy chết như phát hiện animation, thêm tiền, v.v.
        CoinManager.instance.AddCoins(coin);
        Destroy(gameObject, 0.3f);
        ani.SetTrigger("Die");
    }
}
