using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GUIMainCamera : MonoBehaviour
{

	public void ButtonDismissExample ()
	{
		EventSystem.current.currentSelectedGameObject.GetComponent<GUIEffects> ().DismissObjects ();
	}

	public void ReplayScene ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

}
