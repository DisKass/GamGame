using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GameText : Singleton<GameText>
{
    LanguageController lang;
    UnityWebRequest request;
    public enum Language:int
    {
        ENGLISH,
        RUSSIAN
    };
    private string[] languagesNames = new string[]
    {
        "English", "Russian"
    };
    string languageToSet;

    public Events.EventLanguageChanged OnLanguageChanged = new Events.EventLanguageChanged();
    public bool Initialized = false;
    protected override void Awake()
    {
        base.Awake();

        string path = Path.Combine(Application.streamingAssetsPath, "lang.xml");
        www = UnityWebRequest.Get(path);

        SetLanguage(Language.RUSSIAN);
    }
    public void SetLanguage(Language language)
    {
        languageToSet = languagesNames[(int)language];
        www.SendWebRequest().completed += Completed;
    }


    UnityWebRequest www;

    private void Completed(AsyncOperation obj)
    {
        // Show results as text
        //Debug.Log("Text: " + www.downloadHandler.text);

        lang = new LanguageController(www.downloadHandler.text, languageToSet, false);
        Initialized = true;
        OnLanguageChanged.Invoke();
    }

    public string GetText(string name)
    {
        return lang.getString(name);
    }
}
