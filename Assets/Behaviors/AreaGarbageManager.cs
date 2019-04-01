using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaGarbageManager : MonoBehaviour
{
	/*public List<CleanableItem> filty = new List<GameObject>();
	public List<GameObject> slime = new List<GameObject>();
	public List<GameObject> bags = new List<GameObject>();
	public List<GameObject> grease = new List<GameObject>();
	public List<GameObject> oil = new List<GameObject>();*/
	public AreaTrashHUD myHUD;
	public int totalFiltyInArea;
	// Use this for initialization
	void Start ()
	{
		
		myHUD.totalFilty.text = totalFiltyInArea.ToString();

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void CleanedFilty(){ // activated by 'CleanableItem.cs'
		myHUD.AddCleanedFilty();


	}
}

