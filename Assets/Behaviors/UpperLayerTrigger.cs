using UnityEngine;
using System.Collections;

public class UpperLayerTrigger : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Player"){
			if(collider.gameObject.layer == 12){ // switch to 'UpperPlayer' layer if currently on 'Lower level'
				collider.gameObject.layer = 24;
				collider.GetComponent<Renderer>().sortingLayerName ="Layer02";
				gameObject.layer =21; //change this to 'UpperTiles' so it collides with player again
				Debug.Log("Set player layer to 'upperPlayer'");
			}else if(collider.gameObject.layer == 24){//switch back to 'Player' layer from 'UpperPlayer'
				collider.gameObject.layer = 12;
				gameObject.layer =21; //change this to 'UpperTiles' so it collides with player again
				collider.GetComponent<Renderer>().sortingLayerName ="Layer01";
				Debug.Log("Set player layer back to base Player layer");
			}
		}
	}
}

