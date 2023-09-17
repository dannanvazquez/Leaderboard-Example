using UnityEngine;

[CreateAssetMenu(fileName = "ProjectData", menuName = "ScriptableObjects/ProjectSettingsScriptableObject", order = 0)]
public class ProjectSettingsScriptableObject : ScriptableObject {
    [Header("Leaderboard Settings")]
    [Tooltip("The ID of the leaderboard being used.")]
    public string leaderboardId;
}