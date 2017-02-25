using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
public class EmojiEditor
{
    [MenuItem("GameObject/UI/EmojiText", priority = 0)]
    public static void CreateEmojiText()
    {
        Transform par = null;
        if(Selection.gameObjects.Length > 0)
        {
            par = Selection.gameObjects[0].transform;
            if(AssetDatabase.Contains(par.gameObject))
            {
                par = null;
            }
        }
        GameObject go = new GameObject("EmojiText");
        go.transform.SetParent(par);
        go.AddComponent<EmojiText>();
        RectTransform rt = go.transform as RectTransform;
        rt.localScale = Vector3.one;
        rt.localPosition = Vector3.zero;
        rt.sizeDelta = new Vector3(100f, 100f);
        EditorGUIUtility.PingObject(go);
    }

    [MenuItem("EmojiHelper/FillEmojiData")]
    public static void FillEmojiData()
    {
        EmojiData ed = Resources.Load<EmojiData>("EmojiData/EmojiData");
        if(ed == null)
        {
            Debug.LogError("请首先在工程视图中的EmojiData文件夹下创建EmojiData文件,选中EmojiData文件夹右键点击Create/CreateEmojiData即可");
            return;
        }
        ed.datas.Clear();
        string[] files = Directory.GetFiles(Application.dataPath + "/EmojiText/Emojis", "*.png");
        foreach(string file in files)
        {
            string key = "[" + Path.GetFileNameWithoutExtension(file) + "]";
            string p = file.Replace(Application.dataPath, "Assets");
            Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(p);
            ed.datas.Add(new EmojiSprites() { key = key, sprite = s });
        }
        AssetDatabase.Refresh();
    }
}
#endif

[System.Serializable]
public struct EmojiSprites
{
    public string key;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "EmojiData", menuName = "CreateEmojiData", order = 1)]
public class EmojiData : ScriptableObject
{
    public List<EmojiSprites> datas = new List<EmojiSprites>();
}

