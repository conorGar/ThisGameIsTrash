using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_MainCamera : MonoBehaviour {

	public float MIN_X = -9.4f;
	public float MAX_X = -9.4f;
	public float MIN_Y = -9.08f;
	public float MAX_Y = -9.08f;
	public bool stableCamera = false;//set true in scenes where the camera doesnt move
    public float cameraSpeed = 10.0f;


    Vector3 targetPos;
	bool transitioning;
	Vector3 velocity = Vector3.zero;
	List<GameObject> activeEnemies = new List<GameObject>();
	float screenShake;
	float startShakeX;
	float startShakeY;
	//int hitbounds = 0;
	GameObject player;
	Vector3 offset;

	void Start(){
		player = GameObject.Find("Jim");


		/*offset = transform.position - player.transform.position;
		Debug.Log(offset);*/

    }

	void Update () {
		
			if(!transitioning){
				if(screenShake == 0){
					if(stableCamera == false){
<<<<<<< HEAD
//<<<<<<< HEAD
						transform.position = new Vector3(player.transform.position.x,player.transform.position.y,-10f); // follows only when player is in center of screen
//=======
						transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, player.transform.position.x, Time.deltaTime * cameraSpeed),
                                                         Mathf.SmoothStep(transform.position.y, player.transform.position.y, Time.deltaTime * cameraSpeed),
                                                         -10f); // follows only when player is in center of screen

//>>>>>>> refs/remotes/origin/digital_smash
						transform.position = new Vector3(
							Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
							Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y),
							-10f);

=======
						transform.position = new Vector3(Mathf.Clamp(Mathf.SmoothStep(transform.position.x, player.transform.position.x, Time.deltaTime * cameraSpeed), MIN_X, MAX_X),
                                                         Mathf.Clamp(Mathf.SmoothStep(transform.position.y, player.transform.position.y, Time.deltaTime * cameraSpeed), MIN_Y, MAX_Y),
                                                         -10f); // follows only when player is in center of screen

>>>>>>> refs/remotes/origin/digital_smash
						/*if(transform.position.x >= MAX_X || transform.position.x <= MIN_X ){
							hitbounds = 1; //=1:no x movement, =2: no y movement
						}else if(transform.position.y >= MAX_Y || transform.position.y <= MIN_Y ){
							hitbounds = 2; //=1:no x movement, =2: no y movement
						}

						if(hitbounds == 0){
							transform.position = player.transform.position + offset;
						}else if(hitbounds == 1){
							transform.position = new Vector3(transform.position.x,player.transform.position.y + offset.y,-10f);
								if(Mathf.Abs(player.transform.position.x - transform.position.x) < 10f)
									hitbounds = 0;
						}else if(hitbounds == 2){
							transform.position = new Vector3(player.transform.position.x,transform.position.y+offset.y,-10f);
							if(Mathf.Abs(player.transform.position.y - transform.position.y) < 10f)
								hitbounds = 0;
						}*/
		
					}
				}else{
					transform.position = new Vector3(startShakeX + Random.Range(0f, screenShake),startShakeY + Random.Range(0f, screenShake), -10f);
				}

			}else{
				transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity,0.2f);
			}


	}//end of update

	public void Transition(string direction, string rName){
		
		ActivateEnemies(rName);
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		if(direction.CompareTo("left") == 0){
			targetPos = transform.TransformPoint(new Vector3(MIN_X - transform.position.x,0f,0f));
			Debug.Log("TARGET POSITION:" + targetPos.x);
		}else if(direction.CompareTo("right") == 0){
			targetPos = gameObject.transform.TransformPoint(new Vector3(MIN_X - transform.position.x,0f,0f));
		}else if(direction.CompareTo("up") == 0){
			targetPos = gameObject.transform.TransformPoint(new Vector3(0f,MIN_Y - transform.position.y,0f));
		}else if(direction.CompareTo("bot") == 0){
			targetPos = gameObject.transform.TransformPoint(new Vector3(0f,MAX_Y - transform.position.y,0f));
		}

		Debug.Log("Player can move = " + GlobalVariableManager.Instance.PLAYER_CAN_MOVE);
		transitioning = true;

		StartCoroutine("StopTransition");
	}

//<<<<<<< HEAD
	/*public void SetMinMax(float minX, float maxX, float minY, float maxY){
	//activated by BoundSetter script
		MIN_X = minX;
		MIN_Y = minY;
		MAX_X = maxX;
		MAX_Y = maxY;
		Debug.Log(MIN_X);
		Debug.Log(MIN_Y);
		Debug.Log(MAX_X);
		Debug.Log(MAX_Y);
	}*/

//=======
    public void SetMinMax(Room room)
    {
        Rect rect = room.GetRoomCameraBoundaries();
        MIN_X = rect.xMin;
        MAX_X = rect.xMax;
        MIN_Y = rect.yMin;
        MAX_Y = rect.yMax;
    }

    public void SetMinMax(float minX, float maxX, float minY, float maxY)
    {
        //activated by BoundSetter script
        MIN_X = minX;
        MIN_Y = minY;
        MAX_X = maxX;
        MAX_Y = maxY;
        Debug.Log(MIN_X);
        Debug.Log(MIN_Y);
        Debug.Log(MAX_X);
        Debug.Log(MAX_Y);
    }

    /*
>>>>>>> refs/remotes/origin/digital_smash
	public void SetMax_X(float max){
		MAX_X = max;
	}
	public void SetMin_X(float min){
		MIN_X = min;
	}
	public void SetMax_Y(float max){
		MAX_Y = max;
	}
	public void SetMin_Y(float min){
		MIN_Y = min;
<<<<<<< HEAD
	}
	*/
///=======
	//}

//>>>>>>> refs/remotes/origin/digital_smash

    IEnumerator StopTransition(){
		yield return new WaitForSeconds(.5f);
		transitioning = false;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		Debug.Log("Player can move = " + GlobalVariableManager.Instance.PLAYER_CAN_MOVE);


	}

	void ActivateEnemies(string roomName){
		Debug.Log("ACTIVATE ENEMIES ACTIVATED -------" + roomName);
		//--------Deactivate Previous Room's Enemies------------//
		if(activeEnemies.Count > 0){
			for(int i = 0; i<activeEnemies.Count;i++){
				activeEnemies[i].SetActive(false);
			}
			activeEnemies.Clear();
		}
		//-----------------------------------------------------//
		GameObject parentRoom = GameObject.Find(roomName);
		for(int i = 0; i< parentRoom.transform.childCount; i++){
			Transform child =parentRoom.transform.GetChild(i);
			if(child.tag == "Enemy"){
				activeEnemies.Add(child.gameObject);
			}
		}
		Debug.Log("Number of enemies in room:" + activeEnemies.Count);
		//-------activate this room's enemies----//
		if(activeEnemies.Count > 0){
			for(int i = 0; i<activeEnemies.Count;i++){
				activeEnemies[i].SetActive(true);

			}
		}
		//-----------------------------------------//
	}//end of ActivateEnemies()


	public IEnumerator ScreenShake(float time){
		Debug.Log("SCREEN SHAKE ACTIVATE----------");
		screenShake = .2f;
		startShakeX = transform.position.x;
		startShakeY = transform.position.y;
		gameObject.transform.parent = null;//unattach from player
		yield return new WaitForSeconds(time);
		//if(!transitioning && !stableCamera)//stable camera check - no jim on stable camera scenes.
		//	gameObject.transform.parent = GameObject.Find("Jim").transform; // attatch back
		screenShake = 0;
	}
}
