using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXBANK
{
    NONE,
    HIT6,
    HIT7,
    RAT_SQUEAL,
    ENEMY_BOUNCE,
    HEAL,
    DUMPSTERDASH,
    SCRAPPYSNEAK,
    SPARKLE,
    ITEM_CATCH,
    CLANK,
    VOICE_TICK,
    NOTICE, 
    TRUCK_APPEAR,
    MENU_NAV1,
    MENU_NAV2,
}

public enum MUSICBANK
{
    NONE
}

public class SoundManager : MonoBehaviour {

	public AudioSource sfxSource;
	public AudioSource musicSource;
	public AudioSource backupMusicSource; // for use with looping sfx that play while main worl dmusic is paused
	public static SoundManager instance = null;
	public AudioClip worldMusic;
	public float lowPitchRange = .95f;
	public float highPitchRange = 1.1f;
    public float fadeSpeed = .4f;

    [System.Serializable]
    public struct SFXItem
    {
        public SFXBANK soundID;
        public AudioClip clip;
    }

    [System.Serializable]
    public struct MusicItem
    {
        public MUSICBANK soundID;
        public AudioClip clip;
    }

    public List<SFXItem> SFXList;
    public List<MusicItem> MusicList;

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

    public void PlaySingle(MUSICBANK id)
    {
        var clip = GetMusic(id);

        if (clip != null) {
            PlaySingle(clip);
        }
    }

    public void PlaySingle(SFXBANK id)
    {
        var clip = GetSFX(id);
		sfxSource.pitch = 1;

        if (clip != null) {
            PlaySingle(clip);
        }
    }

	public void PlaySingle(AudioClip clip){
		//sfxSource.clip = clip;
		//sfxSource.Play();
		sfxSource.pitch = 1;
		sfxSource.PlayOneShot(clip);
	}

	public void PlaySingle(AudioClip clip, float pitch){
		//sfxSource.clip = clip;
		//sfxSource.Play();
		sfxSource.pitch = pitch;
		sfxSource.PlayOneShot(clip);
	}

	public void PlaySingle(SFXBANK id, float pitch){
		//sfxSource.clip = clip;
		//sfxSource.Play();
		var clip = GetSFX(id);
		sfxSource.pitch = 1;

        if (clip != null) {
			sfxSource.pitch = pitch;
			sfxSource.PlayOneShot(clip);
        }
	}

    public void RandomizeSfx(params SFXBANK[] ids)
    {
        int randomIndex = Random.Range(0, ids.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxSource.pitch = randomPitch;

        var clip = GetSFX(ids[randomIndex]);

        if (clip != null)
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

    public void RandomizeSfx(SFXBANK id, float lowPitch, float highPitch)
    {
        //int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitch, highPitch);

        sfxSource.pitch = randomPitch;

        var clip = GetSFX(id);

        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void RandomizeSfx(AudioClip clip, float lowPitch, float highPitch){
		//int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitch,highPitch);

		sfxSource.pitch = randomPitch;
		//sfxSource.clip = clips[randomIndex];
		//sfxSource.Play();
		sfxSource.PlayOneShot(clip);
	}

	public void SwitchToOtherBGM(AudioClip bgClip){
		musicSource.volume = 0;
		backupMusicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
		backupMusicSource.clip = bgClip;
		SoundManager.instance.backupMusicSource.Play();
	}

	public void ReturnToMainBGM(){
		SoundManager.instance.backupMusicSource.Stop();
		SoundManager.instance.musicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
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

    public Coroutine TransitionMusic(MUSICBANK id, bool fadeOut = true, bool fadeIn = false, System.Action callback = null)
    {
        var clip = GetMusic(id);

        if (clip != null)
            return TransitionMusic(id, fadeOut, fadeIn, callback);

        return null;
    }

    public Coroutine TransitionMusic(AudioClip nextClip, bool fadeOut = true, bool fadeIn = false, System.Action callback = null)
    {
        StopAllCoroutines();
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

    // Helpers

    // Are these too slow to loop through all the time?  Should it be a hash internally?
    public AudioClip GetSFX(SFXBANK id)
    {
        if (id == SFXBANK.NONE)
            return null;

        for (int i = 0; i < SFXList.Count; i++) {
            if (id == SFXList[i].soundID)
                return SFXList[i].clip;
        }

        Debug.LogError("SFX Clip not found for SFXBANK id: " + id + " Did you define it in the SoundManager?");
        return null;
    }

    AudioClip GetMusic(MUSICBANK id)
    {
        if (id == MUSICBANK.NONE)
            return null;

        for (int i = 0; i < MusicList.Count; i++) {
            if (id == MusicList[i].soundID)
                return MusicList[i].clip;
        }

        Debug.LogError("Music Clip not found for MUSICBANK id: " + id + " Did you define it in the SoundManager?");
        return null;
    }
}
