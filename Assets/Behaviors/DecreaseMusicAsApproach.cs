using UnityEngine;
using System.Collections;

public class DecreaseMusicAsApproach : MonoBehaviour
{

	public float distanceTillStartDecreasing;
	public float decreaseVolTo;
	public AudioSource musicAudioSource;

	float currentMusicVol;
	// Use this for initialization
	void Start ()
	{
		currentMusicVol = musicAudioSource.volume;
		musicAudioSource = SoundManager.instance.GetComponents<AudioSource>()[1];
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Vector2.Distance(gameObject.transform.position, PlayerManager.Instance.player.transform.position) < distanceTillStartDecreasing){
			float distance = Vector2.Distance(gameObject.transform.position, PlayerManager.Instance.player.transform.position);
			if(distance <1){//music stops when distance = 1 based on algorithm
				distance = 1;
			}
			Debug.Log("AudioSource decreasing...");
			musicAudioSource.volume = currentMusicVol - (currentMusicVol/distance);//TODO: this isnt really gonna work if player raises/lowers volume on options menu...
		}
	}
}

