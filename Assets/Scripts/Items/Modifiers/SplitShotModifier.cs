using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Modifiers/SplitShot")]
public class SplitShotOnHit : AttackModifier
{
    public override void ApplyAttack(ref AttackData data) { }

    public override void OnHit(EnemyBase enemy, AttackData data)
{
    Vector3 f = data.forwardDirection.normalized;

    // Project to XZ plane (important!)
    f.y = 0f;
    f.Normalize();

    Vector3 left  = Quaternion.AngleAxis(-90f, Vector3.up) * f;
    Vector3 right = Quaternion.AngleAxis( 90f, Vector3.up) * f;

    float offset = 0.25f;

    Vector3 hit = data.hitPoint;
    Vector3 safeOrigin = hit - f * offset;

    // keep original Y
    safeOrigin.y = hit.y;

    data.multiplier *= 0.5f;
    data.range = 0.25f;
    data.knockback = 0;
    data.isBullet = false;

    SpawnSplitBullet(left,  safeOrigin, data);
    SpawnSplitBullet(right, safeOrigin, data);
}

    public override void OnHitEnvironment(AttackData data)
{
    Vector3 f = data.forwardDirection.normalized;

    f.y = 0f;
    f.Normalize();

    Vector3 left  = Quaternion.AngleAxis(-90f, Vector3.up) * f;
    Vector3 right = Quaternion.AngleAxis( 90f, Vector3.up) * f;

    float offset = 0.25f;

    Vector3 hit = data.hitPoint;
    Vector3 safeOrigin = hit - f * offset;
    safeOrigin.y = hit.y;

    data.multiplier *= 0.5f;
    data.range = 0.25f;
    data.knockback = 0;
    data.isBullet = false;

    SpawnSplitBullet(left,  safeOrigin, data);
    SpawnSplitBullet(right, safeOrigin, data);
}




    private void SpawnSplitBullet(Vector3 dir, Vector3 origin, AttackData data)
{
    GameObject bulletGO = BulletPool.Instance.GetBullet();

    bulletGO.transform.position = origin;
    bulletGO.transform.forward = dir;
    bulletGO.SetActive(true);
    BulletScript script = bulletGO.GetComponent<BulletScript>();
    script.Initialize(data, new List<AttackModifier>());
}

}
