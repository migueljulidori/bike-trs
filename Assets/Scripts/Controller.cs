using UnityEngine;

public class Controller : MonoBehaviour
{
    public bool Heart1, Heart2, Heart3, NotActivated1, NotActivated2, NotActivated3;
    public int Erro1, Erro2, Erro3;
    public GameObject Indicator1, Indicator2, Indicator3, Animation1, Animation2, Animation3, Win;
    public ScoreText Score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Heart1 = true;
        Heart2 = true;
        Heart3 = true;
        NotActivated1 = true;
        NotActivated2 = true;
        NotActivated3 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Heart1 && NotActivated1)
        {
            Indicator1.SetActive(false);
            Animation1.SetActive(true);
            NotActivated1 = false;
        }
        if(!Heart2 && NotActivated2)
        {
            Indicator2.SetActive(false);
            Animation2.SetActive(true);
            NotActivated2 = false;
        }
        if(!Heart3 && NotActivated3)
        {
            Indicator3.SetActive(false);
            Animation3.SetActive(true);
            NotActivated3 = false;
        }

        if(Erro3 != 0)
        {
            Win.SetActive(true);
            Score.Change();
        }
    }
}
