using UnityEngine;
using System.Collections;
using TMPro;

public class GUI_CollectableFriendHUD : MonoBehaviour
{
	public TextMeshProUGUI collected;
	public TextMeshProUGUI max;

	int numberCollected;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void UpdateCollected(){
		gameObject.GetComponent<Animator>().SetTrigger("Appear");
		numberCollected++;
		collected.text = numberCollected.ToString();
	}
}

