using System;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

using AccountInfo;
using LobbyInformaion;

using GetSetAccountInfo;
using GetSetLobbyInformation;

using ConnectServer;
using ChangeSceneManager;

using MVP.CheckingIsPlaying.ModelIsPlayingCheck;

using MVP.InGamePlayer.PresenterPlayer;
using MVP.InGamePlayer.ViewPlayer;

using MVP.InGameEnemy.PresenterEnemyManager;

using MVP.InGameSkills.PresenterSkills;
using MVP.InGameSkills.ViewSkills;

public partial class GameManager : MonoBehaviour
{
    static GameManager gameManager;
    private GameObject player;
    private GameObject enemyManager;
    private GameObject skillsManager;
    private AccountData accountData;
    private ChangeScene changeScene;

    private PresenterPlayer presenterPlayer;
    private PresenterSkillsManager presenterSkillsManager;
    private PresenterEnemyManager presenterEnemyManager;

    private RequestUserData requestUserData;

    private void Awake()
    {
        requestUserData = new RequestUserData();
        changeScene = new ChangeScene();

        StartTime = Time.time;

        RecordTime = Time.time - StartTime;
        EarnedScore = 0;
        EarnedCoin = 0;

//        Initialize();

        // �Ͻ������� ��, ���� ����.
        PauseGame();

        InGameSetting();
        Debug.Log("recordNumber : " + recordNumber);

        PlayerSetting();

        EnemyManagerSetting();

        SkillsSetting();

        // Player ������ ������ �Ͻ������� �����Ѵ�.
        ResumeGame();
    }

/*    // "GameManager" ���� ������Ʈ�� �����ϴ��� Ȯ���ϰ�, ������ �����Ѵ�.
    // ���� Ŭ����(GameManager) Ŭ������ ������ �ϴ� �ʱ�ȭ �Լ��̴�.
    public void Initialize()
    {
        GameObject manager = GameObject.FindWithTag("GameManager");

        // "GameManager" ���� ������Ʈ�� �����ϴ��� Ȯ���ϰ�, ������ �����Ѵ�.
        if (manager == null)
        {
            manager = new GameObject();
            manager.name = "GameManager";
            manager.tag = "GameManager";
        }

        // "GameManager" ���� ������Ʈ�� GameManager.cs Ŭ������ �����ϰ� �ִ��� Ȯ���ϰ�, ������ ���Խ�Ų��.
        if (manager.GetComponent<GameManager>() == null)
        {
            manager.AddComponent<GameManager>();
        }

        // Destroy()�� �� ���������� �ϴ� �Լ��̴�.
        DontDestroyOnLoad(manager);

        // Ŭ���� ���� �����ν� �����ϴ� �κ��̴�.
        gameManager = manager.GetComponent<GameManager>();
    }*/

    public void InitializePanel()
    {
        //        if (parentPanel.childCount == 0) return;
        //          gameOverPanel
        //      usedSkillsInfoPanel


    }

    // ���� ����, GameManager�� ���� ���� ��û
    public void InGameSetting()
    {
        JObject responseData = JObject.Parse(requestUserData.RequestSettingGameData(CreateInGameSettingRequestData()));

        if(!(bool)responseData["success"])
        {
            Debug.Log("GameData Error");
            return;
        }

        this.recordNumber = (int)responseData["recordNumber"];
        new SerializationIsPlayingCheck().SetInGameRecord(new IsPlayingCheck(true));
    }

    public string CreateInGameSettingRequestData()
    {
        // �ʿ��� ��ü �������� ������ JObject �ѹ��� ������.
        JObject requestData = new JObject();

        LobbyData01 lobbyData = new GetSetLobbyData().GetLobbyData();
        accountData = new GetSetAccountData().GetAccountDataInAccountDataType();

        JObject tempJObject01 = new JObject();
        JObject tempJObject02 = new JObject();
        JArray tempJArray = new JArray();

        requestData["AccountData"] = (JObject)JToken.FromObject(accountData);
        requestData["selecedCharacterNumber"] = lobbyData.SelecedCharacterNumber;

        tempJObject01["skillNumber"] = lobbyData.GetCharacterSkill(0);
        tempJArray.Add(tempJObject01);
        tempJObject02["skillNumber"] = lobbyData.GetCharacterSkill(1);
        tempJArray.Add(tempJObject02);

        requestData["characterSkill"] = tempJArray;

        requestData["selecedSkills"] = (JArray)JToken.FromObject(lobbyData.GetSelectedSkills());

        return JsonConvert.SerializeObject(requestData, Formatting.Indented);
    }

