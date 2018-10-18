using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

// Super simple Global manager for the cameras and camera related things in the scene.
public class CamManager : MonoBehaviour {
    public static CamManager Instance;

    public Ev_MainCamera mainCam;
    public Ev_MainCameraEffects mainCamEffects;
    public tk2dCamera tk2dMainCam;
    public PostProcessingBehaviour mainCamPostProcessor;
    public Camera guiCam;

    // Use this for initialization
	void Awake () {
        Instance = this;
	}
}
