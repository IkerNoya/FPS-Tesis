using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable {

    void Hit(float damage);
    void HitWithStun(float damage, float stunDuration);

}
