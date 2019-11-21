using UnityEngine;
using System.Collections;

public class Ev_DroppedWeapon : MonoBehaviour
{

	WeaponDefinition myWeapon;
	public GameObject shadow;

	TimedLeapifier timedLeapifier;  // leaping, in a bouncy manner
	public float maxBounceHeight; // maximum bounce height
    public float totalBounceTime; // how long the whole bounce animation will run
    public GameObject bounceDest; // where the pin will be after it stops bouncing
    public AnimationCurve bounceCurve; // bouncy y-value curve

	void OnEnable () {
        Bounce();
	}

	void OnDisable()
    {
        if (timedLeapifier != null) {
            timedLeapifier.Reset();
        }
    }

	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (timedLeapifier != null) {
                if (timedLeapifier.OnUpdate()) {
                    timedLeapifier.Reset();
                    timedLeapifier = null;
                }
            }
	}

	void OnTriggerStay2D(Collider2D collider){
		//TODO: only pick up if press button
		if(collider.gameObject.tag == "Player" && GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)){
			if((GlobalVariableManager.Instance.WeaponInventory.Count + GlobalVariableManager.Instance.CONSUMABLE_INVENTORY.Count ) < GlobalVariableManager.Instance.MaxInventorySize){
				GlobalVariableManager.Instance.WeaponInventory.Add(myWeapon);
				ObjectPool.Instance.ReturnPooledObject(this.gameObject);
			}
		}
	}

	void Bounce(){
        timedLeapifier = new TimedLeapifier(gameObject, shadow, maxBounceHeight, totalBounceTime, bounceDest.transform.position, bounceCurve);
	}

	public void setWeaponData(WeaponDefinition data){
        myWeapon = data;
		gameObject.GetComponent<tk2dSprite>().SetSprite(data.sprite);


	}
}

