using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using SimpleJSON;

// Steam is pretty straight forward in regards to saving to disk.  It's not asyncronous at all.
public class SteamUserDataManager : UserDataManager {
    public override IEnumerator ReadAsync(Action callback = null)
    {
        yield return WhileReading();

        isReading = true;
        string fileName = Path.Combine(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "TGIT"), "UserData_" + slot + ".json");
        string text = "";
        try
        {
            string line;

            StreamReader reader = new StreamReader(fileName, Encoding.Default);
            do
            {
                line = reader.ReadLine();

                if (line != null)
                {
                    text += line;
                }
            }
            while (line != null);

            reader.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }

        // Create default data if no data is found.
        if (string.IsNullOrEmpty(text))
        {
            yield return WriteAsync();
        }
        else
        {

            JSONObject jsonObject = StringToJson(text).AsObject;
            SetUserItemData(jsonObject);
        }

        yield return base.ReadAsync(callback);
        isReading = false;
    }

    protected override IEnumerator WriteAsync()
    {
        yield return WhileWriting();

        isWriting = true;

        string directory_path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "TGIT");
        if (!Directory.Exists(directory_path))
        {
            Directory.CreateDirectory(directory_path);
        }

        string fileName = Path.Combine(directory_path, "UserData_" + slot + ".json");

        JSONObject json_data = GetUserItemData();

        try
        {
            var sw = new StreamWriter(fileName, false, Encoding.Default);
            sw.Write(json_data.ToString());
            sw.Close();
        }
        catch (System.Exception e)
        {
            Debug.Log("{0}\n" + e.Message);
        }

        PlayerPrefs.Save();   
        yield return base.WriteAsync();
        isWriting = false;
    }
}
