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

    public Image _BigPicture;
    public Image _SmallPicture;

    public Sprite _BoySprite;
    public Sprite _GirlSprite;

    public Slider _SmallSlider;
    public Slider _BigSlider;

    public HealthUI _GirlHealthUI;
    public HealthUI _BoyHealthUI;

    private bool m_isBoy;

    private void Update()
    {
        if (m_isBoy)
            BoySlider();
        else GirlSlider();
    }
    public void BoySpellActive()
    {
        m_isBoy = true;
        _BigSpellQ.sprite = _ShieldImage;
        _BigSpellW.sprite = _StunImage;
        _SmallSpellQ.sprite = _HealImage;
        _SmallSpellW.sprite = _ExplosionImage;

        _BigPicture.sprite = _BoySprite;
        _SmallPicture.sprite = _GirlSprite;
    }

    public void GirlActive()
    {
        m_isBoy = false;
        _BigSpellQ.sprite = _HealImage;
        _BigSpellW.sprite = _ExplosionImage;
        _SmallSpellQ.sprite = _ShieldImage;
        _SmallSpellW.sprite = _StunImage;

        _BigPicture.sprite = _GirlSprite;
        _SmallPicture.sprite = _BoySprite;

    }

    private void GirlSlider()
    {
        _BigSlider.value = (1.0f / _GirlHealthUI.m_maxHealth) * _GirlHealthUI.m_curHealth;
        _SmallSlider.value = (1.0f / _BoyHealthUI.m_maxHealth) * _BoyHealthUI.m_curHealth;
    }
    private void BoySlider()
    {
        _BigSlider.value = (1.0f / _BoyHealthUI.m_maxHealth) * _BoyHealthUI.m_curHealth;
        _SmallSlider.value = (1.0f / _GirlHealthUI.m_maxHealth) * _GirlHealthUI.m_curHealth;
    }
}
