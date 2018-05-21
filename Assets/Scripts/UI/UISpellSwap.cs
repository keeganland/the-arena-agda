using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpellSwap : MonoBehaviour {

    public Image _BigSpellQ;
    public Image _BigSpellW;
    public Image _SmallSpellQ;
    public Image _SmallSpellW;

    public Sprite _HealImage;
    public Sprite _ExplosionImage;
    public Sprite _ShieldImage;
    public Sprite _StunImage;

    public void BoySpellActive()
    {
        _BigSpellQ.sprite = _ShieldImage;
        _BigSpellW.sprite = _StunImage;
        _SmallSpellQ.sprite = _HealImage;
        _SmallSpellW.sprite = _ExplosionImage;      
    }

    public void GirlActive()
    {
        _BigSpellQ.sprite = _HealImage;
        _BigSpellW.sprite = _ExplosionImage;
        _SmallSpellQ.sprite = _ShieldImage;
        _SmallSpellW.sprite = _StunImage;
    }
}
