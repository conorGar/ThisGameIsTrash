using UnityEngine;
using System.Collections;
using TMPro;

public class GUI_SuitUpgradeBox : MonoBehaviour
{

	public string myDescription;
	public int trashCost;
	public TextMeshProUGUI textBox;
	public TextMeshProUGUI costTextDisplay;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Higlight(){
		textBox.text = myDescription;
		costTextDisplay.text = trashCost.ToString();
		gameObject.GetComponent<RectTransform>().localScale= new Vector2(0.6f,0.6f);
	}

	public void Unhighlight(){
		gameObject.GetComponent<RectTransform>().localScale= new Vector2(0.4f,0.4f);

	}

}

