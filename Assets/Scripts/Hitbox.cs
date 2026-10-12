using UnityEngine;
using UnityEngine.SceneManagement;

public class Hitbox : MonoBehaviour
{
    public Controller cont;
    public GameObject Win, Banana, BadFaixa, BadTurn, GoodTurn;
    public ScoreText Score;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Sinal")
            {
               if (cont.Heart1)
               {
                    cont.Heart1 = false;
                    cont.Erro1 = 1;
               }
               else if (cont.Heart2)
               {
                    cont.Heart2 = false;
                    cont.Erro2 = 1;
               }
               else if (cont.Heart3)
               {
                    cont.Heart3 = false;
                    cont.Erro3 = 1;
               }
            }

            if (gameObject.tag == "Side")
            {
                if (cont.Heart1)
                {
                    cont.Heart1 = false;
                    cont.Erro1 = 2;
                }
                else if (cont.Heart2)
                {
                    cont.Heart2 = false;
                    cont.Erro2 = 2;
                }
                else if (cont.Heart3)
                {
                    cont.Heart3 = false;
                    cont.Erro3 = 2;
                }
            }

            if (gameObject.tag == "GoodCross")
            {
                //Faixa.Check();
                //gameObject.SetActive(false);
                Banana.SetActive(true);
            }

            if (gameObject.tag == "BadCross")
            {
                if (cont.Heart1)
                {
                    cont.Heart1 = false;
                    cont.Erro1 = 3;
                }
                else if (cont.Heart2)
                {
                    cont.Heart2 = false;
                    cont.Erro2 = 3;
                }
                else if (cont.Heart3)
                {
                    cont.Heart3 = false;
                    cont.Erro3 = 3;
                }
            }

            if (gameObject.tag == "Obstacle")
            {
                if (cont.Heart1)
                {
                    cont.Heart1 = false;
                    cont.Erro1 = 4;
                }
                else if (cont.Heart2)
                {
                    cont.Heart2 = false;
                    cont.Erro2 = 4;
                }
                else if (cont.Heart3)
                {
                    cont.Heart3 = false;
                    cont.Erro3 = 4;
                }
            }

            if (gameObject.tag == "GoodTurn")
            {
                BadTurn.SetActive(true);
                GoodTurn.SetActive(false);
            }

            if (gameObject.tag == "BadTurn")
            {
                if (cont.Heart1)
                {
                    cont.Heart1 = false;
                    cont.Erro1 = 5;
                }
                else if (cont.Heart2)
                {
                    cont.Heart2 = false;
                    cont.Erro2 = 5;
                }
                else if (cont.Heart3)
                {
                    cont.Heart3 = false;
                    cont.Erro3 = 5;
                }
            }
        }
        if (col.CompareTag("win"))
        {
            Win.SetActive(true);
            Score.Change();
        }

        if (col.gameObject.tag == "Banana")
        {
            //Banana.SetActive(false);
            BadFaixa.SetActive(false);
            Debug.Log("Work");
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
