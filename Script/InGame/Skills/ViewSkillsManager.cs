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

            // �� Bullet �𵨸��� �� ��ƾ ����.
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
        // ��ư�� ������ Panel
        private RectTransform playerInfoPanel;
        private RectTransform levelUPPanel;

        private List<ModelSkill> modelSkills = new List<ModelSkill>();
        private Dictionary<string, ModelBullet.ModelBullet> modelBullets;
        private ModelStatusLevel modelStatusLevel = new ModelStatusLevel();

        private bool usedSkillsInfoViewActive = false;
        private bool playerInfoViewActive = false;

        private void Start()
        {
            // ����ϰ��� �ϴ� ȭ�� : 0
            usedSkillsInfoPanel = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().usedSkillsInfoPanel;
            // ��ư ���� : 1 - 0 - 0
            usedSkillsInfoPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(OnViewSkillsInfoButtonEvent);

            // ����ϰ��� �ϴ� ȭ�� : 1
            playerInfoPanel = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().playerInfoPanel;
            // ��ư ���� : 0 - 0OnViewPlayerInfoButtonEvent
            playerInfoPanel.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(OnViewPlayerInfoButtonEvent);

            levelUPPanel = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().levelUPPanel;
            WriteModelStatusLevelInLevelUpUI();
            WriteModelSkillsInfo(usedSkillsInfoPanel.GetChild(0));
        }

        private void Update()
        {
            WriteModelPlayerInfo();
        }

        // modelSkills�� ���̸�ŭ ���� �ٸ� �ڷ�ƾ ����.
        private IEnumerator CreateBullet(ModelBullet.ModelBullet modelbullet)
        {
            while (true)
            {
                for(int i = 0; i < modelbullet.SkillBulletCount; i++)
                {
                    // �� Bullet���� �� ���� Bullet ������ ���� �Ѵ�.
                    ModelBullet.ModelBullet passBulletData = modelbullet.DeepCopy();

                    // Ÿ�԰� �Ӽ� ���� �ٸ� �������� �����ȴ�.
                    // �̹����� �ν��ϴ� ���� �ƴ�, �������� �ν��ϴ� ���� �ִϸ��̼� �����̴�.
                    // "Prefab/InGame/Bullet/Projectile/Projectile00" �̷� ����
                    string prefabPath = "Prefab/InGame/Bullet/" + passBulletData.SkillType + "/" + passBulletData.SkillType + passBulletData.SkillAttribute;

                    // �ش� ������ ����
                    // �̰�(������ �ڷ�ƾ�� ����Ǵ�)���� �������� �����ϴ� ������ Bullet�� �������� ��ü�� �̷�� ���ؼ��̴�.
                    GameObject bullet = GameObject.Instantiate(Resources.Load<GameObject>(prefabPath));
                    bullet.tag = "InGameBulletProjectile";
                    bullet.GetComponent<SpriteRenderer>().sortingOrder = 3;

                    // ��� °�� �����Ǵ� bullet���� ��Ÿ����.
                    passBulletData.Index = i;

                    new PresenterBullet.PresenterBullet(bullet, passBulletData);
                }


                yield return new WaitForSeconds(modelbullet.SkillSpawnTime);
            }
        }

        // ���� �� ��, UI ���� ������ ������.
        private void WriteModelStatusLevelInLevelUpUI()
        {
            JArray statusData = JArray.Parse(File.ReadAllText("./Assets/GameData/ModelStatusInLevelIncrease/ModelStatusInLevelIncrease.json"));

            for (int i = 0; i < statusData.Count; i++)
            {
                StatusNumber(i, (JObject)statusData[i]);
            }
        }

        // ���� �� UI�� ���� ������ ���, Button �̺�Ʈ ����.
        private void StatusNumber(int index, JObject statusData)
        {
            // main/skillPanel/Box(i) : 1-0- i
            // Image : 0-0, Name : 1-0, Detail : 2-0

            //            LevelUPPanel.GetChild(1).GetChild(0).GetChild(index).GetChild(0).GetChild(0).GetComponent<Image>().sprite = ;
            levelUPPanel.GetChild(1).GetChild(0).GetChild(index).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)statusData["StatusName"];
            levelUPPanel.GetChild(1).GetChild(0).GetChild(index).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)statusData["StatusDetail"];

            // Button.onClick.AddListener(Events.UnityAction callbackfunction);
            // delegate void UnityAction();
            // delegate�� �����Ϳ� ����� ������ �Ѵ�.
            // ���� ����ϰ� ���� �Լ��� �̸� ������ ��, delegate�� �����Ͽ� �Ű������� �����ϴ� Button�� ������ �� �ִ�.
            levelUPPanel.GetChild(1).GetChild(0).GetChild(index).GetComponent<Button>().onClick.AddListener(delegate { OnLevelUpButtonEvent(index); });
        }

        // Ŭ���� ��ư�� index�� ���´�.
        // ModelStatusInLevelIncrease.json �� ������ ���� ����.
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




        // �÷��̾� ���� ���.
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

        // GameOverPanel â�� �÷��̾� ���� ���
        public void WriteModelPlayerInfoToGameOverPanel(Transform playerLevelIngameOverPanel)
        {
            playerLevelIngameOverPanel.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.Level);
            playerLevelIngameOverPanel.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelDamage);
            playerLevelIngameOverPanel.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletCount);
            playerLevelIngameOverPanel.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletHitCount);
            playerLevelIngameOverPanel.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletHoldingTime);
            playerLevelIngameOverPanel.GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(modelStatusLevel.LevelBulletSpawnTime);
        }

        // ���� ���, �÷��̾� Status UI Ŭ�� ��ư.
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



        // ��� ��ų ���� ���.
        public void WriteModelSkillsInfo(Transform positionForCreateSkillInfoPrefab)
        {
            Dictionary<string, int[]> displayUsedSkills = new Dictionary<string, int[]>();

            // SkillType�� ������ŭ ������ ����.
            for(int i = 0; i < modelBullets.Count; i++)
            {
                // ����ϴ� ��ų ������ ū Ʋ ����.
                GameObject usedSkillsPanel = Instantiate(Resources.Load<GameObject>("Prefab/InGame/UI/UsedSkill/UsedSkillPanel"));
                usedSkillsPanel.transform.SetParent(positionForCreateSkillInfoPrefab);
                usedSkillsPanel.SetActive(true);

                displayUsedSkills.Add(modelBullets.Keys.ToList()[i], new int[2] { i, 0 } );

/*                Debug.Log("displayUsedSkills.Keys.ToList() : " + displayUsedSkills.Keys.ToList());
                Debug.Log("displayUsedSkills.Keys.ToList()[0] : " + displayUsedSkills.Keys.ToList()[i]);
                Debug.Log("displayUsedSkills[Projectile] : " + displayUsedSkills["Projectile"]);
                Debug.Log("displayUsedSkills[Projectile][0] : " + displayUsedSkills["Projectile"][0]);
                Debug.Log("displayUsedSkills[Projectile][1] : " + displayUsedSkills["Projectile"][1]);*/

                // �ռ� ��ų �̹��� ����.
                string skillImage_path = "SkillImage/" + displayUsedSkills.Keys.ToList()[i] + "/" + displayUsedSkills.Keys.ToList()[i] + "_" + modelBullets[modelBullets.Keys.ToList()[i]].SkillAttribute;
//                Debug.Log("skillImage_path : " + skillImage_path);

                usedSkillsPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(skillImage_path);
            }

            // ���� ���� ��ų �̹��� ����.
            for (int i = 0; i < modelSkills.Count; i++)
            {
                string skillImage_path = "SkillImage/" + modelSkills[i].SkillType + "/" + modelSkills[i].SkillType + "_" + (int)Enum.Parse(typeof(AttributeData), modelSkills[i].SkillAttribute);
//                Debug.Log("skillImage_path : " + skillImage_path);

                positionForCreateSkillInfoPrefab.GetChild(displayUsedSkills[modelSkills[i].SkillType][0]).GetChild(1).GetChild(displayUsedSkills[modelSkills[i].SkillType][1]).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(skillImage_path);
                // modelSkills.SkillType�� ���� Dictionary�� ���ڸ� �����Ѵ�.
                // �ش� ������ �̹��� ����.
                displayUsedSkills[modelSkills[i].SkillType][1]++;
            }
        }
        // ���� ���, ��ų UI Ŭ�� ��ư.
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

        // SkillLevelUp ��, Panel ���.
        public void LevelUPEvent()
        {
            levelUPPanel.gameObject.SetActive(true);
        }
    }

}