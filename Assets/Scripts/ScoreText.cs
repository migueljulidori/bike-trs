using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public int FullScore, Penalty1, Penalty2, ArrowsInt, HelmetInt, LightsInt;
    public bool LightsBool, ArrowsBool, HelmetBool;
    public string TextSubstitute, HelmetWarning, LightsWarning, ArrowsWarning, WrongSideWarning, TraficWarning, CrossWarning, ObstacleWarning, TurnWarning;
    public Text Score, Penalty1Text, Penalty2Text, Penalty3Text, ArrowsText, HelmetText, LightsText;
    public Image Penalty1Image, Penalty2Image, Penalty3Image, ArrowsImage, HelmetImage, LightsImage;
    public Sprite ArrowsSprite, LightsSprite, HelmetSprite, WrongSideSprite, TraficSprite, CrossSprite, ObstacleSprite, TurnSprite;
    public Controller cont;
    public PlayerMovement Player;

    public void Start()
    {
        LightsBool = true;
        ArrowsBool = true;
        HelmetBool = true;
        Player.Activated = false;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            LoadSceneByName("Main Menu");
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Change()
    {
        FullScore = 10000;

        if(cont.Erro1 == 2)
        {
            Penalty1Text.text = WrongSideWarning;
            Penalty1Image.sprite = WrongSideSprite;
            Penalty1 = 1000;
        }
        else if(cont.Erro1 == 1)
        {
            Penalty1Text.text = TraficWarning;
            Penalty1Image.sprite = TraficSprite;
            Penalty1 = 1000;
        }
        else if (cont.Erro1 == 3)
        {
            Penalty1Text.text = CrossWarning;
            Penalty1Image.sprite = CrossSprite;
        }
        else if (cont.Erro1 == 4)
        {
            Penalty1Text.text = ObstacleWarning;
            Penalty1Image.sprite = ObstacleSprite;
        }
        else if (cont.Erro1 == 5)
        {
            Penalty1Text.text = TurnWarning;
            Penalty1Image.sprite = TurnSprite;
        }
        else
        {
            Penalty1Text.text = null;
            Penalty1Image.enabled = false;
        }

        if (cont.Erro2 == 2)
        {
            Penalty2Text.text = WrongSideWarning;
            Penalty2Image.sprite = WrongSideSprite;
            Penalty2 = 1000;
        }
        else if (cont.Erro2 == 1)
        {
            Penalty2Text.text = TraficWarning;
            Penalty2Image.sprite = TraficSprite;
            Penalty2 = 1000;
        }
        else if (cont.Erro2 == 3)
        {
            Penalty2Text.text = CrossWarning;
            Penalty2Image.sprite = CrossSprite;
        }
        else if (cont.Erro2 == 4)
        {
            Penalty2Text.text = ObstacleWarning;
            Penalty2Image.sprite = ObstacleSprite;
        }
        else if (cont.Erro2 == 5)
        {
            Penalty2Text.text = TurnWarning;
            Penalty2Image.sprite = TurnSprite;
        }
        else
        {
            Penalty2Text.text = null;
            Penalty2Image.enabled = false;
        }

        if (!ArrowsBool)
        {
            ArrowsInt = 1000;
            ArrowsImage.enabled = true;
            ArrowsText.text = ArrowsWarning;
            ArrowsImage.sprite = ArrowsSprite;
        }
        else
        {
            ArrowsText.text = null;
            ArrowsImage.enabled = false;
            ArrowsInt = 0;
        }
        
        if(!HelmetBool)
        {
            HelmetInt = 1000;
            HelmetImage.enabled = true;
            HelmetText.text = HelmetWarning;
            HelmetImage.sprite = HelmetSprite;
        }
        else
        {
            HelmetText.text = null;
            HelmetImage.enabled = false;
            HelmetInt = 0;
        }

        if(!LightsBool)
        {
            LightsInt = 1000;
            LightsImage.enabled = true;
            LightsText.text = LightsWarning;
            LightsImage.sprite = LightsSprite;
        }
        else
        {
            LightsText.text = null;
            LightsImage.enabled = false;
            LightsInt = 0;
        }

        FullScore = FullScore - Penalty1 - Penalty2 - ArrowsInt - HelmetInt - LightsInt;

        if(cont.Erro3 != 0)
        {
            FullScore = 0;
        }
        if (cont.Erro3 == 2)
        {
            Penalty3Text.text = WrongSideWarning;
            Penalty3Image.sprite = WrongSideSprite;
        }
        else if (cont.Erro3 == 1)
        {
            Penalty3Text.text = TraficWarning;
            Penalty3Image.sprite = TraficSprite;
        }
        else if (cont.Erro3 == 3)
        {
            Penalty3Text.text = CrossWarning;
            Penalty3Image.sprite = CrossSprite;
        }
        else if (cont.Erro3 == 4)
        {
            Penalty3Text.text = ObstacleWarning;
            Penalty3Image.sprite = ObstacleSprite;
        }
        else if (cont.Erro1 == 5)
        {
            Penalty1Text.text = TurnWarning;
            Penalty1Image.sprite = TurnSprite;
        }
        else
        {
            Penalty3Text.text = null;
            Penalty3Image.enabled = false;
        }

            TextSubstitute = FullScore.ToString();
        Score.text = TextSubstitute;
    }
}
