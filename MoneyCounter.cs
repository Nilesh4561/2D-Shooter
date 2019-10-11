using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour {

    private Text moneyCounter;

    // Use this for initialization
    void Start()
    {
        moneyCounter = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyCounter.text = "Money : " + GameMaster.Money.ToString();
    }
}
