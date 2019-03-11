using UnityEngine;
using System.Collections;

public class MopPuddle : MonoBehaviour
{

	

	public void SlowDownEnemy(RandomDirectionMovement behavior){
		if(behavior.mopSlow == false){
			behavior.SlowDown(2f);
			behavior.gameObject.GetComponent<tk2dSprite>().color = Color.blue;
			StopAllCoroutines();
			StartCoroutine("ReturnFromSlow",behavior);
			behavior.mopSlow = true;
		}
	}
	public void SlowDownEnemy(FollowPlayer behavior){
		//behavior.SlowDown(2f);
	}


	IEnumerator ReturnFromSlow(RandomDirectionMovement behavior){
		yield return new WaitForSeconds(1.5f);
		behavior.SpeedUp(2f);
		behavior.gameObject.GetComponent<tk2dSprite>().color = Color.white;
		behavior.mopSlow = false;
	}

}

