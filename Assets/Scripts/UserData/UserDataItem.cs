using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

// little generic byte buffer storage item.
public class StorageItem
{
    public StorageItem(bool p_isReady, byte[] p_buffer)
    {
        IsReady = p_isReady;
        IsDirty = false;
        Buffer = p_buffer;
    }

    private bool is_ready;
    public bool IsReady
    {
        get { return is_ready; }
        set { is_ready = value; }
    }

    private bool is_dirty;
    public bool IsDirty
    {
        get { return is_dirty; }
        set { is_dirty = value; }
    }

    private byte[] buffer;
    public byte[] Buffer
    {
        get { return buffer; }
        set { buffer = value; }
    }
}

// inheritable abstract class for anything in the game that wants to define save-able user data.
public abstract class UserDataItem : MonoBehaviour {

    public abstract string UserDataKey(); // unique string key that matches this UserDataItem's type;
    public abstract JSONObject Save(); // Save data from the current game state.
    public abstract void Load(JSONObject jsonObject); // Load data into the current game state.
}
