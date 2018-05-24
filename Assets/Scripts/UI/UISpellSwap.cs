using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpellSwap : MonoBehaviour {

    public Text _BigTextHP;
    public Text _SmallTextHP;

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

    public SpellCommand _GirlSpellCommandScript;
    public SpellCommand _SpellCommand;

    public Text _BigQCooldowntext;
    public Text _BigWCooldowntext;
    public Text _SmallQCooldowntext;
    public Text _SmallWCooldowntext;

    public Image _CooldownBigQImage;
    public Image _CooldownBigWImage;
    public Image _CooldownSmallQImage;
    public Image _CooldownSmallWImage;

    public BetterPlayer_Movement _GirlMovementScript;
    public BetterPlayer_Movement _BoyMovementScript;

    private int m_bigQcooldowntext;
    private int m_bigWcooldowntext;
    private int m_smallQcooldowntext;
    private int m_smallWcooldowntext;

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

        _BigTextHP.text = "HP : " + _GirlHealthUI.m_curHealth.ToString() + " /  " + _GirlHealthUI.m_maxHealth.ToString();
        _SmallTextHP.text = "HP : " + _BoyHealthUI.m_curHealth.ToString() + " /  " + _BoyHealthUI.m_maxHealth.ToString();

        m_smallQcooldowntext = (int) _GirlSpellCommandScript._ShieldUITimer;
        m_smallWcooldowntext = (int)_GirlSpellCommandScript._StunUITimer;
        m_bigQcooldowntext = (int)_GirlSpellCommandScript._HealUITimer;
        m_bigWcooldowntext = (int)_GirlSpellCommandScript._AOEUITimer;

        if (m_smallQcooldowntext == 0)
        {
            _SmallQCooldowntext.text = "";
        }
        else
            _SmallQCooldowntext.text = m_smallQcooldowntext.ToString();
        if (m_smallWcooldowntext == 0)
        {
            _SmallWCooldowntext.text = "";
        }
        else
            _SmallWCooldowntext.text = m_smallWcooldowntext.ToString();
        if (m_bigQcooldowntext == 0)
        {
            _BigQCooldowntext.text = "";
        }
        else
            _BigQCooldowntext.text = m_bigQcooldowntext.ToString();
        if (m_bigWcooldowntext == 0)
        {
            _BigWCooldowntext.text = "";
        }
        else
            _BigWCooldowntext.text = m_bigWcooldowntext.ToString();

        _CooldownSmallQImage.fillAmount = _GirlSpellCommandScript._ShieldUITimer/_GirlSpellCommandScript._ShieldCooldown;
        _CooldownSmallWImage.fillAmount = _GirlSpellCommandScript._StunUITimer / _GirlSpellCommandScript._StunCooldown;
        _CooldownBigQImage.fillAmount = _GirlSpellCommandScript._HealUITimer / _GirlSpellCommandScript._HealCooldown;
        _CooldownBigWImage.fillAmount = _GirlSpellCommandScript._AOEUITimer / _GirlSpellCommandScript._AOECooldown;

    }
    private void BoySlider()
    {
        _BigSlider.value = (1.0f / _BoyHealthUI.m_maxHealth) * _BoyHealthUI.m_curHealth;
        _SmallSlider.value = (1.0f / _GirlHealthUI.m_maxHealth) * _GirlHealthUI.m_curHealth;

        _SmallTextHP.text = "HP : " + _GirlHealthUI.m_curHealth.ToString() + " /  " + _GirlHealthUI.m_maxHealth.ToString();
        _BigTextHP.text = "HP : " + _BoyHealthUI.m_curHealth.ToString() + " /  " + _BoyHealthUI.m_maxHealth.ToString();

        m_smallQcooldowntext = (int)_GirlSpellCommandScript._HealUITimer;
        m_smallWcooldowntext = (int)_GirlSpellCommandScript._AOEUITimer;
        m_bigQcooldowntext = (int)_GirlSpellCommandScript._ShieldUITimer;
        m_bigWcooldowntext = (int)_GirlSpellCommandScript._StunUITimer;

        if (m_bigQcooldowntext == 0)
        {
            _BigQCooldowntext.text = "";
        }
        else
        {
            _BigQCooldowntext.text = m_bigQcooldowntext.ToString();
        }
        if (m_bigWcooldowntext == 0)
        {
            _BigWCooldowntext.text = "";
        }
        else
            _BigWCooldowntext.text = m_bigWcooldowntext.ToString();
        if (m_smallQcooldowntext == 0)
        {
            _SmallQCooldowntext.text = "";
        }
        else
            _SmallQCooldowntext.text = m_smallQcooldowntext.ToString();
        if (m_smallWcooldowntext == 0)
        {
            _SmallWCooldowntext.text = "";
        }
        else
            _SmallWCooldowntext.text = m_smallWcooldowntext.ToString();

        _CooldownBigQImage.fillAmount = _GirlSpellCommandScript._ShieldUITimer / _GirlSpellCommandScript._ShieldCooldown;
        _CooldownBigWImage.fillAmount = _GirlSpellCommandScript._StunUITimer / _GirlSpellCommandScript._StunCooldown;
        _CooldownSmallQImage.fillAmount = _GirlSpellCommandScript._HealUITimer / _GirlSpellCommandScript._HealCooldown;
        _CooldownSmallWImage.fillAmount = _GirlSpellCommandScript._AOEUITimer / _GirlSpellCommandScript._AOECooldown;
    }

    public void BigQisCastedWithMouse()
    {
      
        if (m_isBoy)
        {
            _SpellCommand.CastSpellQBoy();
        }
        else _SpellCommand.CastSpellQGirl();
    }

    public void BigWisCastedWithMouse()
    {
        if (m_isBoy)
        {
            _SpellCommand.CastSpellWBoy();
        }
        else _SpellCommand.CastSpellWGirl();
    }

    public void SmallQisCastedWithMouse()
    {

        if (m_isBoy)
        {
            _SpellCommand.CastSpellQGirl();
        }
        else _SpellCommand.CastSpellQBoy();
    }

    public void SmallWisCastedWithMouse()
    {
        if (m_isBoy)
        {
            _SpellCommand.CastSpellWGirl();
        }
        else _SpellCommand.CastSpellWBoy();
    }

    public void PressSmallPicture()
    {
        Debug.Log("here");
        if (m_isBoy)
        {
            GirlActive();
            _GirlMovementScript.SwapGirl();
            _BoyMovementScript.SwapGirl();
        }
        else
        {
            BoySpellActive();
            _BoyMovementScript.SwapBoy();
            _GirlMovementScript.SwapBoy();
        }
    }
}
