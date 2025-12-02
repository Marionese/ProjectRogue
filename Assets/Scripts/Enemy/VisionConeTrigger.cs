using UnityEngine;

public class VisionConeTrigger : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyScript enemy = GetComponentInParent<EnemyScript>();
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            enemy.SetTarget(player);
            enemy.SwitchState(EnemyScript.EnemyState.aggressive);
        }
    }
}
