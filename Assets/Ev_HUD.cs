using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_HUD : MonoBehaviour {
    public void Start()
    {
        GameStateManager.Instance.RegisterEnterEvent(typeof(RespawnState), OnEnterRespawnState);
        GameStateManager.Instance.RegisterLeaveEvent(typeof(RespawnState), OnLeaveRespawnState);
    }

    public void OnDestroy()
    {
        GameStateManager.Instance.UnregisterEnterEvent(typeof(RespawnState), OnEnterRespawnState);
        GameStateManager.Instance.UnregisterLeaveEvent(typeof(RespawnState), OnLeaveRespawnState);
    }

    void OnEnterRespawnState()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }

    void OnLeaveRespawnState()
    {
        GetComponent<CanvasGroup>().alpha = 1;
    }

    public void Create(GameObject go){
		go.SetActive(true);

		//couldnt get instantiate to work well with GUI effects, so for now
		//I just have popups disabled until needed


		//Instantiate(go,gameObject.transform.localPosition,Quaternion.identity);
		//go.transform.parent = this.gameObject.transform;
	}
}
