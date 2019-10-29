using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Net.Sockets;
using System.Net;
using System.Text;

/*
 * Broadcast all Debug Log messages on the current WiFi network or displayed on VR overlay UI
 * based on Peter Koch <peterept@gmail.com>
 * Disruptive Exp Lab
 */

public class VRConsole : MonoBehaviour
{

    GameObject canvasGameObj;
    Canvas canvas;
    public static Text text;

    static ArrayList messages = new ArrayList();
    public static int maxMessages = 3;

    public int broadcastPort = 9999;
    IPEndPoint remoteEndPoint;
    UdpClient client = null;

    public bool UIconsole;
    public bool BroadCast;

    void OnEnable()
    {
        if (BroadCast)
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, broadcastPort);
            client = new UdpClient();
        }
        Application.logMessageReceived += HandlelogMessageReceived;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandlelogMessageReceived;
        if (client != null)
            client.Close();
        remoteEndPoint = null;
    }

    private void Awake()
    {
        if (UIconsole)
        {
            canvasGameObj = new GameObject(name: "Canvas Console");
            canvas = canvasGameObj.AddComponent<Canvas>();

            canvas.renderMode = RenderMode.WorldSpace;
            CanvasScaler canvasScaler = canvasGameObj.AddComponent<CanvasScaler>();
            canvasScaler.scaleFactor = 1f;
            canvasScaler.dynamicPixelsPerUnit = 15;
            canvasScaler.referencePixelsPerUnit = 1;


            var image = new GameObject(name: "image").AddComponent<Image>();
            var color = Color.black;
            color.a = 0.4f;
            image.material = new Material(Shader.Find("GUI/Text Shader"));
            image.material.SetOverrideTag("Queue", "Overlay");
            image.material.color = color;

            text = new GameObject(name: "text").AddComponent<Text>();
            text.text = "Test test >";
            text.material.SetOverrideTag("Queue", "Overlay");
            text.color = new Color(241, 241, 241);

            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.font = ArialFont;
            text.fontSize = 5;
            text.material = ArialFont.material;

            text.transform.parent = image.transform.parent = canvasGameObj.transform;

            var anchor = GameObject.Find("CenterEyeAnchor");
            canvasGameObj.transform.SetParent(anchor.transform);

            var rect = canvasGameObj.GetComponent<RectTransform>();
            canvasGameObj.transform.position = new Vector3(0, 0.4f, -0.4f);

            var w = 200;
            var h = 70;

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            canvasGameObj.transform.localScale = new Vector3(0.012f, 0.012f, 1);

            rect = text.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

            rect = image.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        }
    }

    public static void Log(string msg)
    {
        if (text == null) return;
        messages.Add(msg);
        int diff;
        if (messages.Count > maxMessages)
        {
            if (messages.Count <= 0)
            {
                diff = 0;
            }
            else
            {
                diff = messages.Count - maxMessages;
            }
            messages.RemoveRange(0, (int)diff);
        }

        text.text = "";
        int x = 0;
        while (x < messages.Count)
        {
            text.text += (string)messages[x];
            text.text += '\n';
            x += 1;
        }
    }

    void HandlelogMessageReceived(string condition, string stackTrace, LogType type)
    {

        string s = stackTrace.Replace("\n", "\n  ");
        string msg = string.Format("[{0}] {1}{2}",
                                   type.ToString().ToUpper(),
                                   condition,
                                   "\n    " + stackTrace.Replace("\n", "\n    "));
        Log(msg);
        byte[] data = Encoding.UTF8.GetBytes(msg);
        if (client != null)
            client.Send(data, data.Length, remoteEndPoint);
    }
}
