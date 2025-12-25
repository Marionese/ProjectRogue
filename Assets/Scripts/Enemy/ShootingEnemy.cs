using System.Collections;
using UnityEditor.Experimental;
using UnityEngine;

public class ShootingEnemy : EnemyBase
{
    private float dashLag = 0.3f;
    [SerializeField] private Color bulletColor;
    [SerializeField] private Sprite bulletSprite;
    protected override IEnumerator Attack()
    {
        knockbackTimer = 0f;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);

        Vector2 dir =
            ((Vector2)currentTargetPlayer.transform.position
             - (Vector2)transform.position).normalized;

        AttackData atk = new AttackData();
        atk.baseDamage = 1f;
        atk.speed = 4f;
        atk.multiplier = 1f;
        atk.isBullet = true;
        atk.size = 1f;
        atk.range = 1f;
        atk.bulletSprite = bulletSprite;
        atk.bulletColor = bulletColor;

        Quaternion rot = Quaternion.FromToRotation(Vector3.right, dir);
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = rot;
        bullet.GetComponent<BulletScript>().Initialize(atk, null);

        yield return new WaitForSeconds(dashLag);
        attackCoroutine = null;
    }
}
