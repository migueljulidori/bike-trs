using UnityEngine;

public class CursorBackOn : MonoBehaviour
{
    public GameObject Main, Title, LevelScreen, City, Trevo, Candy;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