    // �÷��̾� ����, �÷��̾ ���� ���� ��û
    public void PlayerSetting()
    {
        JObject responseData = JObject.Parse(requestUserData.RequestCharacterData(CreatePlayerSettingRequestData()));

        if (!(bool)responseData["success"])
        {
            Debug.Log("GameData Error");
            return;
        }

        characterNumber = Convert.ToString(responseData["characterInfo"]["characterNumber"]);
        string prefabPath = "Prefab/InGame/Player/Player" + characterNumber;

        player = Instantiate(Resources.Load<GameObject>(prefabPath));
        player.name = "Player";
        player.tag = "Player";

        presenterPlayer = new PresenterPlayer(player, (JObject)responseData["characterInfo"]);
    }

    public string CreatePlayerSettingRequestData()
    {
        JObject requestData = new JObject();

        requestData["userID"] = accountData.UserID;
        requestData["userPW"] = accountData.UserMACAddress;
        requestData["recordNumber"] = recordNumber;

        return JsonConvert.SerializeObject(requestData, Formatting.Indented);
    }

    public void EnemyManagerSetting()
    {
        // JArray�� �޾ƿ� ������ ���� �ִ´�. ( request �����ʹ� ����. )
        // ���Ŀ� MapNumber�� �ѱ�ٸ�, Map���� �ٸ� Enemy�� ������ �� �ִ�.
        // �̰������� �ʿ� ������ Enemy ������ ����Ѵ�.
        JObject responseData = JObject.Parse(requestUserData.RequestEnemyData());

        if (!(bool)responseData["success"])
        {
            Debug.Log("GameData Error");
            return;
        }

        enemyManager = new GameObject("EnemyManager");
        enemyManager.tag = "EnemyManager";
        enemyManager.AddComponent<PresenterEnemyManager>().SettingEnemyManager(responseData);

    }

    public void SkillsSetting()
    {
        JObject responseData = JObject.Parse(requestUserData.RequestSkillsData(CreatePlayerSettingRequestData()));

        Debug.Log(((JArray)responseData["skillsData"]).Count);

        if (!(bool)responseData["success"])
        {
            Debug.Log("GameData Error");
            return;
        }

        skillsManager = new GameObject("SkillsManager");
        skillsManager.tag = "SkillsManager";
        skillsManager.AddComponent<ViewSkillsManager>();

        presenterSkillsManager = new PresenterSkillsManager(skillsManager, (JArray)responseData["skillsData"]);
    }

    public void MapSetting()
    {

    }
}

// �̰��� UI���õ� ������ ����Ѵ�.
partial class GameManager
{
    // serializeField�� ã�� ������, setActive�� �Ǳ� ���� �ش� Ŭ������ findwithtag�� ã�� �� ����̴�.
    // ���� ã�� �� �ִ� ����� �ϵ��ڵ������� �˾Ƴ��� �ʹ�.
    [SerializeField]
    private RectTransform gameOverPanel;
    [SerializeField]
    public RectTransform usedSkillsInfoPanel;
    [SerializeField]
    public RectTransform playerInfoPanel;
    [SerializeField]
    public RectTransform levelUPPanel;
    [SerializeField]
    public RectTransform pausePanel;
    [SerializeField]
    public Transform mapParent;
    [SerializeField]
    public AudioMixer audioMixer;
    [SerializeField]
    public Slider BGMSlider;
    [SerializeField]
    public Slider EffectSlider;

    private TextMeshProUGUI timeText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI coinText;

    [SerializeField]
    private TextMeshProUGUI resultTimeText;
    [SerializeField]
    private TextMeshProUGUI resultScoreText;
    [SerializeField]
    private TextMeshProUGUI resultCoinText;

    // Map ��ġ ������ ���� �ڵ�.
    private Transform[] mapTransform = new Transform[9];
    private float cameraWidth = 8.888f;
    private float cameraHeight = 5.0f;
    private int xCount = 0;
    private int yCount = 0;

    // �Ʒ� 4�� �޴����� ������ ���� ����.
    private int recordNumber;
    private float startTime;
    private float recordTime;
    private int earnedScore;
    private int earnedCoin;
    private string characterNumber;

    private bool pauseStatus = false;

    public int RecordNumber
    {
        get { return recordNumber; }
        set { recordNumber = value; }
    }
    public float StartTime
    {
        get { return startTime; }
        set { startTime = value; }
    }
    public float RecordTime
    {
        get { return recordTime; }
        set { recordTime = value; }
    }
    public int EarnedScore
    {
        get { return earnedScore; }
        set { earnedScore = value; }
    }
    public int EarnedCoin
    {
        get { return earnedCoin; }
        set { earnedCoin = value; }
    }

