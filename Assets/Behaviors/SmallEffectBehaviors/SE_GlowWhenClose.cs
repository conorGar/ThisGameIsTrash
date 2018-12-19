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
	//GameObject tempSpawnedObject;

	int glowCheck = 0;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		if(glowSprite != null)
			startSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
	}

	protected void Update(){
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (player != null) {
                if (Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < distanceUntilGlow && Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) < distanceUntilGlow) {
                	//Debug.Log("Close enough to glow");
                    if (glowCheck == 0) {
                        if (glowSprite != null) {
                            gameObject.GetComponent<SpriteRenderer>().sprite = glowSprite;
						}
                        GlowFunction();
                        
                        if (spawnSomethingWithGlow) {
                        	objectToSpawn.SetActive(true);
                          
                        }
                        SoundManager.instance.PlaySingle(highlightSound);
                        glowCheck = 1;
                    }
                    else if (activateSomethingWithSpace) {
                        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                            Activate();
                        }
                    }
                }
                else { //if further away than distance
                    if (glowCheck > 0) {
                    	if(startSprite !=null)
                       		 gameObject.GetComponent<SpriteRenderer>().sprite = startSprite;
                        if (objectToSpawn != null) {
                            objectToSpawn.SetActive(false);
                        }
                        StopGlowFunction();
                        
                        glowCheck = 0;
                    }
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
				gameObject.GetComponent<Animator>().Play("dumpsterLargeTrashTake",-1,0f);
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

	public virtual void GlowFunction(){
		//nothing for base
	}

	public virtual void StopGlowFunction(){
		//nothing for base
	}

}
