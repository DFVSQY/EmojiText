using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EmojiText : Text
{
    private struct EmojiStruct
    {
        public int posIndex;
        public string des;

        public EmojiStruct(int posIndex, string des)
        {
            this.posIndex = posIndex;
            this.des = des;
        }
    }

    private static char emSpace = '\u2001';
    private List<UIVertex> verts = new List<UIVertex>();
    private List<EmojiStruct> emojis = new List<EmojiStruct>();

    private static EmojiData _asset;
    private static EmojiData asset
    {
        get
        {
            if (_asset == null)
            {
                _asset = Resources.Load<EmojiData>("EmojiData/EmojiData");
                if (_asset == null)
                {
                    Debug.LogError("_asset is null");
                }
            }
            return _asset;
        }
    }

    private static Dictionary<string, Sprite> _data;
    private static Dictionary<string, Sprite> data
    {
        get
        {
            if (_data == null)
            {
                _data = new Dictionary<string, Sprite>();
                List<EmojiSprites> es = asset.datas;
                foreach (var e in es)
                {
                    if (!_data.ContainsKey(e.key))
                    {
                        _data.Add(e.key, e.sprite);
                    }
                    else
                    {
                        Debug.LogError("emoji repeat,key:" + e.key);
                    }
                }
            }
            return _data;
        }
    }

    public override string text
    {
        get
        {
            return base.text;
        }

        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = ParserText(value);
            }
            base.text = value;

            // sometimes when set text, OnPopulateMesh won't be called, 
            // use this statement to force OnPopulateMesh to be called 
            SetVerticesDirty();

            StartCoroutine(ShowEmoji());
        }
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);
        if (emojis.Count > 0) toFill.GetUIVertexStream(verts);
    }

    private IEnumerator ShowEmoji()
    {
        yield return new WaitUntil(() =>
        {
            return cachedTextGenerator.vertexCount > 0;
        });

        int count = emojis.Count;
        if (count > 0 && verts.Count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                int index = emojis[i].posIndex;
                Image image = null;
                if (i >= transform.childCount)                                  // if emoji gameobject is not enough
                {
                    GameObject go = new GameObject("emoji");
                    image = go.AddComponent<Image>();
                    go.transform.SetParent(transform);
                    go.transform.localScale = Vector3.one;
                }
                else
                {
                    image = transform.GetChild(i).GetComponent<Image>();
                }
                RectTransform rt = image.rectTransform;
                rt.gameObject.SetActive(true);
                rt.sizeDelta = new Vector2(fontSize, fontSize);
                float x = verts[index * 6].position.x + fontSize / 2;
                float y = verts[index * 6].position.y + fontSize / 4;
                rt.localPosition = new Vector3(x, y, 0f);
                image.sprite = data[emojis[i].des];
            }
            for (int i = count; i < transform.childCount; i++)
            {
                Transform ch = transform.GetChild(i);
                ch.gameObject.SetActive(false);
            }
        }
        else if (count <= 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform ch = transform.GetChild(i);
                ch.gameObject.SetActive(false);
            }
        }
        verts.Clear();
    }

    private string ParserText(string content)
    {
        emojis.Clear();
        StringBuilder sb = new StringBuilder();
        int i = 0;
        int length = content.Length;
        while (i < length)
        {
            char c = content[i];
            int end = i + 3;                            //[微笑]...  
            if (end >= length || !c.Equals('['))
            {
                sb.Append(c);
                i++;
            }
            else
            {
                string s = content.Substring(i, 4);
                if (data.ContainsKey(s))
                {
                    sb.Append(emSpace);
                    emojis.Add(new EmojiStruct(sb.Length - 1, s));
                    i += 4;
                }
                else
                {
                    sb.Append(c);
                    i++;
                }
            }
        }
        return sb.ToString();
    }
}
