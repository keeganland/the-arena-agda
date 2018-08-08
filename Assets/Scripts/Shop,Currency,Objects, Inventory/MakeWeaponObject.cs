using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeWeaponObject 
{
    [MenuItem("Assets/Create/Weapon Object")]
    public static void CreateWeaponObject()
    {
        WeaponObject asset = ScriptableObject.CreateInstance<WeaponObject>();
        AssetDatabase.CreateAsset(asset, "Assets/NewWeaponObject.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
 