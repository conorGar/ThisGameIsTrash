using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_JumboFilmSFXHandler : MonoBehaviour {

	List<AudioClip> pawShankAudio = new List<AudioClip>();
	List<AudioClip> pupFictionAudio = new List<AudioClip>();
	List<AudioClip> citizenDaneAudio = new List<AudioClip>();


	List<AudioClip> currentAudioList = new List<AudioClip>();

	tk2dSpriteAnimator myAnim;
	int aniFrame = 1; //workaround for not being able to figure out how tk2d triggers work... starts at 1 so no play during title screen
	// Use this for initialization
	void Start () {
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(myAnim.CurrentFrame > aniFrame){
			SoundManager.instance.PlaySingle(currentAudioList[myAnim.CurrentFrame +1]); //+1 because of title screen
			aniFrame = myAnim.CurrentFrame;
		}
	}
}
