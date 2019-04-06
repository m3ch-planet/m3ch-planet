using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ARDebugger : MonoBehaviour
{
    public GameObject LogGameObject;
    public GameObject PersistLogGameObject;
    TextMeshProUGUI log, persistLog;
    float t;
    // Start is called before the first frame update
    void Start()
    {
        log = LogGameObject.GetComponent<TextMeshProUGUI>();
        persistLog = PersistLogGameObject.GetComponent<TextMeshProUGUI>();
        t = 0.1f;
    }

    private void Update()
    {
        log.text = "";
        if (UnityEngine.Time.time > t)
        {
            persistLog.text = "";
            t += 20;
        }
    }

    public void Log(string s)
    {
        log.text += s + "\n";
    }

    public void LogPersist(string s)
    {
        persistLog.text += s + "\n";
    }
}
