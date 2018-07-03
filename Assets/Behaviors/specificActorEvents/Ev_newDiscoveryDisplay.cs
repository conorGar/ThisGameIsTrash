using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ev_newDiscoveryDisplay : MonoBehaviour {

	public GameObject myTrash;

	Text myText;
	Vector2 refVelocity;
	// Use this for initialization
	void Start () {
		gameObject.transform.position = Vector2.SmoothDamp(transform.position, new Vector2(gameObject.transform.position.x + 2f,gameObject.transform.position.y), ref refVelocity,20f,10f,Time.deltaTime);

		StartCoroutine("PhaseChange");
	}
	
	// Update is called once per frame
	void Update () {
		if(GlobalVariableManager.Instance.SCENE_IS_TRANSITIONING)
			Destroy(gameObject);
	}

	public void KillMyTrash(){
		if(myTrash != null){
			Destroy(myTrash);
		}
	}

	IEnumerator PhaseChange(){
		yield return new WaitForSeconds(4f);
		gameObject.transform.position = Vector2.SmoothDamp(transform.position, new Vector2(gameObject.transform.position.x - 2f,gameObject.transform.position.y), ref refVelocity,20f,10f, Time.deltaTime);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}

	public void SetMyTrash(GameObject go){

	}

	public void SetWhichTrash(int whichTrash, string anim){
		if(whichTrash == 1){
			myText.text = "Crumpled Paper";
		}else if(whichTrash == 2){
			myText.text = "Last Year's Phone";
		}else if(whichTrash == 3){
			myText.text = "Hardcover Book";
		}else if(whichTrash == 4){
			myText.text = "Art Supplies";
		}else if(whichTrash == 5){
			myText.text = "Spicy Chip Bag";
		}else if(whichTrash == 6){
			myText.text = "Pot. Chip Bag";
		}else if(whichTrash == 7){
			myText.text = "BBQ Chip Bag";
		}else if(whichTrash == 8){
			myText.text = "Casette";
		}else if(whichTrash == 9){
			myText.text = "Chinese Leftovers";
		}else if(whichTrash == 10){
			myText.text = "Juice Box";
		}else if(whichTrash == 11){
			myText.text = "Dead Cat";
		}else if(whichTrash == 12){
			myText.text = "Chipped Mug";
		}else if(whichTrash == 13){
			myText.text = "Party Leftovers";
		}else if(whichTrash == 14){
			myText.text = "Used Sock";
		}else if(whichTrash == 15){
			myText.text = "Tissue Box";
		}else if(whichTrash == 16){
			myText.text = "Toilet Paper";
		}else if(whichTrash == 17){
			myText.text = "Wad of Hair";
		}else if(whichTrash == 18){
			myText.text = "Fish Carcass";
		}else if(whichTrash == 19){
			myText.text = "Used Needle";
		}else if(whichTrash == 20){
			myText.text = "A Baby";
		}else if(whichTrash == 21){
			myText.text = "Severed Arm";
		}else if(whichTrash == 22){
			myText.text = "Childhood Memories";
		}else if(whichTrash == 23){
			myText.text = "Gift For Mom";
		}
		myTrash.GetComponent<tk2dSprite>().SetSprite(anim);
	}
}
