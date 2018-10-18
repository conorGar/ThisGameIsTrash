using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTriggersBehavior : MonoBehaviour {

	public bool topTransition;
	public bool botTransition;
	public bool leftTransition;
	public bool rightTransition;
	public string myNextRoomName;
	public int myNextRoomNumber;

	/// <summary>
	/// When the player is a lesser x than a left transition, the camera shifts to the left(maybe until the end of it = this current
	//transitioner) than this transitioner becomes a 'right transition'.

	//the camera stops when its x< a left transition, > a right transition, above a top transitioner, or below a bottom transiter.

	//maybe there are 4 transitioners attacted to the camera. upon enter a new room, these transitioners jump to the location
	//of their respective left, right, top and bottom markers(if all 4 are in this room.)


	// the transitioners spawn when enter the room just like everything else. Each transitioner is labeled in the inspector(left,right,up,down)
	//and is enables/disabled upon entering/leaving the room. Maybe wehn enabled, each transitioner sets the bounds of the camera based
	//on its location and what direction transitioner it is(meaning even if a room doesnt exit all 4 sides, each room still needs a
	//transitioner for all directions since they serve as the room bounds as well
	/// </summary>
	public GameObject currentCamera;
	public GameObject player;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*public void EnableAgain(){

		if(rightTransition){
			mainCam.SetMax_X(gameObject.transform.position.x);
		}else if(leftTransition){
			mainCam.SetMin_X(gameObject.transform.position.x);
		}else if(topTransition){
			mainCam.SetMax_Y(gameObject.transform.position.y);
		}else if(botTransition){
			mainCam.SetMin_Y(gameObject.transform.position.y);
		}

	}*/

	void OnTriggerEnter2D(Collider2D collision){
		if(rightTransition){
            CamManager.Instance.mainCam.Transition("right",myNextRoomName);
		}
	}
}
