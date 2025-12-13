using UnityEngine;

public class VisionConeTrigger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBase enemy = GetComponentInParent<EnemyBase>();
        if (other.CompareTag("PlayerHitBox"))
        {

            var player = other.GetComponentInParent<PlayerController>();
            enemy.Alert(player);

        }

    }
}
