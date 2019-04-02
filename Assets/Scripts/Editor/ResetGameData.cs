using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using SimpleJSON;
using UnityEditor;


public class ResetGameData : MonoBehaviour {
    [MenuItem("TGIT/Reset Slot One Data")]
    static void ResetSlotOneData () {
        ResetData(0);
	}

    [MenuItem("TGIT/Reset Slot Two Data")]
    static void ResetSlotTwoData()
    {
        ResetData(1);
    }

    [MenuItem("TGIT/Reset Slot Three Data")]
    static void ResetSlotThreeData()
    {
        ResetData(2);
    }

    static void ResetData (int slot) {
        string directory_path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "TGIT");
        if (!Directory.Exists(directory_path)) {
            return;
        }

        string fileName = Path.Combine(directory_path, "UserData_" + slot + ".json");

        if (File.Exists(fileName)) {
            File.Delete(fileName);
        }

        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("Data in Slot: " + slot + " has been deleted!");
    }
}
