using System.Collections;
using Unity.VisualScripting;
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
        isPaused = true;
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(0.3f);

        Vector3 dir =
            currentTargetPlayer.transform.position
             - transform.position;
        dir.y = 0;
        dir.Normalize();
        AttackData atk = new AttackData();
        atk.baseDamage = 1f;
        atk.speed = 4f;
        atk.multiplier = 1f;
        atk.isBullet = true;
        atk.size = 1f;
        atk.range = 1f;
        atk.bulletSprite = bulletSprite;
        atk.bulletColor = bulletColor;
        atk.forwardDirection = dir;
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, dir);
        GameObject bullet = BulletPool.Instance.GetBullet();
        var spawnpos = transform.position;
        spawnpos.y = 0.5f;
        bullet.transform.position = spawnpos;
        bullet.transform.rotation = rot;
        
        bullet.SetActive(true);
        bullet.GetComponent<BulletScript>().Initialize(atk, null);

        yield return new WaitForSeconds(dashLag);
        isPaused = false;

        attackCoroutine = null;
    }
}
