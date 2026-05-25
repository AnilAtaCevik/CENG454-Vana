using UnityEngine;

//static wrapper around PlayerPrefabs for typed saved/load operations
public static class SaveSystem
{
    // Int
    public static void SaveInt(string key, int value) => PlayerPrefs.SetInt(key, value);
    public static int LoadInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);

    // Float
    public static void SaveFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
    public static float LoadFloat(string key, float defaultValue = 0f) => PlayerPrefs.GetFloat(key, defaultValue);

    // Bool
    public static void SaveBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);
    public static bool LoadBool(string key, bool defaultValue = false) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;

    // String
    public static void SaveString(string key, string value) => PlayerPrefs.SetString(key, value);
    public static string LoadString(string key, string defaultValue = "") => PlayerPrefs.GetString(key, defaultValue);

    // Special cases
    // Saves the last played level index so Play button can resume from it
    public static void SaveLastLevel(int index) => SaveInt("lastLevel", index);
    public static int LoadLastLevel() => LoadInt("lastLevel", 0);

    public static void SaveMusicEnabled(bool enabled) => SaveBool("musicEnabled", enabled);
    public static bool LoadMusicEnabled() => LoadBool("musicEnabled", true);

    public static void DeleteAll() => PlayerPrefs.DeleteAll();
}