using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float frameRate = 0.1f; // Tempo entre cada frame

    private int currentFrame;
    public int Atual, MaxTime;
    private float timer;
    public GameObject Faixa;

    void Start()
    {
        timer = 0f;
    }

    public void Check()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;

            if (Atual >= MaxTime)
            {
                Faixa.SetActive(false);
            }
            else
            {
                Check();
            }

            Atual += 1;
        }
    }
}
