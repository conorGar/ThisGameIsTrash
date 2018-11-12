using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitioner : MonoBehaviour {
    public AudioClip TopMusic;
    public AudioClip BottomMusic;

    bool isTransitioning = false;

	void OnTriggerEnter2D(Collider2D collider){
        if (!isTransitioning) {
            TransitionCheck(collider);
        }
	}

    void TransitionCheck(Collider2D collider)
    {
        Debug.Log("Player enters audio switcher");
        if (collider.gameObject.tag == "Player") {
            isTransitioning = true;

            if (collider.gameObject.transform.position.y < transform.position.y) {
                SoundManager.instance.TransitionMusic(TopMusic, true, false, () => {
                    isTransitioning = false;
                });

            }
            else {
                SoundManager.instance.TransitionMusic(BottomMusic, true, false, () => {
                    isTransitioning = false;
                });
            }
        }
    }
}
