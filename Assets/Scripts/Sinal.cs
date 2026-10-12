using UnityEngine;
using UnityEngine.UI;

public class Sinal : MonoBehaviour
{
    public float frameRate = 0.1f; // Tempo entre cada frame

    private Image imageComponent;
    private int currentFrame;
    public int Atual, VermelhoAmarelo, AmareloVerde, VerdeVermelho;
    private float timer;
    public GameObject All, SinalVermelho, SinalAmarelo, SinalVerde, HitboxFalha;

    void Start()
    {
        All.SetActive(false);
        SinalVermelho.SetActive(true);
        imageComponent = GetComponent<Image>();
        currentFrame = 0;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;

            if(Atual == VermelhoAmarelo)
            {
                SinalVermelho.SetActive(false);
                SinalAmarelo.SetActive(true);
            }
            else if(Atual == AmareloVerde)
            {
                SinalAmarelo.SetActive(false);
                SinalVerde.SetActive(true);
                HitboxFalha.SetActive(false);
            }
            else if(Atual == VerdeVermelho)
            {
                Atual = -1;
                SinalVerde.SetActive(false);
                SinalVermelho.SetActive(true);
                HitboxFalha.SetActive(true);
            }
            Atual = Atual + 1;
        }
    }
}