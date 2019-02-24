using UnityEngine;
using System.Collections;
using TMPro;
public class GUI_TotalTrashDisplay : MonoBehaviour
{
	public TextMeshProUGUI displayText;
	// Use this for initialization
	void OnEnable(){
		displayText.text = GlobalVariableManager.Instance.TOTAL_TRASH.ToString();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	public void UpdateDisplay(){
		displayText.text = GlobalVariableManager.Instance.TOTAL_TRASH.ToString();
	}

}

