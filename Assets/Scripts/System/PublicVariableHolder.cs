using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PublicVariableHolder : MonoBehaviour {

    [Header("NeverUnload/PlayerUI : UISpellSwap")]
    public Text _BigTextHP; //Goes with "BigTextHP" (neverUnload)
    public Text _SmallTextHP; //Goes with "SmallTextHP" (neverUnload)

    public Image _BigSpellQ; //Goes with "BigSpellQIcon" (neverUnload)
    public Image _BigSpellW; //Goes with "BigSpellWIcon" (neverUnload)
    public Image _SmallSpellQ; //Goes with "SmallSpellQIcon" (neverUnload)
    public Image _SmallSpellW; //Goes with "SmallSpellWIcon" (neverUnload)

    public Sprite _HealImage; //Goes with "HealIcon" (assets)
    public Sprite _ExplosionImage; //Goes with "Explosion Spell" (assets)
    public Sprite _ShieldImage; //Goes with "Shield Spell Icon" (assets)
    public Sprite _StunImage; //Goes with "Stun_Enemy_Hero_Spell_img" (assets)

    public Image _BigPicture; //Goes with "BigPictureImage" (neverUnload)
    public Image _SmallPicture; //Goes with "SmallPictureImage" (neverUnload)

    public Sprite _BoySprite; //Goes with "SF_People1_1" (assets)
    public Sprite _GirlSprite; //Goes with "SF_People1_52" (assets)

    public Slider _SmallSlider; //Goes with "SliderSmall" (neverUnload)
    public Slider _BigSlider; //Goes with "SliderBig" (neverUnload)

    public HealthUI _GirlHealthUI; //Goes with "Girl" (neverUnload)
    public HealthUI _BoyHealthUI; //Goes with "Boy" (neverUnload)

    public SpellCommand _GirlSpellCommandScript; //Goes with "Girl" (neverUnload)
    public SpellCommand _SpellCommand; //Goes with "Girl" (neverUnload)

    public Text _BigQCooldowntext; //Goes with "CooldownbigQtext" (neverUnload)
    public Text _BigWCooldowntext; //Goes with "CooldownbigWtext" (neverUnload)
    public Text _SmallQCooldowntext; //Goes with  "CooldownsmallQtext" (neverUnload)
    public Text _SmallWCooldowntext; //Goes with "CooldownsmallWtext" (neverUnload)

    public Image _CooldownBigQImage; //Goes with "cooldownQimage" (neverUnload)
    public Image _CooldownBigWImage; //Goes with "cooldownWimage" (neverUnload)
    public Image _CooldownSmallQImage; //Goes with "cooldownQimagesmall" (neverUnload)
    public Image _CooldownSmallWImage; //Goes with "cooldownWimagesmall" (neverUnload)

    public BetterPlayer_Movement _GirlMovementScript; //Goes with "Girl" (neverUnload)
    public BetterPlayer_Movement _BoyMovementScript; //Goes with "Boy" (neverUnload)

    void NeverUnload()
    {

    }


	
	// Update is called once per frame
	void Update () {
		
	}
}
