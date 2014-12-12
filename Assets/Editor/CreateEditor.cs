using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

[CustomEditor(typeof(Grid))]
public class CreateEditor : Editor {
	Grid grid;

	void OnEnable() {
		grid = (Grid)target;
	}

	[MenuItem("Assets/Create/TileSet")]
	static void CreateTileSet() {
		var asset = ScriptableObject.CreateInstance<TileSet>();
		var path = AssetDatabase.GetAssetPath(Selection.activeObject);
		if (string.IsNullOrEmpty(path)) {
			path = "Assets/";
		} else if (Path.GetExtension(path) != "") {
			path = path.Replace(Path.GetFileName(path), "");
		} else {
			path += "/";
		}

		var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "TileSet.asset");
		AssetDatabase.CreateAsset(asset, assetPathAndName);
		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
		asset.hideFlags = HideFlags.DontSave;
	}

	public override void OnInspectorGUI () {
		//base.OnInspectorGUI();
		grid.width = createSlider("Width", grid.width);
		grid.height = createSlider("Height", grid.height);
		if (GUILayout.Button("Open Grid Window")) {
			GridWindow window = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow));
			window.init();
		}

		// Tile Prefab
		EditorGUI.BeginChangeCheck();
		var newTilePrefab = (Transform)EditorGUILayout.ObjectField("Tile Prefab", grid.tilePrefab, typeof(Transform), false);
		if (EditorGUI.EndChangeCheck()) {
			grid.tilePrefab = newTilePrefab;
			Undo.RecordObject(target, "Grid Changed");
		}

		// Tile Map
		EditorGUI.BeginChangeCheck();
		var newTileSet = (TileSet) EditorGUILayout.ObjectField("Tileset", grid.tileSet, typeof(TileSet), false);
		if (EditorGUI.EndChangeCheck()) {
			grid.tileSet = newTileSet;
			Undo.RecordObject(target, "Grid Changed");
		}
	}

	private float createSlider(string labelName, float sliderPosition) {
		GUILayout.BeginHorizontal();
		GUILayout.Label ("Grid" + labelName);
		sliderPosition = EditorGUILayout.Slider(sliderPosition, 1f, 100f, null);
		GUILayout.EndHorizontal();
		return sliderPosition;
	}
}
