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
    // 받아온 적 데이터를 List에 저장한다.
    public class PresenterEnemyManager : MonoBehaviour
    {
        private System.Random random = new System.Random();
        private PresenterEnemy.PresenterEnemy presenterEnemy;
        private GameObject enemyManager;

        // 아래 1개 메니저가 가지고 있을 정보
        private List<ModelEnemyInformation> modelEnemyInformations = new List<ModelEnemyInformation>();
        private List<ModelEnemyInformation> modelBossInformations = new List<ModelEnemyInformation>();

        private List<PresenterEnemy.PresenterEnemy> enemyList = new List<PresenterEnemy.PresenterEnemy>();

        public List<PresenterEnemy.PresenterEnemy> EnemyList => enemyList;

        private float angle = 0f;

        // 생서자로써, 일종의 Awake or Start 역할을 한다.
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

        // 시간에 따라 무엇을 생성할지는 Manager가 정한다.
        // 시간에 따른, Enemy의 강화는 ModelEnemy-PresenterEnemy에서 수행한다.

        // 랜덤으로 적을 구하고, 적의 정보를 반환.
        public ModelEnemyInformation RandomEnemyChange(List<ModelEnemyInformation> informations)
        {
            // enemy의 번호가 1부터 있다고 할 수 없다. 1~4일수도 5~8일 수도 있음.
            // enemy 번호가 01, 02, 03, 04가 오면, 1, 2, 3, 4가 리턴 되어야 한다.
            // 0, 1, 2, 3을 랜덤으로 받고 + 1을 해버리자.
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

        // 보스 생성
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

        // 적 세팅, 생성은 하나씩 한다.
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

                    // 시간의 지남에 따라 Enemy 업그레이드.
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


/*            // 여기서 spawnType에 따라 다르게 위치를 다르게 조정한다.
            switch ((CreateType)Enum.Parse(typeof(CreateType), ModelEnemyInformation.CreateType))
            {
                // 낱개로 무작위 출력
                case CreateType.individual:
                    modelEnemy.GetStartPosition(ModelEnemyInformation.CreateType, angle, 0);
                    CreatePrefab();
                    break;
                // angle은 랜덤 유지, enemy간의 충돌이 존재하게 하여 한 점에서, 퍼지게 만들자.
                case CreateType.colony:
                    while (i < (int)((CreateType)Enum.Parse(typeof(CreateType), ModelEnemyInformation.CreateType)))
                    {
                        modelEnemy.GetStartPosition(ModelEnemyInformation.CreateType, angle, i);
                        CreatePrefab();
                        i++;
                    }
                    break;
                // angle이 5씩 증가.
                case CreateType.wall:
                    while (i < 6)
                    {
                        modelEnemy.GetStartPosition(ModelEnemyInformation.CreateType, angle, i);
                        CreatePrefab();
                        i++;
                    }
                    break;
                // angle이 20씩 증가.
                case CreateType.surround:
                    while (i < 12)
                    {
                        modelEnemy.GetStartPosition(ModelEnemyInformation.CreateType, angle, i);
                        CreatePrefab();
                        i++;
                    }
                    break;
            }*/