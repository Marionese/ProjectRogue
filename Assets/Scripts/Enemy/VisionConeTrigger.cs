using UnityEngine;

public class VisionConeTrigger : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = GetComponentInParent<EnemyBase>();
        if (other.CompareTag("Player"))
        {

            var player = other.GetComponentInParent<PlayerController>();
            enemy.Alert(player);

        }

    }
}
