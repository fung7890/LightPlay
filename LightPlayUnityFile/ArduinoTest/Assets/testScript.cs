using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GradientHandler : MonoBehaviour {

    public Camera camera;
    public Gradient gradient;
    // Use this for initialization
    void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera; //Gets Camera script from MainCamera object(Object's tag is MainCamera).
        GradientColorKey[] colorKey = new GradientColorKey[2];
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = 1.0f;
        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;
        gradient.SetKeys(colorKey, alphaKey);
    }

    // Update is called once per frame
    void Update () {
        Debug.Log ("Time: "+Time.deltaTime);
		Debug.Log(gradient.Evaluate(0.25f));

    }
}