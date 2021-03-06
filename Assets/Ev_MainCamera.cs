﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_MainCamera : MonoBehaviour {

	public float MIN_X = -9.4f;
	public float MAX_X = -9.4f;
	public float MIN_Y = -9.08f;
	public float MAX_Y = -9.08f;
	public bool stableCamera = false;//set true in scenes where the camera doesnt move

    [SerializeField] float currentCameraSpeed = 10f;
    [SerializeField] float normalCameraSpeed = 10f;
    [SerializeField] float fastCameraSpeed = 20f;
    [SerializeField] float slowCameraSpeed = 5f;


    Vector3 targetPos;
	bool transitioning;
	Vector3 velocity = Vector3.zero;
	List<GameObject> activeEnemies = new List<GameObject>();
	float screenShake;
	float startShakeX;
	float startShakeY;
	//int hitbounds = 0;
	Vector3 offset;

    void Awake()
    {
        SetNormalCameraSpeed();
    }

    void Start(){
        if (PlayerManager.Instance.player != null)
            transform.position = new Vector3(PlayerManager.Instance.player.transform.position.x, PlayerManager.Instance.player.transform.position.y, transform.position.z);
    }

	void Update () {
			if(GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)){
			    if(!transitioning){
				    if(screenShake == 0){
					    if(stableCamera == false){
						    transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, PlayerManager.Instance.player.transform.position.x, Time.deltaTime * currentCameraSpeed),
                                                             Mathf.SmoothStep(transform.position.y, PlayerManager.Instance.player.transform.position.y, Time.deltaTime * currentCameraSpeed),
                                                             -10f); // follows only when player is in center of screen

						    transform.position = new Vector3(
							    Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
							    Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y),
							    -10f);

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
				    }

			    }else{
				    transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity,0.2f);
			    }
			}

			if(screenShake != 0){
				transform.position = new Vector3(startShakeX + Random.Range(0f, screenShake),startShakeY + Random.Range(0f, screenShake), -10f);	  
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

    public void SetMinMax(Room room)
    {
        Rect rect = room.GetRoomCameraBoundaries();
        MIN_X = rect.xMin;
        MAX_X = rect.xMax;
        MIN_Y = rect.yMin;
        MAX_Y = rect.yMax;
    }



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
    public void SetNormalCameraSpeed()
    {
        currentCameraSpeed = normalCameraSpeed;
    }

    public void SetFastCameraSpeed()
    {
        currentCameraSpeed = fastCameraSpeed;
    }

    public void SetSlowCameraSpeed()
    {
        currentCameraSpeed = slowCameraSpeed;
    }


    public void ScreenShake(float time)
    {
		screenShake = .2f;
        StartCoroutine(ScreenShakeEnumerator(time));
    }
	public void ScreenShake(float time, float intensity)
    {
		screenShake = intensity;
        StartCoroutine(ScreenShakeEnumerator(time));
    }

	public IEnumerator ScreenShakeEnumerator(float time){

		startShakeX = transform.position.x;
		startShakeY = transform.position.y;
		gameObject.transform.parent = null;//unattach from player
		yield return new WaitForSeconds(time);
		//if(!transitioning && !stableCamera)//stable camera check - no jim on stable camera scenes.
		screenShake = 0;
	}
}
