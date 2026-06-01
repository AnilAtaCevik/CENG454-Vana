using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Game/Mission Data")]
public class MissionData : ScriptableObject
{
    public string missionName;
    public string description;
    public string sceneName;
    public Vector2 mapPosition;
    public int missionNumber;
}