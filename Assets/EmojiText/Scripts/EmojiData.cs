using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
public class EmojiEditor
{
    [MenuItem("GameObject/UI/EmojiText",priority = 0)]
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
        rt.sizeDelta = new Vector3(100f,100f);
        EditorGUIUtility.PingObject(go);
    }
}
#endif

[System.Serializable]
public struct EmojiSprites
{
    public string key;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "EmojiData",menuName = "CreateEmojiData",order = 1)]
public class EmojiData : ScriptableObject
{
    public List<EmojiSprites> datas = new List<EmojiSprites>();
}

