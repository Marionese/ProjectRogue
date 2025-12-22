using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = GetComponentInParent<EnemyBase>();
        if (other.CompareTag("Player"))
        {

            var player = other.GetComponentInParent<PlayerController>();
            if (player != null)
                enemy.PlayerGotInRange(player);

        }

    }
    void OnTriggerExit(Collider other)
    {
        EnemyBase enemy = GetComponentInParent<EnemyBase>();
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponentInParent<PlayerController>();
            if (player != null)
                enemy.LeftPlayerRange(player);

        }

    }
}
