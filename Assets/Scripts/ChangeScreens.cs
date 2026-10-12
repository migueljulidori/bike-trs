using UnityEngine;

public class ChangeScreens : MonoBehaviour
{
    public GameObject screen1, screen2;

    public void Change()
    {
        screen1.SetActive(false);
        screen2.SetActive(true);
    }
}
