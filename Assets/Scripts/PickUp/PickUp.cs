﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
    public void PickUpUpgrade(Player p) {
        p.UnlockAllWeapons();
        Destroy(this.gameObject);
    }
}
