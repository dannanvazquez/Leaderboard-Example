using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class SceneAttribute : PropertyAttribute {
    [SerializeField] public string scenePath;
}