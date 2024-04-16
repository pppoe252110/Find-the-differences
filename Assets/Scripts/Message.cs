using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public static Message Instance
    {
        get
        {
            if(_instance == null)
                _instance = FindAnyObjectByType<Message>();
            return _instance;
        }
    }
    private static Message _instance;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _instance = this;
    }

    public void DoMessage(string text)
    {
        StopAllCoroutines();
        StartCoroutine(DoText(text));
    }

    private IEnumerator DoText(string text)
    {
        _text.text = text;
        yield return new WaitForSeconds(1f);
        _text.text = "";
    }
}
