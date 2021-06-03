using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    HPController hpController;
    public Text healthPoints;

    void Start()
    {
        hpController = GetComponent<HPController>();
    }

    void Update()
    {
        if(healthPoints!=null)
            healthPoints.text = "HP: " + ((int)hpController.GetHP()).ToString();
    }
}
