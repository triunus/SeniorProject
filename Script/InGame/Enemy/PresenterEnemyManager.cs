using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using UnityEngine;

using ConnectServer;

using MVP.InGameEnemy.ModelEnemy;
using MVP.InGameEnemy.PresenterEnemy;
using MVP.InGameEnemy.ViewEnemy;

enum CreateType
{
    individual,
    colony,
    wall,
    surround
}

namespace MVP.InGameEnemy.PresenterEnemyManager
{
    // �޾ƿ� �� �����͸� List�� �����Ѵ�.
    public class PresenterEnemyManager : MonoBehaviour
    {
        private System.Random random = new System.Random();
        private PresenterEnemy.PresenterEnemy presenterEnemy;
        private GameObject enemyManager;

        // �Ʒ� 1�� �޴����� ������ ���� ����
        private List<ModelEnemyInformation> modelEnemyInformations = new List<ModelEnemyInformation>();
        private List<ModelEnemyInformation> modelBossInformations = new List<ModelEnemyInformation>();

        private List<PresenterEnemy.PresenterEnemy> enemyList = new List<PresenterEnemy.PresenterEnemy>();

        public List<PresenterEnemy.PresenterEnemy> EnemyList => enemyList;

        private float angle = 0f;

        // �����ڷν�, ������ Awake or Start ������ �Ѵ�.
        public void SettingEnemyManager(JObject responseData)
        {
            CraeteEnemyInformation((JArray)responseData["enemyInfo"]);
            CraeteEnemyInformation((JArray)responseData["bossInfo"]);

            StopCoroutine("CreateEnemy");
            StopCoroutine("CreateBoss");

            StartCoroutine(CreateEnemy(modelEnemyInformations));
            StartCoroutine(CreateBoss(modelBossInformations));
        }

        public void CraeteEnemyInformation(JArray responseData)
        {
            for (int i = 0; i < responseData.Count; i++)
            {
                ModelEnemyInformation temp = new ModelEnemyInformation();

                temp.EnemyNumber = (int)responseData[i]["enemyNumber"];
                temp.EnemyType = (string)responseData[i]["enemyType"];
                temp.CreateType = (string)responseData[i]["createType"];
                temp.SpawnTime = (float)responseData[i]["spawnTime"];
                temp.MaxHP = (int)responseData[i]["maxHP"];
                temp.Damage = (float)responseData[i]["damage"];
                temp.Speed = (int)responseData[i]["speed"];
                temp.Experience = (float)responseData[i]["experience"];
                temp.Score = (int)responseData[i]["score"];
                temp.Coin = (int)responseData[i]["coin"];

                switch ((EnumEnemyType)Enum.Parse(typeof(EnumEnemyType), (string)responseData[i]["enemyType"]))
                {
                    case EnumEnemyType.NomalEnemy:
                        modelEnemyInformations.Add(temp);
                        break;
                    case EnumEnemyType.BossEnemy:
                        modelBossInformations.Add(temp);
                        break;
                    default:
                        break;
                }
            }
        }

        // �ð��� ���� ������ ���������� Manager�� ���Ѵ�.
        // �ð��� ����, Enemy�� ��ȭ�� ModelEnemy-PresenterEnemy���� �����Ѵ�.

        // �������� ���� ���ϰ�, ���� ������ ��ȯ.
        public ModelEnemyInformation RandomEnemyChange(List<ModelEnemyInformation> informations)
        {
            // enemy�� ��ȣ�� 1���� �ִٰ� �� �� ����. 1~4�ϼ��� 5~8�� ���� ����.
            // enemy ��ȣ�� 01, 02, 03, 04�� ����, 1, 2, 3, 4�� ���� �Ǿ�� �Ѵ�.
            // 0, 1, 2, 3�� �������� �ް� + 1�� �ع�����.
            int randomValue = random.Next(
                informations[0].EnemyNumber - 1
                , informations[informations.Count - 1].EnemyNumber
                );

            Debug.Log("randomValue : " + randomValue);

            for (int i = 0; i< informations.Count; i++)
            {
                if(randomValue + 1 == informations[i].EnemyNumber)
                {
                    Debug.Log("informations[i].CreateType : " + informations[i].CreateType);

                    return informations[i].DeepCopyToModelEnemyInformation();
                }
            }

            return null;
        }

        // ���� ����
        private IEnumerator CreateBoss(List<ModelEnemyInformation> modelBossInformations)
        {
            ModelEnemyInformation modelBossInformation = new ModelEnemyInformation();

            while (true)
            {
                if ((int)GameObject.FindWithTag("GameManager").GetComponent<GameManager>().RecordTime % 30 == 0) modelBossInformation = RandomEnemyChange(modelBossInformations);

                int i = 0;
                angle = random.Next(0, 360);

                Debug.Log("modelBossInformation.CreateType : " + modelBossInformation.CreateType);

                while (i < (int)((EnumCreateType)Enum.Parse(typeof(EnumCreateType), modelBossInformation.CreateType)))
                {
                    ModelEnemy.ModelEnemy modelBoss = modelBossInformation.DeepCopyToModelEnemy();

                    modelBoss.GetStartPosition(modelBossInformation.CreateType, angle, i);
                    CreatePrefab(modelBossInformation, modelBoss);
                    i++;
                }

                yield return new WaitForSeconds(modelBossInformation.SpawnTime);
            }
        }

