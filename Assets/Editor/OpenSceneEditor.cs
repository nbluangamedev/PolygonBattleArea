using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class OpenSceneEditor : EditorWindow
{
    private const string MENUITEM_PATH = "NBLuanGD/OpenScene/";
    private static string _scenePath = "Assets/Scenes/{0}.unity";

    [MenuItem(MENUITEM_PATH + "Loading", false, 1)]
    public static void Menu()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        EditorSceneManager.OpenScene(string.Format(_scenePath, "Loading"), OpenSceneMode.Single);
    }

    [MenuItem(MENUITEM_PATH + "Home", false, 1)]
    public static void Home()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        EditorSceneManager.OpenScene(string.Format(_scenePath, "Home"), OpenSceneMode.Single);
    }

    [MenuItem(MENUITEM_PATH + "Medium", false, 1)]
    public static void Level1()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        EditorSceneManager.OpenScene(string.Format(_scenePath, "Medium"), OpenSceneMode.Single);
    }

    [MenuItem(MENUITEM_PATH + "Small", false, 1)]
    public static void Level2()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        EditorSceneManager.OpenScene(string.Format(_scenePath, "Small"), OpenSceneMode.Single);
    }

    [MenuItem(MENUITEM_PATH + "CharacterSelection", false, 1)]
    public static void CharacterSelection()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        EditorSceneManager.OpenScene(string.Format(_scenePath, "CharacterSelection"), OpenSceneMode.Single);
    }

    [MenuItem(MENUITEM_PATH + "Lobby", false, 1)]
    public static void LobbySelection()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        EditorSceneManager.OpenScene(string.Format("Assets/Scenes/Multiplayer/{0}.unity", "Lobby"), OpenSceneMode.Single);
    }

    [MenuItem(MENUITEM_PATH + "Multiplayer", false, 1)]
    public static void MultiplayerSelection()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        EditorSceneManager.OpenScene(string.Format("Assets/Scenes/Multiplayer/{0}.unity", "Multiplayer"), OpenSceneMode.Single);
    }
}
