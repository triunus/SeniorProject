using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MVP.InGameSkills.EnumData;
using MVP.InGameSkills.ModelSkills;
using MVP.InGameSkills.ModelBullet;

using MVP.InGameSkills.PresenterSkills;
using MVP.InGameSkills.ViewBullet;

namespace MVP.InGameSkills.ViewSkills
{
    public partial class ViewSkillsManager : IViewSkills
    {
        public void SetSkillsManager(PresenterSkillsManager presenterSkillsManager, GameObject skillsManager)
        {
            this.presenterSkillsManager = presenterSkillsManager;
            this.skillsManager = skillsManager;
        }

        public List<ModelSkill> ModelSkills
        {
            get { return modelSkills; }
            set { modelSkills = value; }
        }

        public Dictionary<string, ModelBullet.ModelBullet> ModelBullets
        {
            get { return modelBullets; }
            set { modelBullets = value; }
        }

        public ModelStatusLevel ModelStatusLevel
        {
            get { return modelStatusLevel; }
            set { modelStatusLevel = value; }
        }

        public void StartCreateBullet()
        {
            StopCoroutine("CreateBullet");

            // 각 Bullet 모델마다 코 루틴 실행.
            foreach (ModelBullet.ModelBullet value in modelBullets.Values)
            {
                StartCoroutine("CreateBullet", value);
            }
        }
    }

    partial class ViewSkillsManager : MonoBehaviour
    {
        private PresenterSkillsManager presenterSkillsManager;

        private GameObject skillsManager;
        private RectTransform usedSkillsInfoPanel;
        // 버튼을 포함한 Panel
        private RectTransform playerInfoPanel;
        private RectTransform levelUPPanel;

        private List<ModelSkill> modelSkills = new List<ModelSkill>();
        private Dictionary<string, ModelBullet.ModelBullet> modelBullets;
        private ModelStatusLevel modelStatusLevel = new ModelStatusLevel();

        private bool usedSkillsInfoViewActive = false;
        private bool playerInfoViewActive = false;

        private void Start()
        {
            // 출력하고자 하는 화면 : 0
            usedSkillsInfoPanel = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().usedSkillsInfoPanel;
            // 버튼 연결 : 1 - 0 - 0
            usedSkillsInfoPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(OnViewSkillsInfoButtonEvent);

            // 출력하고자 하는 화면 : 1
            playerInfoPanel = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().playerInfoPanel;
            // 버튼 연결 : 0 - 0OnViewPlayerInfoButtonEvent
            playerInfoPanel.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(OnViewPlayerInfoButtonEvent);

            levelUPPanel = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().levelUPPanel;
            WriteModelStatusLevelInLevelUpUI();
            WriteModelSkillsInfo(usedSkillsInfoPanel.GetChild(0));
        }

        private void Update()
        {
            WriteModelPlayerInfo();
        }

        // modelSkills의 길이만큼 서로 다른 코루틴 실행.
        private IEnumerator CreateBullet(ModelBullet.ModelBullet modelbullet)
        {
            while (true)
            {
                for(int i = 0; i < modelbullet.SkillBulletCount; i++)
                {
                    // 각 Bullet마다 각 각의 Bullet 정보를 갖게 한다.
                    ModelBullet.ModelBullet passBulletData = modelbullet.DeepCopy();

                    // 타입과 속성 별로 다른 프리팹이 생성된다.
                    // 이미지만 인식하는 것이 아닌, 프리팹을 인식하는 것은 애니메이션 때문이다.
                    // "Prefab/InGame/Bullet/Projectile/Projectile00" 이런 느낌
                    string prefabPath = "Prefab/InGame/Bullet/" + passBulletData.SkillType + "/" + passBulletData.SkillType + passBulletData.SkillAttribute;

                    // 해당 프리팹 생성
                    // 이곳(각각의 코루틴이 실행되는)에서 프리팹을 생성하는 이유는 Bullet이 개별적인 객체를 이루기 위해서이다.
                    GameObject bullet = GameObject.Instantiate(Resources.Load<GameObject>(prefabPath));
                    bullet.tag = "InGameBulletProjectile";
                    bullet.GetComponent<SpriteRenderer>().sortingOrder = 3;

                    // 몇번 째로 생성되는 bullet인지 나타낸다.
                    passBulletData.Index = i;

                    new PresenterBullet.PresenterBullet(bullet, passBulletData);
                }


                yield return new WaitForSeconds(modelbullet.SkillSpawnTime);
            }
        }