    private void Start()
    {
        audioMixer.SetFloat("BGM", -30f);

        scoreText = GameObject.FindWithTag("InGameScoreText").GetComponent<TextMeshProUGUI>();
        coinText = GameObject.FindWithTag("InGameCoinText").GetComponent<TextMeshProUGUI>();
        timeText = GameObject.FindWithTag("InGameTimeText").GetComponent<TextMeshProUGUI>();

        for(int i = 0; i< mapParent.childCount; i++)
        {
            mapTransform[i] = mapParent.GetChild(i).GetComponent<Transform>();
        }

        UpdateMapPosition();
        // ���� ���� (GameOverPanel������) ���� ��ų ������ �����. 
        DestroySkillsInfoInGameOverPanel();
    }

    private void Update()
    {
        timeText.text = ConvertTimeToString();
        scoreText.text = Convert.ToString(EarnedScore);
        coinText.text = Convert.ToString(EarnedCoin);

        xCount = (int)(player.transform.position.x / cameraWidth);
        yCount = (int)(player.transform.position.y / cameraHeight);
        UpdateMapPosition();

        if (Input.GetKeyDown(KeyCode.P) && !pauseStatus)
        {
            pausePanel.gameObject.SetActive(true);
            PauseGame();
            pauseStatus = true;
        }
        else if(Input.GetKeyDown(KeyCode.P) && pauseStatus)
        {
            pausePanel.gameObject.SetActive(false);
            ResumeGame();
            pauseStatus = false;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    // player ���� ��, GC ȣ��.
    public void PlayerDstroy()
    {
        presenterPlayer = null;
        GC.Collect();
    }

    // bullet ���� ��, GC ȣ��.
    public void DestroyBullet(System.Object bullet)
    {
        bullet = null;
        GC.Collect();
    }

    // Player�� ��ġ�� ���� 8������ ���� �� ��ġ�ϴ� ��� (but, �ش� ���Ӱ� ���� ���� �����ص� ��� ���� �ڵ常 �����ϴ�.)
    public void UpdateMapPosition()
    {
        mapTransform[0].position = new Vector3(cameraWidth * (xCount - 1), cameraHeight * (yCount + 1), 0);
        mapTransform[1].position = new Vector3(cameraWidth * (xCount), cameraHeight * (yCount + 1), 0);
        mapTransform[2].position = new Vector3(cameraWidth * (xCount + 1), cameraHeight * (yCount + 1), 0);
        mapTransform[3].position = new Vector3(cameraWidth * (xCount - 1), cameraHeight * (yCount), 0);
        mapTransform[4].position = new Vector3(cameraWidth * (xCount), cameraHeight * (yCount), 0);
        mapTransform[5].position = new Vector3(cameraWidth * (xCount + 1), cameraHeight * (yCount), 0);
        mapTransform[6].position = new Vector3(cameraWidth * (xCount - 1), cameraHeight * (yCount - 1), 0);
        mapTransform[7].position = new Vector3(cameraWidth * (xCount), cameraHeight * (yCount - 1), 0);
        mapTransform[8].position = new Vector3(cameraWidth * (xCount + 1), cameraHeight * (yCount - 1), 0);
    }




    // ----- ���� UI ���� ���

    // ���� �߾� �ð� ����
    public string ConvertTimeToString()
    {
        RecordTime = Time.time - StartTime;

//        Debug.Log("RecordTime = Time.time - StartTime : " + RecordTime + " = " + Time.time + " - " + StartTime);

        int time = (int)RecordTime;
        string min = Convert.ToString((int)(time / 60));
        string sec = Convert.ToString((time - time / 60 * 60) );

        return min + " : " + sec;
    }

    // Player ��� ��, UI ���
    public void DisplayGameOverPanel()
    {
        DestroySkillsInfoInGameOverPanel();
        gameOverPanel.gameObject.SetActive(true);

        resultTimeText.text = timeText.text;
        resultScoreText.text = scoreText.text;
        resultCoinText.text = coinText.text;

        string playerImage_path = "Image/InGame/Player/Player" + characterNumber;
        gameOverPanel.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(playerImage_path);
        skillsManager.GetComponent<ViewSkillsManager>().WriteModelPlayerInfoToGameOverPanel(gameOverPanel.GetChild(0).GetChild(1).GetChild(1));
        skillsManager.GetComponent<ViewSkillsManager>().WriteModelSkillsInfo(gameOverPanel.GetChild(0).GetChild(2));

        // ContentSizeFitter�� ���Ե� Layout�� ���� ���� �������� ���� ��, ��ġ ������ �߻��Ѵ�.
        // �̶�, Layout�� ������Ʈ�ϸ鼭, �ش� GameObject�� �ִ�, ContentSizeFitter�� �� �����ϸ� ������ �ذ�ȴ�.
        Canvas.ForceUpdateCanvases();
        gameOverPanel.GetChild(0).GetChild(2).GetComponent<ContentSizeFitter>().enabled = false;
        gameOverPanel.GetChild(0).GetChild(2).GetComponent<ContentSizeFitter>().enabled = true;
    }

    // GameOver ��ο��� ��ų������ ������Ų��.
    public void DestroySkillsInfoInGameOverPanel()
    {
        if (gameOverPanel.GetChild(0).GetChild(2).childCount == 0) return;

        for (int i = 0; i < gameOverPanel.GetChild(0).GetChild(2).childCount; i++)
        {
            Destroy(gameOverPanel.GetChild(0).GetChild(2).GetChild(i).gameObject);
        }

        gameOverPanel.GetChild(0).GetChild(2).DetachChildren();
    }

    // Enemy�� �׾��� ��, score�� coin ����.
    public void UpdateGameDataInEnemyDie(int score, int coin)
    {
        earnedScore = earnedScore + score;
        earnedCoin = earnedCoin + coin;
    }

    // �Ͻ� �������¿��� �������� ���ư��� ��ư Ŭ�� �̺�Ʈ
    public void OnClickedBuutonBackToGame()
    {
        pausePanel.gameObject.SetActive(false);
        ResumeGame();
        pauseStatus = false;
    }

    // �Ͻ� �������¿��� �κ�� ���ư��� ��ư Ŭ�� �̺�Ʈ
    public void OnClickedBuutonBackToLobby()
    {
        pausePanel.gameObject.SetActive(false);
        ResumeGame();
        pauseStatus = false;
        StopAllCoroutines();

        changeScene.SceneLoader("GameLobbyScene");
    }

    // �� ���� �Ҹ� ���� ���
    public void AudioControl(string audioGroup)
    {
        if(audioGroup == "BGM") audioMixer.SetFloat(audioGroup, BGMSlider.value);
        if (audioGroup == "Effect") audioMixer.SetFloat(audioGroup, EffectSlider.value);
    }



    // ��ư Ŭ����, ������ ������ ������ ������ ��.
    // �ϼ�
    public string GatherInGameInformationForRequestingServer()
    {
        string temp01;

        JObject requestData = new JObject();

        requestData["recordNumber"] = RecordNumber;
        requestData["recordTime"] = (int)RecordTime;
        requestData["earnedScore"] = EarnedScore;
        requestData["earnedCoin"] = EarnedCoin;

        temp01 = gameOverPanel.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite.name;
        requestData["characterNumber"] = temp01.Replace("Character", String.Empty);

        requestData["level"] = new JObject(
            new JProperty("playerLevel", gameOverPanel.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text),
            new JProperty("damageLevel", gameOverPanel.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text),
            new JProperty("bulletCountLevel", gameOverPanel.GetChild(0).GetChild(1).GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text),
            new JProperty("hitCountLevel", gameOverPanel.GetChild(0).GetChild(1).GetChild(1).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text),
            new JProperty("holdingTimeLevel", gameOverPanel.GetChild(0).GetChild(1).GetChild(1).GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text),
            new JProperty("spawnTimeLevel", gameOverPanel.GetChild(0).GetChild(1).GetChild(1).GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text)
        );

        JArray jArrayTemp01 = new JArray();

        for(int i =0; i < gameOverPanel.GetChild(0).GetChild(2).childCount; i++)
        {
            JObject jObjectTemp01 = new JObject();

            temp01 = gameOverPanel.GetChild(0).GetChild(2).GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite.name;
            string[] temp02 = temp01.Split('_');

            jObjectTemp01["skillType"] = temp02[0];
            jObjectTemp01["compoundSkillNumber"] = temp02[1];

            JArray jArrayTemp02 = new JArray();

            for(int j = 0; gameOverPanel.GetChild(0).GetChild(2).GetChild(i).GetChild(1).GetChild(j).GetChild(0).GetComponent<Image>().sprite != null; j++)
            {
                if (j == 9) break;

                Debug.Log("check : " + i + "_" + j);

                temp01 = gameOverPanel.GetChild(0).GetChild(2).GetChild(i).GetChild(1).GetChild(j).GetChild(0).GetComponent<Image>().sprite.name;

                jArrayTemp02.Add(new JObject(new JProperty("materialSkillNumber", temp01.Replace(temp02[0] + '_', String.Empty))));
            }

            jObjectTemp01["materialSkill"] = jArrayTemp02;

            jArrayTemp01.Add(jObjectTemp01);
        }

        requestData["skillData"] = jArrayTemp01;

        Debug.Log("requestData : " + requestData);

        return JsonConvert.SerializeObject(requestData, Formatting.Indented);
    }

    // Lobby�� ���ư��� ��ư.
    // �Ͻ������� �����ϴ��� ����.
    public void OnClickedGoToGameLobbyButton()
    {
        gameOverPanel.gameObject.SetActive(false);
        StopAllCoroutines();

        JObject responseData = JObject.Parse(requestUserData.RequestGameResult(GatherInGameInformationForRequestingServer()));

        Debug.Log("success : " + (bool)responseData["success"]);

        changeScene.SceneLoader("GameLobbyScene");
    }

}