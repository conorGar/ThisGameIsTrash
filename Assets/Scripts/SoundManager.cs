using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource sfxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;
	public AudioClip worldMusic;
	public float lowPitchRange = .95f;
	public float highPitchRange = 1.1f;
    public float fadeSpeed = .4f;



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
				musicSource.volume -= fadeSpeed*Time.deltaTime;
			}else{
				musicFading = false;
			}
		}
	}

    private void OnLevelWasLoaded(int level)
    {
        // Make sure fading ends before the next scene starts loading stuff.
        musicFading = false;
    }

    public void FadeMusic(){
		musicFading = true;
	}

    public Coroutine TransitionMusic(AudioClip nextClip, bool fadeOut = true, bool fadeIn = false, System.Action callback = null)
    {
        return StartCoroutine(TransitionMusicEnumerator(nextClip, fadeOut, fadeIn, callback));
    }

    IEnumerator TransitionMusicEnumerator(AudioClip nextClip, bool fadeOut = true, bool fadeIn = false, System.Action callback = null)
    {
        // fade out the last clip.
        if (fadeOut) {
            while (musicSource.volume > 0f) {
                musicSource.volume = Mathf.Max(musicSource.volume - fadeSpeed * Time.deltaTime, 0f);
                yield return null;
            }
        } else {
            musicSource.volume = 0f;
        }

        // Switch to the new clip.
        musicSource.Stop();
        musicSource.clip = nextClip;
        musicSource.Play();

        // fade in the next clip.
        if (fadeIn) {
            while (musicSource.volume < GlobalVariableManager.Instance.MASTER_MUSIC_VOL) {
                musicSource.volume = Mathf.Min(musicSource.volume + fadeSpeed * Time.deltaTime, GlobalVariableManager.Instance.MASTER_MUSIC_VOL);
                yield return null;
            }
        } else {
            musicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
        }

        if (callback != null)
            callback();
    }
}
