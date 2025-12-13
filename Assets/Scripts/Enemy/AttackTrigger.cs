using UnityEngine;

public class AttackTrigger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBase enemy = GetComponentInParent<EnemyBase>();
        if (other.CompareTag("PlayerHitBox"))
        {

            var player = other.GetComponentInParent<PlayerController>();
            if (player != null)
                enemy.PlayerGotInRange(player);

        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        EnemyBase enemy = GetComponentInParent<EnemyBase>();
        if (other.CompareTag("PlayerHitBox"))
        {
            var player = other.GetComponentInParent<PlayerController>();
            if (player != null)
                enemy.LeftPlayerRange(player);

        }

    }
}
