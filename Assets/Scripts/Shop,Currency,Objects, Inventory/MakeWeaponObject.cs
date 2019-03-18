using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR)

public class MakeWeaponObject 
{
    [MenuItem("Assets/Create/Weapon Object")]
    public static void CreateWeaponObject()
    {
        WeaponObject asset = ScriptableObject.CreateInstance<WeaponObject>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/WeaponsObjects/WeaponObjectAssets/You Just Created this weapon object. Give it a name!.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}

#endif
