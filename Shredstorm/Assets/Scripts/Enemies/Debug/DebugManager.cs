using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager I;
    [SerializeField] private int maxMessages = 8;

    private readonly List<string> buffer = new List<string>();

    void Awake()
    {
        // singleton
        I = this;
    }

    /// <summary>
    /// Call this from anywhere instead of Debug.Log
    /// </summary>
    public static void Log(string msg)
    {
        if (I == null) return;
        I.buffer.Add(msg);
        if (I.buffer.Count > I.maxMessages)
            I.buffer.RemoveAt(0);
        Debug.Log(msg);
    }

    void OnGUI()
    {
        // calculate centered area
        float areaWidth = 400;
        float areaHeight = 20 * maxMessages;
        float x = (Screen.width  - areaWidth)  * 0.5f;
        float y = (Screen.height - areaHeight) * 0.5f;
        Rect areaRect = new Rect(x, y, areaWidth, areaHeight);

        // draw black background box
        Color prevBg = GUI.backgroundColor;
        GUI.backgroundColor = Color.black;
        GUI.Box(areaRect, GUIContent.none);
        GUI.backgroundColor = prevBg;

        // white text style
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = Color.white;
        style.fontSize = 14;

        // overlay the messages
        GUILayout.BeginArea(areaRect);
        foreach (var msg in buffer)
        {
            GUILayout.Label(msg, style);
        }
        GUILayout.EndArea();
    }
}