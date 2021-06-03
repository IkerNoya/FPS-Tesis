using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    HPController hpController;
    void Start()
    {
        hpController = GetComponent<HPController>();
    }

    void Update()
    {
        
    }
}
