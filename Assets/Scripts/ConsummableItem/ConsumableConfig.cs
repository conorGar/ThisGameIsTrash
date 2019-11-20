using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ConsumableConfig", menuName = "TGIT/ItemConfig", order = 6)]
public class ConsumableConfig : ScriptableObject
{

	public List<ConsumableItem> itemList;

}

