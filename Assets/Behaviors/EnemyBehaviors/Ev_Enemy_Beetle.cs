using UnityEngine;
using System.Collections;

public class Ev_Enemy_Beetle : MonoBehaviour
{

	public float maxSpeed;


	float currentSpeed;
	Vector2 destinationPosition;
	bool speedBuildUp;
	int negateValue = 1;
	int roomEdgeMultiplier; // helps bugs not get stuck at room edge

	// Use this for initialization
	void Start ()
	{
		//random start direction
		negateValue = Random.Range(0,2);
		if(negateValue == 0)
			negateValue = -1;


		SetNewDestination();
		currentSpeed = maxSpeed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(GameStateManager.Instance.GetCurrentState() == (typeof(GameplayState))){
			if(Vector2.Distance(gameObject.transform.position,destinationPosition) < 3){
				speedBuildUp = false;
				if(currentSpeed > 2){
					currentSpeed -= .1f;
				}else{
					SetNewDestination();
				}
				//gameObject.transform.position = Vector2.Lerp(gameObject.transform.position,destinationPosition,1*Time.deltaTime);

			}


			if(speedBuildUp && currentSpeed < maxSpeed){
				currentSpeed += .1f;
			}


			gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,destinationPosition,currentSpeed*Time.deltaTime);
		}


	}

	void SetNewDestination(){
		speedBuildUp = true;
		negateValue *= -1;
		float xChange = Random.Range(11,27) + roomEdgeMultiplier;
		gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x*-1,gameObject.transform.localScale.y);
		destinationPosition = new Vector2((gameObject.transform.position.x + xChange) * negateValue,Random.Range(gameObject.transform.position.y -10f,gameObject.transform.position.y + 11f));
		roomEdgeMultiplier =0;


		//destinationPosition = new Vector2(Random.Range(RoomManager.Instance.currentRoom.GetRoomBoundaries().xMin , RoomManager.Instance.currentRoom.GetRoomBoundaries().xMax),RoomManager.Instance.currentRoom.GetRoomBoundaries().y + Random.Range(RoomManager.Instance.currentRoom.GetRoomBoundaries().yMin,RoomManager.Instance.currentRoom.GetRoomBoundaries().yMax));


		//Debug.Log("Bug- set new destination:" + destinationPosition + RoomManager.Instance.currentRoom.GetRoomBoundaries().yMax + RoomManager.Instance.currentRoom.GetRoomBoundaries().yMin);
		//make sure new destination positions aren't outside room boundaries;


		if(destinationPosition.x < RoomManager.Instance.currentRoom.GetRoomBoundaries().xMin){
			destinationPosition.x = RoomManager.Instance.currentRoom.GetRoomBoundaries().xMin;
			roomEdgeMultiplier =6;
		}else if(destinationPosition.x > RoomManager.Instance.currentRoom.GetRoomBoundaries().xMax){
			destinationPosition.x = RoomManager.Instance.currentRoom.GetRoomBoundaries().xMax;
			roomEdgeMultiplier =6;
		}

		if(destinationPosition.y < RoomManager.Instance.currentRoom.GetRoomBoundaries().yMin){
			destinationPosition.y = RoomManager.Instance.currentRoom.GetRoomBoundaries().yMin;
		}else if(destinationPosition.y > RoomManager.Instance.currentRoom.GetRoomBoundaries().yMax){
			destinationPosition.y = RoomManager.Instance.currentRoom.GetRoomBoundaries().yMax;
		}


	}
}

