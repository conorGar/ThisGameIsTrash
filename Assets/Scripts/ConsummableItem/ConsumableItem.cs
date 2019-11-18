using UnityEngine;
using System.Collections;



public enum CONSUMABLE: long{
	NONE =                              0,
    BREAD =                  (long)1 << 0,
    PBJ =                	 (long)1 << 1,
    THINGTHATDAMAGESENEMIES =(long)1 << 2
}

[CreateAssetMenu(fileName = "New Item", menuName = "TGIT/Item", order = 5)]
public class ConsumableItem : ScriptableObject
{

	public CONSUMABLE Type
    {
        set { itemValue = (long)value; }
        get { return (CONSUMABLE)itemValue; }
    }

	[HideInInspector]
    public long itemValue;

	public string displayName = "New Pin";
    public string description = "New Pin Description";
	public string sprite = "sprite_name";
	public int price = 1;

}

