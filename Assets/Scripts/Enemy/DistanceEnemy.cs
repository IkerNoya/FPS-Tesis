using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceEnemy : Enemy {

    [SerializeField] Projectile projectile;
    [SerializeField] Transform pewPewPosition;
    [SerializeField] float projectileForce;
    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }
    protected override void FixedUpdate() {
        base.FixedUpdate();
    }
    protected override void Attack() {
        timerAttack = 0;
        if (Physics.Raycast(transform.position, (objective.transform.position - transform.position), out hit, distanceToAttack))
            if (hit.transform.gameObject == objective) {
                Projectile p = Instantiate(projectile, pewPewPosition.position, Quaternion.identity);
                p.transform.LookAt(objective.transform.position);
                p.damage = damage;
                p.ignoreLayer = this.gameObject.layer;
                p.rb.AddForce(p.transform.forward * projectileForce);
            }
    }

    public override void Hit(float damage) {
        base.Hit(damage);
    }

    public override void HitWithStun(float damage, float stunDuration) {
        base.HitWithStun(damage, stunDuration);
    }
}
