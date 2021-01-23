using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    public GameObject prefab;

    List<GameObject> overlays;

    public UnityEngine.UI.Text FilePathUI;

    static public Initializer Instance;

    static string configFilePath = ".\\Config\\displays.json";

    static string developOverlay = @"{""name"":""DevelopOverlay"",""position"":{""x"":0.6,""y"":0,""z"":-0.5},""rotation"":{""x"":-90,""y"":0,""z"":180},""resolution"":{""x"":300,""y"":200},""width"":0.3}";
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            overlays = new List<GameObject>();
        }

        Reload();
    }
    static void DevelopOverlayInitialized(object sender, System.EventArgs e)
    {
        ((Vuplex.WebView.CanvasWebViewPrefab)sender).WebView.LoadHtml(@"
		<!DOCTYPE html>
		<html>
		<head>
		<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
		<meta name=""transparent"" content=""true"">
		<style>
		html {
			width: 300px;
			height: 200px;
		}
		* {
			margin: 0;
			padding: 0;
		}
		</style>
		</head>
		<body>
			<p>大場レイ 開発版 起動中</p>
		</body>
		</html>
		");
    }

    void CreateDevelopOverlay()
    {
        var web = CreateOverlay(OverlayConfig.CreateFromJSON(developOverlay)).GetComponent<Vuplex.WebView.CanvasWebViewPrefab>();
        web.InitialUrl = null;
        web.Initialized += DevelopOverlayInitialized;
    }

    public void Reload()
    {
        Debug.Log("Start Reloading");
        foreach (var o in overlays) Destroy(o);
        overlays.Clear();

        FilePathUI.text = configFilePath;

        string config = System.IO.File.ReadAllText(configFilePath);
        foreach (var d in OverlayConfig.CreateArrayFromJSON(config)) CreateOverlay(d);

        CreateDevelopOverlay();

        Debug.Log("Reloaded");
    }

    public void Load(string path)
    {
        Debug.Log("Start Load file: " + path);
        configFilePath = path;
        Reload();
        Debug.Log("Reloaded");
    }

    GameObject CreateOverlay(OverlayConfig config)
    {
        GameObject overlay = Instantiate(prefab, Vector3.zero, Quaternion.identity, null);
        overlays.Add(overlay);

        overlay.GetComponent<InitializeRenderTexture>().SetRenderSize(config.resolution.x, config.resolution.y);

        EasyOpenVROverlayForUnity o = overlay.GetComponent<EasyOpenVROverlayForUnity>();
        o.Position = config.position;
        o.Rotation = config.rotation;

        o.width = config.width;

        o.OverlayFriendlyName = config.name;
        o.OverlayKeyName = config.id;

        if (config.device == null || config.device == "None")
        {
            o.DeviceTracking = false;
        }
        else
        {
            o.DeviceTracking = true;
            switch (config.device)
            {
                case "HMD":
                    o.changeToHMD();
                    break;
                case "Left":
                    o.changeToLeftController();
                    break;
                case "Right":
                    o.changeToRightController();
                    break;
                default:
                    o.DeviceSerialNumber = config.device;
                    break;

            }
        }

        o.MirrorX = OverlayConfig.MirrorX;
        o.MirrorY = OverlayConfig.MirrorY;

        Vuplex.WebView.CanvasWebViewPrefab web = overlay.GetComponent<Vuplex.WebView.CanvasWebViewPrefab>();
        if (config.url.StartsWith("."))
        {
            config.url = System.IO.Path.GetFullPath(config.url);
        }
        web.InitialUrl = config.url;

        return overlay;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
