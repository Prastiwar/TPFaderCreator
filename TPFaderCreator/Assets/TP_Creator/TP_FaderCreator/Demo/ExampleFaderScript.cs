using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TP_Fader;

public class ExampleFaderScript : MonoBehaviour
{
    public Button button;
    public string otherScene;
    public float speed;

	// Use this for initialization
	void Awake ()
    {
        //button.onClick.AddListener(() => TPFaderCreator.FadeTo(otherScene, Color.black, speed));
	}

}
