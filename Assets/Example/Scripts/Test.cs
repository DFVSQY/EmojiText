using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private Text _text;
    private Text text
    {
        get
        {
            if (_text == null)
            {
                _text = transform.Find("EmojiText").GetComponent<Text>();
            }
            return _text;
        }
    }

    private IEnumerator Start()
    {
        WaitForSeconds ws = new WaitForSeconds(3f);

        text.text = "[大笑]哈哈[大哭]哇哇[礼物]蛋糕[玫瑰]";
        yield return ws;
        text.text = "[饥饿]This[无语]is[调皮]a[狂汗]EmojiText[鄙视]example[高兴]";
    }
}
