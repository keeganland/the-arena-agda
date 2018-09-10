using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpellSwap : MonoBehaviour {

    public PublicVariableHolderneverUnload _PublicVariableHolder;

    private Text _BigTextHP;
    private Text _SmallTextHP;

    private Image _BigSpellQ;
    private Image _BigSpellW;
    private Image _SmallSpellQ;
    private Image _SmallSpellW;

    private Sprite _HealImage;
    private Sprite _ExplosionImage;
    private Sprite _ShieldImage;
    private Sprite _StunImage;

    private Image _BigPicture;
    private Image _SmallPicture;

    private Sprite _BoySprite;
    private Sprite _GirlSprite;

    private Slider _SmallSlider;
    private Slider _BigSlider;

    private HealthController _GirlHealthUI;
    private HealthController _BoyHealthUI;

    private SpellCommand _GirlSpellCommandScript;
    private SpellCommand _BoySpellCommandScript;

    private Text _BigQCooldowntext;
    private Text _BigWCooldowntext;
    private Text _SmallQCooldowntext;
    private Text _SmallWCooldowntext;

    private Image _CooldownBigQImage;
    private Image _CooldownBigWImage;
    private Image _CooldownSmallQImage;
    private Image _CooldownSmallWImage;

    private BetterPlayer_Movement _GirlMovementScript;
    private BetterPlayer_Movement _BoyMovementScript;

    private GameObject EnemyUI;
    private Text EnemyNameUI;
    private Text CurrentEnemyHPUI;
    private Slider SliderEnemyHPUI;
    public GameObject currentEnemy;
    private bool iscurrentEnemy;

    private int m_bigQcooldowntext;
    private int m_bigWcooldowntext;
    private int m_smallQcooldowntext;
    private int m_smallWcooldowntext;

    private GameObject BigSpellW;
    private GameObject BigSpellQ;
    private GameObject SmallSpellQ;
    private GameObject SmallSpellW;

    private bool m_isBoy;

    private bool m_showGirlSpellsOnly;
    private bool m_spellHidden;

    private void Start()
    {
        _BigTextHP = _PublicVariableHolder._BigTextHP;
        _SmallTextHP = _PublicVariableHolder._SmallTextHP;

        _BigSpellQ = _PublicVariableHolder._BigSpellQ;
        _BigSpellW = _PublicVariableHolder._BigSpellW;
        _SmallSpellQ = _PublicVariableHolder._SmallSpellQ;
        _SmallSpellW = _PublicVariableHolder._SmallSpellW;

        _HealImage = _PublicVariableHolder._HealImage;
        _ExplosionImage = _PublicVariableHolder._ExplosionImage;
        _ShieldImage = _PublicVariableHolder._ShieldImage;
        _StunImage = _PublicVariableHolder._StunImage;

        _BigPicture = _PublicVariableHolder._BigPicture;
        _SmallPicture = _PublicVariableHolder._SmallPicture;

        _BoySprite = _PublicVariableHolder._BoySprite;
        _GirlSprite = _PublicVariableHolder._GirlSprite;

        _SmallSlider = _PublicVariableHolder._SmallSlider;
        _BigSlider = _PublicVariableHolder._BigSlider;

        _GirlHealthUI = _PublicVariableHolder._GirlHealthUI;
        _BoyHealthUI = _PublicVariableHolder._BoyHealthUI;

        _GirlSpellCommandScript = _PublicVariableHolder._GirlSpellCommandScript;
        _BoySpellCommandScript = _PublicVariableHolder._BoySpellCommandScript;

        _BigQCooldowntext = _PublicVariableHolder._BigQCooldowntext;
        _BigWCooldowntext = _PublicVariableHolder._BigWCooldowntext;
        _SmallQCooldowntext = _PublicVariableHolder._SmallQCooldowntext;
        _SmallWCooldowntext = _PublicVariableHolder._SmallWCooldowntext;

        _CooldownBigQImage = _PublicVariableHolder._CooldownBigQImage;
        _CooldownBigWImage = _PublicVariableHolder._CooldownBigWImage;
        _CooldownSmallQImage = _PublicVariableHolder._CooldownSmallQImage;
        _CooldownSmallWImage = _PublicVariableHolder._CooldownSmallWImage;

        _GirlMovementScript = _PublicVariableHolder._GirlMovementScript;
        _BoyMovementScript = _PublicVariableHolder._BoyMovementScript;

        EnemyUI = _PublicVariableHolder.EnemyUI;
        EnemyNameUI = _PublicVariableHolder.EnemyNameUI;
        CurrentEnemyHPUI = _PublicVariableHolder.CurrentEnemyHPUI;
        SliderEnemyHPUI = _PublicVariableHolder.SliderEnemyHPUI;

        BigSpellQ = _PublicVariableHolder.BigSpellQ;
        BigSpellW = _PublicVariableHolder.BigSpellW;
        SmallSpellQ = _PublicVariableHolder.SmallSpellQ;
        SmallSpellW = _PublicVariableHolder.SmallSpellW;

}
    private void Update()
    {
        if (m_isBoy)
            BoySlider();
        else GirlSlider();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Ground")
                {
                    DisableEnemyUI();
                }
            }
        }
        EnableEnemyUI();

        if (m_showGirlSpellsOnly)
        {
            if (m_isBoy)
            {
                BigSpellW.SetActive(false);
                BigSpellQ.SetActive(false);
                SmallSpellQ.SetActive(true);
                SmallSpellW.SetActive(true);
            }
            else
            {
                BigSpellW.SetActive(true);
                BigSpellQ.SetActive(true);
                SmallSpellQ.SetActive(false);
                SmallSpellW.SetActive(false);
            }
        }
    }

    private void EnableEnemyUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Enemy")
                {
                    EnemyUI.SetActive(true);
                    currentEnemy = hit.collider.gameObject;
                    iscurrentEnemy = true;
                    EnemyNameUI.text = currentEnemy.GetComponent<HealthController>().GameObjectName;
                }
            }
        }

        if(currentEnemy)
        {
            CurrentEnemyHPUI.text = currentEnemy.GetComponent<HealthController>().currentHealth.ToString() +  " / "  + currentEnemy.GetComponent<HealthController>().totalHealth.ToString();
            SliderEnemyHPUI.value = (1.0f / (float) currentEnemy.GetComponent<HealthController>().totalHealth) *(float) currentEnemy.GetComponent<HealthController>().currentHealth;
        }
        if (!currentEnemy)
        {
            DisableEnemyUI();
        }
    }


    private void DisableEnemyUI()
    {
        iscurrentEnemy = false;
        EnemyUI.SetActive(false);            
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
        _BigSlider.value = (1.0f / _GirlHealthUI.totalHealth) * _GirlHealthUI.currentHealth;
        _SmallSlider.value = (1.0f / _BoyHealthUI.totalHealth) * _BoyHealthUI.currentHealth;

        _BigTextHP.text = "HP : " + _GirlHealthUI.currentHealth.ToString() + " /  " + _GirlHealthUI.totalHealth.ToString();
        _SmallTextHP.text = "HP : " + _BoyHealthUI.currentHealth.ToString() + " /  " + _BoyHealthUI.totalHealth.ToString();

        m_smallQcooldowntext = (int) _BoySpellCommandScript._ShieldUITimer;
        m_smallWcooldowntext = (int)_BoySpellCommandScript._StunUITimer;
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

        _CooldownSmallQImage.fillAmount = _BoySpellCommandScript._ShieldUITimer/_BoySpellCommandScript._ShieldCooldown;
        _CooldownSmallWImage.fillAmount = _BoySpellCommandScript._StunUITimer / _BoySpellCommandScript._StunCooldown;
        _CooldownBigQImage.fillAmount = _GirlSpellCommandScript._HealUITimer / _GirlSpellCommandScript._HealCooldown;
        _CooldownBigWImage.fillAmount = _GirlSpellCommandScript._AOEUITimer / _GirlSpellCommandScript._AOECooldown;

    }
    private void BoySlider()
    {
        _BigSlider.value = (1.0f / _BoyHealthUI.totalHealth) * _BoyHealthUI.currentHealth;
        _SmallSlider.value = (1.0f / _GirlHealthUI.totalHealth) * _GirlHealthUI.currentHealth;

        _SmallTextHP.text = "HP : " + _GirlHealthUI.currentHealth.ToString() + " /  " + _GirlHealthUI.totalHealth.ToString();
        _BigTextHP.text = "HP : " + _BoyHealthUI.currentHealth.ToString() + " /  " + _BoyHealthUI.totalHealth.ToString();

        m_smallQcooldowntext = (int)_GirlSpellCommandScript._HealUITimer;
        m_smallWcooldowntext = (int)_GirlSpellCommandScript._AOEUITimer;
        m_bigQcooldowntext = (int)_BoySpellCommandScript._ShieldUITimer;
        m_bigWcooldowntext = (int)_BoySpellCommandScript._StunUITimer;

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

        _CooldownBigQImage.fillAmount = _BoySpellCommandScript._ShieldUITimer / _GirlSpellCommandScript._ShieldCooldown;
        _CooldownBigWImage.fillAmount = _BoySpellCommandScript._StunUITimer / _GirlSpellCommandScript._StunCooldown;
        _CooldownSmallQImage.fillAmount = _GirlSpellCommandScript._HealUITimer / _GirlSpellCommandScript._HealCooldown;
        _CooldownSmallWImage.fillAmount = _GirlSpellCommandScript._AOEUITimer / _GirlSpellCommandScript._AOECooldown;
    }

    public void BigQisCastedWithMouse()
    {
        if (!_PublicVariableHolder.StopAllActions)
        {
            if (m_isBoy)
            {
                _GirlSpellCommandScript.CastSpellQBoy(true);
                _BoySpellCommandScript.CastSpellQBoy(true);
            }
            else 
            { 
                _GirlSpellCommandScript.CastSpellQGirl(true);
                _BoySpellCommandScript.CastSpellQGirl(true);
            }
        }
    }

    public void BigWisCastedWithMouse()
    {
        if (!_PublicVariableHolder.StopAllActions)
        {
            if (m_isBoy)
            {
                _GirlSpellCommandScript.CastSpellWBoy(true);
                _BoySpellCommandScript.CastSpellWBoy(true);
            }
            else
            {
                _GirlSpellCommandScript.CastSpellWGirl(true);
                _BoySpellCommandScript.CastSpellWGirl(true);
            }
        }
    }

    public void SmallQisCastedWithMouse()
    {
        if (!_PublicVariableHolder.StopAllActions)
        {
            if (m_isBoy)
            {
                _GirlSpellCommandScript.CastSpellQGirl(false);
                _BoySpellCommandScript.CastSpellQGirl(false);
            }
            else
            {
                _GirlSpellCommandScript.CastSpellQBoy(false);
                _BoySpellCommandScript.CastSpellQBoy(false);
            }
        }
    }

    public void SmallWisCastedWithMouse()
    {
        if (!_PublicVariableHolder.StopAllActions)
        {
            if (m_isBoy)
            {
                _GirlSpellCommandScript.CastSpellWGirl(false);
                _BoySpellCommandScript.CastSpellWGirl(false);
            }
            else
            {
                _GirlSpellCommandScript.CastSpellWBoy(false);
                _BoySpellCommandScript.CastSpellWBoy(false);
            }
        }
    }

    public void PressSmallPicture()
    {
        if (!_PublicVariableHolder.StopAllActions)
        {
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

    public void HiddeSpells()
    {
        BigSpellW.SetActive(false);
        BigSpellQ.SetActive(false);
        SmallSpellQ.SetActive(false);
        SmallSpellW.SetActive(false);

        m_spellHidden = true;
    }

    public void ShowSpells()
    {
        BigSpellW.SetActive(true);
        BigSpellQ.SetActive(true);
        SmallSpellQ.SetActive(true);
        SmallSpellW.SetActive(true);

        m_spellHidden = false;
        m_showGirlSpellsOnly = false;
    }

    public void ShowGirlSpellsOnly()
    {
        if (m_spellHidden == true)
        {
            m_showGirlSpellsOnly = true;
        }
    }
}
