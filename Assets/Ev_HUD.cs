using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_HUD : MonoBehaviour {

	public GameObject promptText;

    public void Start()
    {
        GameStateManager.Instance.RegisterChangeStateEvent(OnChangeState);
    }

    public void OnDestroy()
    {
        GameStateManager.Instance.UnregisterChangeStateEvent(OnChangeState);
    }

    void OnChangeState(System.Type stateType, bool isEntering)
    {
        if (isEntering) {
            if (stateType == typeof(RespawnState))
                GetComponent<CanvasGroup>().alpha = 0;
        }
        else {
            if (stateType == typeof(RespawnState))
                GetComponent<CanvasGroup>().alpha = 1;
        }
    }

    public void Create(GameObject go){
		go.SetActive(true);

		//couldnt get instantiate to work well with GUI effects, so for now
		//I just have popups disabled until needed


		//Instantiate(go,gameObject.transform.localPosition,Quaternion.identity);
		//go.transform.parent = this.gameObject.transform;
	}
}
