using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor (typeof (NoiseGenerator))]
public class NoiseGeneratorGUI : Editor {

	public override void OnInspectorGUI(){
		NoiseGenerator NoiseGen = (NoiseGenerator)target;

		if (DrawDefaultInspector ()) {
			if (NoiseGen.AutoUpdate) {
				NoiseGen.GenerateNoiseMap ();
			}
		}

		if(GUILayout.Button("Generate")){
			NoiseGen.GenerateNoiseMap ();
		}	

	}
}
