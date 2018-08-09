using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMerchand : ActivateTextAtLine {

    public override void ChangeText(TextAsset TextYes, TextAsset TextNo)
    {
        if (TextYes)
            YesText = TextYes;
        if (TextNo)
            NoText = TextNo;
    }
}
