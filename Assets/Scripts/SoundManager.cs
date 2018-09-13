using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource sfxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;

	public float lowPitchRange = .95f;
	public float highPitchRange = 1.1f;


	bool musicFading;
	// Use this for initialization
	void Awake () {
		if(instance == null){
			instance = this;
		}else if (instance != this){
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	public void PlaySingle(AudioClip clip){
		//sfxSource.clip = clip;
		//sfxSource.Play();
		sfxSource.pitch = 1;
		sfxSource.PlayOneShot(clip);
	}

	public void RandomizeSfx(params AudioClip[] clips){
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange,highPitchRange);

		sfxSource.pitch = randomPitch;
		//sfxSource.clip = clips[randomIndex];
		//sfxSource.Play();
		sfxSource.PlayOneShot(clips[randomIndex]);
	}
	public void RandomizeSfx(AudioClip clip, float lowPitch, float highPitch){
		//int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitch,highPitch);

		sfxSource.pitch = randomPitch;
		//sfxSource.clip = clips[randomIndex];
		//sfxSource.Play();
		sfxSource.PlayOneShot(clip);
	}
	void Update(){
		if(musicFading){
			if(musicSource.volume > 0f){
				musicSource.volume -= .4f*Time.deltaTime;
			}else{
				musicFading = false;
			}
		}
	}
	public void FadeMusic(){
		musicFading = true;
	}

}
