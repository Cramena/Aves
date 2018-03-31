using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LensesManager))]
public class LensesEditor : Editor {

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		LensesManager lenses = (LensesManager)target;
		if (GUILayout.Button ("Reset post process values")) {
			lenses.InitializeSettings ();
		}
	}
}
