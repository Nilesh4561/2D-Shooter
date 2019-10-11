using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallexing : MonoBehaviour {

    public Transform[] backgrounds;
    public float smoothing = 1f;

    private float[] parallexScale;
    private Transform cam;
    private Vector3 prevCamPos;

    void Awake()
    {
        cam = Camera.main.transform;        
    }

    // Use this for initialization
    void Start () {
        prevCamPos = cam.position;

        parallexScale = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallexScale[i] = backgrounds[i].position.z * -1;
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallex = (prevCamPos.x - cam.position.x) * parallexScale[i];

            float backgroundTargetPosX = backgrounds[i].position.x + parallex;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        prevCamPos = cam.position;
    }
}