        // 레벨 업 시, UI 넣을 데이터 가져옴.
        private void WriteModelStatusLevelInLevelUpUI()
        {
            JArray statusData = JArray.Parse(File.ReadAllText("./Assets/GameData/ModelStatusInLevelIncrease/ModelStatusInLevelIncrease.json"));

            for (int i = 0; i < statusData.Count; i++)
            {
                StatusNumber(i, (JObject)statusData[i]);
            }
        }

        // 레벨 업 UI가 갖을 데이터 출력, Button 이벤트 연결.
        private void StatusNumber(int index, JObject statusData)
        {
            // main/skillPanel/Box(i) : 1-0- i
            // Image : 0-0, Name : 1-0, Detail : 2-0

            //            LevelUPPanel.GetChild(1).GetChild(0).GetChild(index).GetChild(0).GetChild(0).GetComponent<Image>().sprite = ;
            levelUPPanel.GetChild(1).GetChild(0).GetChild(index).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)statusData["StatusName"];
            levelUPPanel.GetChild(1).GetChild(0).GetChild(index).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)statusData["StatusDetail"];

            // Button.onClick.AddListener(Events.UnityAction callbackfunction);
            // delegate void UnityAction();
            // delegate는 포인터와 비슷한 역할을 한다.
            // 따라서 사용하고 싶은 함수를 미리 정의한 후, delegate로 연결하여 매개변수가 존재하는 Button을 구현할 수 있다.
            levelUPPanel.GetChild(1).GetChild(0).GetChild(index).GetComponent<Button>().onClick.AddListener(delegate { OnLevelUpButtonEvent(index); });
        }

        // 클릭된 버튼의 index가 들어온다.
        // ModelStatusInLevelIncrease.json 안 데이터 순서 참조.
        private void OnLevelUpButtonEvent(int index)
        {
            ModelStatusLevel.Level++;

            switch (index)
            {
                case 0:
                    ModelStatusLevel.LevelDamage++;
                    break;
                case 1:
                    ModelStatusLevel.LevelBulletCount++;
                    break;
                case 2:
                    ModelStatusLevel.LevelBulletHitCount++;
                    break;
                case 3:
                    ModelStatusLevel.LevelBulletHoldingTime++;
                    break;
                case 4:
                    ModelStatusLevel.LevelBulletSpawnTime++;
                    break;
                default:
                    break;
            }

            presenterSkillsManager.UpdatePresenterSkills();
            presenterSkillsManager.CompoundSkills();
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().ResumeGame();
            levelUPPanel.gameObject.SetActive(false);
        }




        // 플레이어 레벨 출력.
        private void WriteModelPlayerInfo()
        {
            // 1 - index - 1 - 0
            playerInfoPanel.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.Level);
            playerInfoPanel.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelDamage);
            playerInfoPanel.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletCount);
            playerInfoPanel.GetChild(1).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletHitCount);
            playerInfoPanel.GetChild(1).GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletHoldingTime);
            playerInfoPanel.GetChild(1).GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletSpawnTime);
        }

        // GameOverPanel 창에 플레이어 레벨 출력
        public void WriteModelPlayerInfoToGameOverPanel(Transform playerLevelIngameOverPanel)
        {
            playerLevelIngameOverPanel.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.Level);
            playerLevelIngameOverPanel.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelDamage);
            playerLevelIngameOverPanel.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletCount);
            playerLevelIngameOverPanel.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletHitCount);
            playerLevelIngameOverPanel.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletHoldingTime);
            playerLevelIngameOverPanel.GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletSpawnTime);
        }

        // 우측 상단, 플레이어 Status UI 클릭 버튼.
        private void OnViewPlayerInfoButtonEvent()
        {
            if (!playerInfoViewActive)
            {
                playerInfoPanel.GetChild(1).gameObject.SetActive(!playerInfoViewActive);
                playerInfoViewActive = true;
            }
            else
            {
                playerInfoPanel.GetChild(1).gameObject.SetActive(!playerInfoViewActive);
                playerInfoViewActive = false;
            }
        }



        // 사용 스킬 정보 출력.
        public void WriteModelSkillsInfo(Transform positionForCreateSkillInfoPrefab)
        {
            Dictionary<string, int[]> displayUsedSkills = new Dictionary<string, int[]>();

            // SkillType의 개수만큼 프리팹 생성.
            for(int i = 0; i < modelBullets.Count; i++)
            {
                // 사용하는 스킬 정보의 큰 틀 생성.
                GameObject usedSkillsPanel = Instantiate(Resources.Load<GameObject>("Prefab/InGame/UI/UsedSkill/UsedSkillPanel"));
                usedSkillsPanel.transform.SetParent(positionForCreateSkillInfoPrefab);
                usedSkillsPanel.SetActive(true);

                displayUsedSkills.Add(modelBullets.Keys.ToList()[i], new int[2] { i, 0 } );

/*                Debug.Log("displayUsedSkills.Keys.ToList() : " + displayUsedSkills.Keys.ToList());
                Debug.Log("displayUsedSkills.Keys.ToList()[0] : " + displayUsedSkills.Keys.ToList()[i]);
                Debug.Log("displayUsedSkills[Projectile] : " + displayUsedSkills["Projectile"]);
                Debug.Log("displayUsedSkills[Projectile][0] : " + displayUsedSkills["Projectile"][0]);
                Debug.Log("displayUsedSkills[Projectile][1] : " + displayUsedSkills["Projectile"][1]);*/

                // 합성 스킬 이미지 지정.
                string skillImage_path = "SkillImage/" + displayUsedSkills.Keys.ToList()[i] + "/" + displayUsedSkills.Keys.ToList()[i] + "_" + modelBullets[modelBullets.Keys.ToList()[i]].SkillAttribute;
//                Debug.Log("skillImage_path : " + skillImage_path);

                usedSkillsPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(skillImage_path);
            }

            // 재료로 사용된 스킬 이미지 지정.
            for (int i = 0; i < modelSkills.Count; i++)
            {
                string skillImage_path = "SkillImage/" + modelSkills[i].SkillType + "/" + modelSkills[i].SkillType + "_" + (int)Enum.Parse(typeof(AttributeData), modelSkills[i].SkillAttribute);
//                Debug.Log("skillImage_path : " + skillImage_path);

                positionForCreateSkillInfoPrefab.GetChild(displayUsedSkills[modelSkills[i].SkillType][0]).GetChild(1).GetChild(displayUsedSkills[modelSkills[i].SkillType][1]).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(skillImage_path);
                // modelSkills.SkillType을 갖는 Dictionary의 인자를 변경한다.
                // 해당 인자의 이미지 변경.
                displayUsedSkills[modelSkills[i].SkillType][1]++;
            }
        }
        // 좌측 상단, 스킬 UI 클릭 버튼.
        private void OnViewSkillsInfoButtonEvent()
        {
            if (!usedSkillsInfoViewActive)
            {
                usedSkillsInfoPanel.GetChild(0).gameObject.SetActive(!usedSkillsInfoViewActive);
                usedSkillsInfoViewActive = true;
            }
            else
            {
                usedSkillsInfoPanel.GetChild(0).gameObject.SetActive(!usedSkillsInfoViewActive);
                usedSkillsInfoViewActive = false;
            }
        }

        // SkillLevelUp 시, Panel 출력.
        public void LevelUPEvent()
        {
            levelUPPanel.gameObject.SetActive(true);
        }
    }

}