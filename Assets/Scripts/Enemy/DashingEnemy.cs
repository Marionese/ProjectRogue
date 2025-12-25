using System.Collections;
using UnityEngine;

public class DashingEnemy : EnemyBase
{
    // Intentionally empty.
    // All behavior comes from EnemyBase.
    private float dashSpeed = 10f;
    private float dashDuration = 0.5f;
    private float dashLag = 0.3f;
    protected override IEnumerator Attack()
    {
        knockbackTimer = 0f;
        isPaused = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);

        Vector2 dir =
            ((Vector2)currentTargetPlayer.transform.position
             - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(dashLag);
        isPaused = false;

        attackCoroutine = null;
    }
}
