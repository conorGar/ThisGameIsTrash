using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class AreaTrashHUD : MonoBehaviour
{
	public TextMeshProUGUI totalFilty;
	int cleanedFilty;
	public TextMeshProUGUI cleanedFiltyDisplay;

	// Use this for initialization
	void Start ()
	{
		cleanedFiltyDisplay.text = cleanedFilty.ToString();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void AddCleanedFilty(){
		cleanedFilty++;
		cleanedFiltyDisplay.text = cleanedFilty.ToString();
	}



}

