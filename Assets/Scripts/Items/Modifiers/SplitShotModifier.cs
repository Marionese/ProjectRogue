using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Modifiers/SplitShot")]
public class SplitShotOnHit : AttackModifier
{
    public override void ApplyAttack(ref AttackData data) { }

    public override void OnHit(EnemyBase enemy, AttackData data)
    {
        Vector2 f = data.forwardDirection.normalized;

        Vector2 left = new Vector2(-f.y, f.x);
        Vector2 right = new Vector2(f.y, -f.x);

        float offset = 0.25f;

        // convert everything cleanly
        Vector3 hit3 = (Vector3)data.hitPoint;
        Vector3 f3 = new Vector3(f.x, f.y, 0f);

        Vector3 safeOrigin = hit3 - f3 * offset;
        data.multiplier *= 0.5f;
        data.range = 0.25f;
        data.knockback = 0;
        data.isBullet = false;
        SpawnSplitBullet(left, safeOrigin, data);
        SpawnSplitBullet(right, safeOrigin, data);
    }
    public override void OnHitEnvironment(AttackData data)
    {
        Vector2 f = data.forwardDirection.normalized;

        Vector2 left = new Vector2(-f.y, f.x);
        Vector2 right = new Vector2(f.y, -f.x);

        float offset = 0.25f;
        data.multiplier *= 0.5f;
        data.range = 0.25f;
        data.knockback = 0;
        data.isBullet = false;
        Vector3 hit3 = (Vector3)data.hitPoint;
        Vector3 f3 = new Vector3(f.x, f.y, 0);
        Vector3 safeOrigin = hit3 - f3 * offset;

        SpawnSplitBullet(left, safeOrigin, data);
        SpawnSplitBullet(right, safeOrigin, data);
    }



    private void SpawnSplitBullet(Vector2 dir, Vector3 origin, AttackData data)
    {
        GameObject bulletGO = BulletPool.Instance.GetBullet();

        // Spawn exactly at safeOrigin
        bulletGO.transform.position = origin;
        bulletGO.transform.right = dir;

        BulletScript script = bulletGO.GetComponent<BulletScript>();
        script.Initialize(data, new List<AttackModifier>());
    }


}
