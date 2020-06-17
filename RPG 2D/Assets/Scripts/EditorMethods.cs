using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMethods : MonoBehaviour
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("EditorMethods/ClearSaveData")]
    private static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
#endif
}
