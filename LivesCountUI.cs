using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesCountUI : MonoBehaviour {

    private Text livesText;

    // Use this for initialization
    void Start () {
        livesText = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        livesText.text = "LIVES: " + GameMaster.PlayerLife.ToString();
    }
}
