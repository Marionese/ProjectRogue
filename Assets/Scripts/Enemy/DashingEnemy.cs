using System.Collections;
using UnityEngine;

public class DashingEnemy : EnemyBase
{
    // Intentionally empty.
    // All behavior comes from EnemyBase.
    private float dashSpeed = 12f;
    private float dashDuration = 0.15f;
    private float dashLag = 0.25f;
    protected override IEnumerator Attack()
    {
        knockbackTimer = 0f;
        isPaused = true;
        yield return new WaitForSeconds(0.2f);

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
