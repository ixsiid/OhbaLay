using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class OverlayConfig
{
    public string name;
    public Vector3 position;
    public Vector2Int resolution;
    public Vector3 rotation;
    public float width;
    public string device;

    public string url;
    public string id;

    static public bool MirrorX;
    static public bool MirrorY;

    private OverlayConfig()
    {
        name = id = System.Guid.NewGuid().ToString();
        device = null; // No tracking, install on room

        position = new Vector3(0, 0, 0.5f);
        rotation = Vector3.zero;
        resolution = new Vector2Int(1200, 1000);

        width = 1.0f;

        url = "https://www.google.com";
    }

    public static OverlayConfig CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<OverlayConfig>(jsonString);
    }

    public static List<OverlayConfig> CreateArrayFromJSON(string jsonString)
    {
        OverlayConfigArray configs = JsonUtility.FromJson<OverlayConfigArray>(jsonString);
        MirrorX = configs.MirrorX;
        MirrorY = configs.MirrorY;
        return configs.Overlays;
    }

    private class OverlayConfigArray
    {
        public bool MirrorX;
        public bool MirrorY;
        public List<OverlayConfig> Overlays;

        private OverlayConfigArray() {
            MirrorX = false;
            MirrorY = false;
            Overlays = new List<OverlayConfig>();
        }
    }
}