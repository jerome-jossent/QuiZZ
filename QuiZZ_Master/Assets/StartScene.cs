using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class StartScene : MonoBehaviour
{
    public enum PlayerPrefNames
    {
        MQTT_ID,
        MQTT_IP,
        MQTT_Port,
        FolderToSave,
        Save,
        FolderToLoad
    }

    public TMP_InputField if_id;
    public TMP_InputField if_ip;
    public TMP_InputField if_port;
    public TMP_InputField if_folderToSave;
    public Toggle tgl_save;

    public TMP_InputField if_folderToLoad;
    public TMP_Text txt_folderCountFiles;
    public Button btn_PortiKReplay;

    public void Start()
    {
        Fill(if_id, PlayerPrefNames.MQTT_ID);
        Fill(if_ip, PlayerPrefNames.MQTT_IP);
        Fill(if_port, PlayerPrefNames.MQTT_Port);
        Fill(if_folderToSave, PlayerPrefNames.FolderToSave);
        Fill(tgl_save, PlayerPrefNames.Save);
        Fill(if_folderToLoad, PlayerPrefNames.FolderToLoad);
    }

    internal static string GetString(PlayerPrefNames ppn)
    {
        return PlayerPrefs.GetString(ppn.ToString());
    }
    internal static bool GetBool(PlayerPrefNames ppn)
    {
        return PlayerPrefs.GetString(ppn.ToString()) == "True";
    }
    internal static void SetString(PlayerPrefNames ppn, string val)
    {
        PlayerPrefs.SetString(ppn.ToString(), val);
        PlayerPrefs.Save();
    }

    void Fill(TMP_InputField txt, PlayerPrefNames ppn)
    {
        txt.text = GetString(ppn);
    }

    void Fill(Toggle tgl, PlayerPrefNames ppn)
    {
        tgl.isOn = GetBool(ppn);
    }

    void Save(TMP_InputField txt, PlayerPrefNames ppn)
    {
        PlayerPrefs.SetString(ppn.ToString(), txt.text);
        PlayerPrefs.Save();
    }

    void Save(Toggle tgl, PlayerPrefNames ppn)
    {
        PlayerPrefs.SetString(ppn.ToString(), tgl.isOn.ToString());
        PlayerPrefs.Save();
    }

    public void Start_PortikLive()
    {
        Save(if_id, PlayerPrefNames.MQTT_ID);
        Save(if_ip, PlayerPrefNames.MQTT_IP);
        Save(if_port, PlayerPrefNames.MQTT_Port);
        Save(if_folderToSave, PlayerPrefNames.FolderToSave);
        Save(tgl_save, PlayerPrefNames.Save);
        SceneManager.LoadScene(1);
    }
}
