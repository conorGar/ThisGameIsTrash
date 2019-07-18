using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossGeneralGrubTank : MonoBehaviour
{
	int currentMark;
	Vector3 destinationPos;
	public List<GameObject> pathMarks = new List<GameObject>(); //public for debugging, otherwise should be given by enemy spawner
	Vector3 startingScale;
	protected tk2dSpriteAnimator anim;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Vector2.Distance(gameObject.transform.position, destinationPos) > 1) {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, destinationPos, 4 * Time.deltaTime);
        } else {
            NextMark();
        }
	}


	void NextMark(){
		
		if(currentMark < (pathMarks.Count-1)){
			currentMark++;
		}else{
			currentMark = 0;
		}


        // use the enemy path grid if they have one.  Tries to find a close position to the pathMark if it can.
      

            destinationPos = (pathMarks[currentMark].transform.position);

    }
}

