using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Simple script to find Jim on any scene and store a reference so any script can find him.
public class PlayerManager : MonoBehaviour {
    public static PlayerManager Instance;
    public GameObject player;
    public JimStateController controller;

    // Use this for initialization
    void Awake () {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    // Find Jim at the start of any scene and store him!
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<JimStateController>();
    }
}
