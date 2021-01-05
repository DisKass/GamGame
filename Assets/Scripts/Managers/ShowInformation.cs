using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInformation : MonoBehaviour
{
    [SerializeField] Text _version;
    [SerializeField] Text _fps;
    [SerializeField] Text _messageBox;
    int _numberOfLines = 4;
    void Start()
    {
        if (_version != null)
            _version.text = GameText.Instance.GetText("version") + Application.version;
        if (_fps != null)
            StartCoroutine(UpdateFPS());
    }
    private void UpdateFps()
    {
        _fps.text = Mathf.Round(1 / Time.deltaTime).ToString();
        //Debug.Log(Time.deltaTime);
    }
    IEnumerator UpdateFPS()
    {
        while (true)
        {
            UpdateFps();
            yield return new WaitForEndOfFrame();
        }
    }

    public void AddMessage(string message)
    {
        if (_messageBox == null) return;
        string currentText = _messageBox.text;
        if (_numberOfLines == 4)
        {
            _messageBox.text = currentText.Substring(currentText.IndexOf('\n')+1) + '\n' + message;

        }
    }
}