        // �� ����, ������ �ϳ��� �Ѵ�.
        private IEnumerator CreateEnemy(List<ModelEnemyInformation> modelEnemyInformations)
        {
            ModelEnemyInformation modelEnemyInformation = new ModelEnemyInformation();

            int previousTime = -1;

            while (true)
            {
                if (previousTime != (int)GameObject.FindWithTag("GameManager").GetComponent<GameManager>().RecordTime / 30) modelEnemyInformation = RandomEnemyChange(modelEnemyInformations);

                previousTime = (int)GameObject.FindWithTag("GameManager").GetComponent<GameManager>().RecordTime / 30;

                int i = 0;
                angle = random.Next(0, 360);

                while (i < (int)((EnumCreateType)Enum.Parse(typeof(EnumCreateType), modelEnemyInformation.CreateType)))
                {
                    ModelEnemy.ModelEnemy modelEnemy = modelEnemyInformation.DeepCopyToModelEnemy();

                    modelEnemy.GetStartPosition(modelEnemyInformation.CreateType, angle, i);

                    // �ð��� ������ ���� Enemy ���׷��̵�.
                    modelEnemy.SetEnemyUpgrade();

                    CreatePrefab(modelEnemyInformation, modelEnemy);

                    if(((EnumCreateType)Enum.Parse(typeof(EnumCreateType), modelEnemyInformation.CreateType)) == EnumCreateType.Colony && (int)((EnumCreateType)Enum.Parse(typeof(EnumCreateType), modelEnemyInformation.CreateType))/2 == i)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }

                    i++;
                }

                yield return new WaitForSeconds(modelEnemyInformation.SpawnTime);
            }
        }

        public void CreatePrefab(ModelEnemyInformation modelEnemyInformation, ModelEnemy.ModelEnemy modelEnemy)
        {
            PresenterEnemy.PresenterEnemy clone;    
            GameObject enemy;
            string prefabPath;

            switch ((EnumEnemyType)Enum.Parse(typeof(EnumEnemyType), modelEnemyInformation.EnemyType))
            {
                case EnumEnemyType.NomalEnemy:
                    prefabPath = "Prefab/InGame/Enemy/Enemy" + Convert.ToString(modelEnemyInformation.EnemyNumber);
                    enemy = Instantiate(Resources.Load<GameObject>(prefabPath));
                    enemy.name = "Enemy" + Convert.ToString(modelEnemyInformation.EnemyNumber);
                    enemy.tag = "Enemy";

                    clone = new PresenterEnemy.PresenterEnemy(enemy, modelEnemy);

                    enemyList.Add(clone);
                    break;
                case EnumEnemyType.BossEnemy:
                    prefabPath = "Prefab/InGame/Boss/Boss" + Convert.ToString(modelEnemyInformation.EnemyNumber);
                    enemy = Instantiate(Resources.Load<GameObject>(prefabPath));
                    enemy.name = "Boss" + Convert.ToString(modelEnemyInformation.EnemyNumber);
                    enemy.tag = "Boss";

                    clone = new PresenterEnemy.PresenterEnemy(enemy, modelEnemy);

                    enemyList.Add(clone);
                    break;
                default:
                    break;
            }
        }

        public void DestroyPresenterEnemy(PresenterEnemy.PresenterEnemy presenterEnemy)
        {
            enemyList.Remove(presenterEnemy);
            presenterEnemy = null;
            GC.Collect();
        }
    }
}


/*            // ���⼭ spawnType�� ���� �ٸ��� ��ġ�� �ٸ��� �����Ѵ�.
            switch ((CreateType)Enum.Parse(typeof(CreateType), ModelEnemyInformation.CreateType))
            {
                // ������ ������ ���
                case CreateType.individual:
                    modelEnemy.GetStartPosition(ModelEnemyInformation.CreateType, angle, 0);
                    CreatePrefab();
                    break;
                // angle�� ���� ����, enemy���� �浹�� �����ϰ� �Ͽ� �� ������, ������ ������.
                case CreateType.colony:
                    while (i < (int)((CreateType)Enum.Parse(typeof(CreateType), ModelEnemyInformation.CreateType)))
                    {
                        modelEnemy.GetStartPosition(ModelEnemyInformation.CreateType, angle, i);
                        CreatePrefab();
                        i++;
                    }
                    break;
                // angle�� 5�� ����.
                case CreateType.wall:
                    while (i < 6)
                    {
                        modelEnemy.GetStartPosition(ModelEnemyInformation.CreateType, angle, i);
                        CreatePrefab();
                        i++;
                    }
                    break;
                // angle�� 20�� ����.
                case CreateType.surround:
                    while (i < 12)
                    {
                        modelEnemy.GetStartPosition(ModelEnemyInformation.CreateType, angle, i);
                        CreatePrefab();
                        i++;
                    }
                    break;
            }*/