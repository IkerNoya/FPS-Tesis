using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy {
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
        base.Attack();
    }

    public override void Hit(float damage) {
        base.Hit(damage);
    }

    public override void HitWithStun(float damage, float stunDuration) {
        base.HitWithStun(damage, stunDuration);
    }
}
