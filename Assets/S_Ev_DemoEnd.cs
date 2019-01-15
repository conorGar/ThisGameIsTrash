using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class S_Ev_DemoEnd : MonoBehaviour {
	public GameObject ratOnAMat;
	public GameObject demoEndFader;
	public GameObject demoEndText;
	public Ev_FadeHelper fader;

	// Use this for initialization
	void Start () {
		GameStateManager.Instance.PushState(typeof(GameplayState));
		ratOnAMat.GetComponent<ActivateDialogWhenClose>().dialogDefiniton = ratOnAMat.GetComponent<RatOnMatFriend>().myDialogDefiniton;
		StartCoroutine("DelayEffect");
	}


	IEnumerator DelayEffect(){
		yield return new WaitForSeconds(3f);
		Debug.Log("Executed Rat on A Mat activate Dialog");
		ratOnAMat.GetComponent<ActivateDialogWhenClose>().Execute();
	}

	public IEnumerator End(){
			demoEndFader.SetActive(true);
			yield return new WaitForSeconds(2f);
			demoEndText.SetActive(true);
			Debug.Log("Got here - demo end");
			int currentSaveSlot = UserDataManager.Instance.GetSlot();
			yield return new WaitForSeconds(2f);
			ResetData(currentSaveSlot);
			GlobalVariableManager.Instance.SetDefaultStats();
			SoundManager.instance.backupMusicSource.Stop();
			GameStateManager.Instance.PopAllStates();
			SoundManager.instance.backupMusicSource.Stop();
			Application.Quit();

			//fader.FadeToScene("TitleScreen2");
	}

	static void ResetData(int slot)
    {
        string directory_path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "TGIT");
        if (!Directory.Exists(directory_path)) {
            return;
        }

        string fileName = Path.Combine(directory_path, "UserData_" + slot + ".json");

        if (File.Exists(fileName)) {
            File.Delete(fileName);
        }

        Debug.Log("Data in Slot: " + slot + " has been deleted!");
    }
	

}
