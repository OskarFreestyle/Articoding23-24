using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class SimpleEditorUtils {
	// Click Ctrl+0 to go to the loading scene and then play
	[MenuItem("Utils/Play-Unplay From Loading Scene %0")]
	public static void PlayFromLoadingScene() {
		if (EditorApplication.isPlaying == true) {
			EditorApplication.isPlaying = false;
			return;
		}
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path); // EditorBuildSettings.scenes[0].path
		EditorApplication.isPlaying = true;
	}

	[MenuItem("Utils/Open BoardCreationScene")]
	public static void OpenBoardCreationScene() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/BoardCreation.unity");
	}

	[MenuItem("Utils/Open EndScene")]
	public static void OpenEndScene() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/EndScene.unity");
	}

	[MenuItem("Utils/Open LevelScene")]
	public static void OpenLevelScene() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/LevelScene.unity");
	}

	[MenuItem("Utils/Open LoadingScene")]
	public static void OpenLoadingScene() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/LoadingScene.unity");
	}

	[MenuItem("Utils/Open MenuScene")]
	public static void OpenMenuScene() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/MenuScene.unity");
	}
}
