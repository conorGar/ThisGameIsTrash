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

    // Makes a gameobject face the player on the x-axis.
    // TODO: add any logic to face the dummy if it's in play.
    public void Face(GameObject go)
    {
        go.transform.localScale = new Vector3(Mathf.Sign(player.transform.position.x - go.transform.position.x),
                                                      go.transform.localScale.y,
                                                      go.transform.localScale.z);
    }

    // Returns true if the player is to the left of the gameObject.
    // TODO: add any dummy logic.
    public bool IsLeft(GameObject go)
    {
        return player.transform.position.x < go.transform.position.x;
    }
}
