using UnityEngine;

public class VisionConeTrigger : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyScript enemy = GetComponentInParent<EnemyScript>();
        if (other.CompareTag("PlayerHitBox"))
        {
            var player = other.GetComponentInParent<PlayerController>();
            enemy.SetTarget(player);
            enemy.SwitchState(EnemyScript.EnemyState.aggressive);
        }
    }
}
