using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    private GameObject m_StartUI;
    private GameObject m_GameUI;
    private GameObject m_Left;
    private GameObject m_Right;
    private UILabel m_Score_Label;
    private UILabel m_Gem_Label;
    private UILabel m_GameScoreLabel;
    private UILabel m_GameGemLabel;
    private GameObject m_PlayButton;
    private PlayerController m_PlayerController;
    // Use this for initialization
    void Start () {
        m_StartUI = GameObject.Find("Start_UI");
        m_GameUI = GameObject.Find("Game_UI");
        m_Score_Label = GameObject.Find("Score_Label").GetComponent<UILabel>();
        m_Gem_Label = GameObject.Find("Gem_Label").GetComponent<UILabel>();
        m_GameScoreLabel = GameObject.Find("GameScoreLabel").GetComponent<UILabel>();
        m_GameGemLabel = GameObject.Find("GameGemLabel").GetComponent<UILabel>();
        m_PlayButton = GameObject.Find("play_btn");
        m_PlayerController = GameObject.Find("cube_books").GetComponent<PlayerController>();
        m_Left = GameObject.Find("Left");
        UIEventListener.Get(m_Left).onClick = Left;
        m_Right = GameObject.Find("Right");
        UIEventListener.Get(m_Right).onClick = Right;

        Init();

        UIEventListener.Get(m_PlayButton).onClick = PlayButtonClick;



        m_GameUI.SetActive(false);
    }

    private void PlayButtonClick(GameObject go)
    {
        m_StartUI.SetActive(false);
        m_GameUI.SetActive(true);

        m_PlayerController.StartGame();
    }

    private void Left(GameObject go)
    {
        m_PlayerController.Left();
    }

    private void Right(GameObject go)
    {
        m_PlayerController.Right();
    }

    private void Init()
    {
        m_Score_Label.text = PlayerPrefs.GetInt("count",0) + "";
        m_Gem_Label.text = PlayerPrefs.GetInt("gem", 0) + "/100";
        m_GameScoreLabel.text = "0";
        m_GameGemLabel.text = PlayerPrefs.GetInt("gem", 0) + "/100";
    }

    public void UpdateData(int Score,int Gemcount)
    {
        m_Gem_Label.text = Gemcount + "/100";
        m_GameScoreLabel.text = Score.ToString();
        m_GameGemLabel.text = Gemcount + "/100";
    }

    public void ResetUI()
    {
        m_StartUI.SetActive(true);
        m_GameUI.SetActive(false);
        m_GameScoreLabel.text = "0";
    }

    // Update is called once per frame
    void Update () {
	
	}

}
