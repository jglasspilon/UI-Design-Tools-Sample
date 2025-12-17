using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataBinderLink))]
public class DataRegistrarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Empty to prevent editing in the editor - controlled in attached Data Binder inspector
    }
}
