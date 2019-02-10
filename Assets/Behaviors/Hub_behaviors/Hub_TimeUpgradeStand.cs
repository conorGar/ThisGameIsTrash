using UnityEngine;
using System.Collections;

public class Hub_TimeUpgradeStand : MonoBehaviour
{
	public GameObject spaceIcon;
	public GameObject player;
	public GameObject timeUpgradeHUD;
	public GameObject timeBack;
	public ParticleSystem starsPS;
	public GameObject catsParent;
	public AudioClip timeTentMusic;
	bool starSlowdown;
	float starSimSpeed = 1f;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            
                if (Mathf.Abs(transform.position.x - player.transform.position.x) < 4f && Mathf.Abs(transform.position.y - player.transform.position.y) < 7f) {

                    if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && timeUpgradeHUD.activeInHierarchy != true) {
                        StartCoroutine("SetUp");
                        spaceIcon.SetActive(false);
                    }
                    else if (!spaceIcon.activeInHierarchy) {
                        spaceIcon.SetActive(true);
                    }
                }
                else if (spaceIcon.activeInHierarchy) {
                    spaceIcon.SetActive(false);
                }
            
        }else{
        	if(starSlowdown && starSimSpeed > .02f){
				starSimSpeed -= .02f;
				Debug.Log("Star SLowdown");
        	}
			var main = starsPS.main;
        	main.simulationSpeed = starSimSpeed;
        }
	}

	IEnumerator SetUp(){
		GameStateManager.Instance.PushState(typeof(ShopState));
		CamManager.Instance.mainCamEffects.CameraPan(new Vector3(20,29,-10),"");
		CamManager.Instance.mainCamEffects.ZoomInOut(.7f,1f);
		timeBack.SetActive(true);
		SoundManager.instance.musicSource.volume = 0;
		SoundManager.instance.backupMusicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
		SoundManager.instance.backupMusicSource.clip = timeTentMusic;
		SoundManager.instance.backupMusicSource.Play();
		catsParent.GetComponent<Animator>().Play("timecatsRiseUp",-1,0f);
		starSlowdown = false;
		starSimSpeed = 2;
		starsPS.Play();
		yield return new WaitForSeconds(.8f);
		starSlowdown = true;
		yield return new WaitForSeconds(3f);
		timeUpgradeHUD.SetActive(true);
       	timeUpgradeHUD.GetComponent<GUI_TimeUpgrade>().Navigate("");
	}
}

