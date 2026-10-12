using UnityEngine;
using UnityEngine.UI;

public class Animation : MonoBehaviour
{
    public Sprite[] frames;        // Lista de sprites da animação
    public float frameRate = 0.1f; // Tempo entre cada frame

    private Image imageComponent;
    private int currentFrame;
    public int Atual;
    private float timer;
    public GameObject Eu;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        currentFrame = 0;
        timer = 0f;

        if (frames.Length > 0)
            imageComponent.sprite = frames[0];
    }

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            imageComponent.sprite = frames[currentFrame];
            Eu.SetActive(false);

            if(Atual >= frames.Length)
            {
                
            }
            else
            {
                Atual += 1;
            }
        }
    }
}
