using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_GlowWhenClose : MonoBehaviour {

	public float distanceUntilGlow;
	public float xSpawnAdjust = 0f;
	public float ySpawnAdjust = 0f;
	public bool activateSomethingWithSpace;
	public bool spawnSomethingWithGlow;
	//public MonoBehaviour behaviorToActivate;
	//public string methodToActivate;
	public Sprite glowSprite;
	public Sprite spawnedObjectSprite;
	public GameObject objectToSpawn;
	public AudioClip highlightSound;

	Sprite startSprite;
	[HideInInspector]
	public GameObject player;
	GameObject tempSpawnedObject;

	int glowCheck = 0;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		startSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
	}

	void Update(){
		if(player!=null){
			if(Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < distanceUntilGlow && Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) < distanceUntilGlow){
				if(glowCheck == 0){
					if(glowSprite != null){
						gameObject.GetComponent<SpriteRenderer>().sprite = glowSprite;
					}
					if(spawnSomethingWithGlow){
						tempSpawnedObject = Instantiate(objectToSpawn, new Vector2(transform.position.x + xSpawnAdjust,transform.position.y + ySpawnAdjust), Quaternion.identity);
						if(spawnedObjectSprite != null){
							tempSpawnedObject.GetComponent<SpriteRenderer>().sprite = spawnedObjectSprite;
						}
					}
					SoundManager.instance.PlaySingle(highlightSound);
					glowCheck = 1;
				}else if(activateSomethingWithSpace){
					if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
                    {
						Activate();
					}
				}
			}else{ //if further away than distance
				if(glowCheck > 0){
					gameObject.GetComponent<SpriteRenderer>().sprite = startSprite;
					if(tempSpawnedObject != null){
						Destroy(tempSpawnedObject);
					}
					glowCheck = 0;
				}
			}
		}
	}//end of update()

	public virtual void Activate(){
		Debug.Log("activate");
		if(gameObject.name == "Dumpster"){
			GameObject largeTrash = GameObject.FindGameObjectWithTag("ActiveLargeTrash");
			if(largeTrash != null){
				largeTrash.GetComponent<Ev_LargeTrash>().dumpster = this.gameObject;
				largeTrash.GetComponent<Ev_LargeTrash>().Return();
				Debug.Log("Large trash return activated here");
			}else{
				gameObject.GetComponent<Ev_Dumpster>().Activate();
				this.enabled = false; //deactivates this behavior, should be reativated at the end of whatever method is activated
				Debug.Log("Dumpster activate() activated here");
			}

		}
	}//end of Activate()

	public void SetGlowCheck(int i){
		glowCheck = i;
	}
}
