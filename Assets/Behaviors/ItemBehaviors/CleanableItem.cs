using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CleanableItem : MonoBehaviour
{
    public Cleanable cleanable = null;

    [HideInInspector]
    public long cleanableBitValue = (long)CLEANABLE_BIT.NONE;
    public CLEANABLE_BIT cleanableBit
    {
        set { cleanableBitValue = (long)value; }
        get { return (CLEANABLE_BIT)cleanableBitValue; }

    }

    public int hp;
	public int spawnChance = 0; // out of 10
	public List<GameObject> possibleSpawnableItems = new List<GameObject>();
	public GameObject dirtyLookingObject;
	bool isClean;
	public ParticleSystem dirtyPS;
	public AreaTrashHUD myHUD;


	protected virtual void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Weapon"){
			if(hp > 0){
				hp--;
				ObjectPool.Instance.GetPooledObject("effect_dirtyHit",gameObject.transform.position);
				gameObject.GetComponent<Animator>().Play("dirtyHitBounce",0,-1f);
			
			}else{
                if (!isClean) {
                    CleanableManager.Instance.Clean(cleanable, cleanableBit);
                    SpawnItem();
                }
			}
		}
	}

    public void InitClean(CLEANABLE_BIT bits)
    {
        if ((cleanableBit & bits) == cleanableBit) {
            SetClean();
        }
    }

	void SpawnItem(){
		int spawnsItem = Random.Range(spawnChance,11);
		ObjectPool.Instance.GetPooledObject("effect_dirtyHit",gameObject.transform.position);

		if(spawnsItem == 10){
			Debug.Log("Spawns Item");
			GameObject myObject = ObjectPool.Instance.GetPooledObject(possibleSpawnableItems[Random.Range(0,possibleSpawnableItems.Count)].tag,gameObject.transform.position);
			myObject.transform.parent = this.transform;
			//TODO: System for determining how the item comes out
			if(myObject.GetComponent<Animator>() != null)
				myObject.GetComponent<Animator>().SetTrigger("Right");

		
		}

		ObjectPool.Instance.GetPooledObject("effect_cleanSparkles",gameObject.transform.position);
        SetClean();
	}

    // helpers
    void SetClean()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        dirtyPS.Stop();
        dirtyLookingObject.SetActive(false);
		myHUD.AddCleanedFilty();
        isClean = true;
    }

    // gizmos
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        style.normal.background = Texture2D.whiteTexture;
        if (cleanable != null) {
            style.normal.textColor = cleanable.gizmoColor;
            UnityEditor.Handles.Label(transform.position + (Vector3.up * 1f), cleanable.CleanableType().ToString(), style);
            if (cleanableBit == CLEANABLE_BIT.NONE) {
                style.normal.textColor = Color.red;
            }

            UnityEditor.Handles.Label(transform.position, cleanableBit.ToString(), style);
        } else {
            style.normal.textColor = Color.red;
            UnityEditor.Handles.Label(transform.position + (Vector3.up * 1f), "ASSIGN CLEANABLE!", style);
        }
#endif
    }
}

