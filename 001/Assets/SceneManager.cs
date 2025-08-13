using UnityEngine;
using UnityEngine.InputSystem;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        UnitySceneManager.LoadScene($"Scene {level}");
    }
}
