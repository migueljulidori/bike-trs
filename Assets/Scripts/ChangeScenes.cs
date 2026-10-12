using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public string Name;

    public void LoadSceneByName()
    {
        SceneManager.LoadScene(Name);
    }
}
