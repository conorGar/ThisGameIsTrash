using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitioner : MonoBehaviour {
    public AudioClip TopMusic;
    public AudioClip BottomMusic;
    private Coroutine MusicRoutine;

	void OnTriggerExit2D(Collider2D collider){
        Debug.Log("Player enters audio switcher");

        // Stop any old transitions if they player is running back and forth over the trigger and trying to break stuff.
        if (MusicRoutine != null) {
            StopCoroutine(MusicRoutine);
            MusicRoutine = null;
        }

        if (collider.gameObject.tag == "Player") {
            if (collider.gameObject.transform.position.y > transform.position.y) {
                MusicRoutine = SoundManager.instance.TransitionMusic(TopMusic);
            }
            else {
                MusicRoutine = SoundManager.instance.TransitionMusic(BottomMusic);
            }
        }
    }
}
