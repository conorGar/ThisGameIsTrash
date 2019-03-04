using UnityEngine;
using System.Collections;

public class CheckpointManager : MonoBehaviour
{
	public static CheckpointManager Instance;
	public Checkpoint lastCheckpoint;

	void Awake(){
		Instance = this;
	}


	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

