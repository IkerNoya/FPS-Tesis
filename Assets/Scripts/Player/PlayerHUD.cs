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
        healthPoints.text = "HP: " + hpController.GetHP().ToString();
    }
}
