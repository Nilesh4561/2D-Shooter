using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveUI : MonoBehaviour {

    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountDownText;

    [SerializeField]
    Text waveNoText;

    WaveSpawner.SpawnState previousState;



    // Use this for initialization
    void Start () {
        if (spawner == null)
        {
            Debug.Log("no object referenece of wavespawner");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.Log("no object referenece of animator");
            this.enabled = false;
        }
        if (waveCountDownText == null)
        {
            Debug.Log("no object referenece of text for wave count down");
            this.enabled = false;
        }
        if (waveNoText == null)
        {
            Debug.Log("no object referenece of text for wave info");
            this.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        switch (spawner.State)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;

            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
        }
        previousState = spawner.State;
    }

    void UpdateCountingUI()
    {
        if (previousState != WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountDown", true);
            // Debug.Log("COUNTING");
        }
        waveCountDownText.text = ((int)spawner.WaveCountdown).ToString();
    }

    void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveCountDown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            waveNoText.text = (spawner.NextWave).ToString();

            // Debug.Log("SPAWNING");
        }
    }
}
