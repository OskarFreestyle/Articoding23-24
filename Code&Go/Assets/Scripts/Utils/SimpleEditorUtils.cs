#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SimpleEditorUtils {

	// Click Ctrl+0 to go to the loading scene and then play
	[MenuItem("Utils/Play-Unplay From Loading Scene %0")]
	public static void PlayFromLoadingScene() {

		// Check if the application is on playing
		if (EditorApplication.isPlaying == true) {

			// Set the Application on not playing
			EditorApplication.isPlaying = false;

			// Create a delay callback that will be called after the exit
			EditorApplication.delayCall += () =>
			{
				// Restore the last scene
				EditorSceneManager.OpenScene(PlayerPrefs.GetString("LastScenePath"));
			};

			return;
		}

		// Save the current scene
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

		// Save the current scene path
		PlayerPrefs.SetString("LastScenePath", SceneManager.GetActiveScene().path);
		PlayerPrefs.Save();

		// Change to the Loading Scene
		EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);

		// Set the Application on playing
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
#endif