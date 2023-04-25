using System;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;

using MVP.InGameSkills.ModelSkills;
using MVP.InGameSkills.ViewSkills;

using MVP.InGameSkills.ModelBullet;
using MVP.InGameSkills.PresenterBullet;
using MVP.InGameSkills.EnumData;


namespace MVP.InGameSkills.PresenterSkills
{
    // View에서의 변화를 업데이트하며, Model에서의 변화를 업데이트하는 변수들을 공유하는데 사용할 수 있다.
    public interface IViewSkills
    {
        List<ModelSkills.ModelSkill> ModelSkills { get; set; }

        Dictionary<string, ModelBullet.ModelBullet> ModelBullets { get; set; }

        ModelStatusLevel ModelStatusLevel { get; set; }

        //        void SetSkillsManager(PresenterSkillsManager presenterEnemy, GameObject enemey);
        void SetSkillsManager(PresenterSkillsManager presenterEnemy, GameObject skillsManager);

        void StartCreateBullet();
    }

    public class PresenterSkillsManager
    {
        private IViewSkills iViewSkills = null;
        private List<ModelSkills.ModelSkill> modelSkills = new List<ModelSkills.ModelSkill>();
        // 해당 클래스는 로직 클래스
        private CompoundModelSkills compoundModelSkills = new CompoundModelSkills();
        private ModelStatusLevel modelStatusLevel = new ModelStatusLevel();
        // 실제 사용할 투사체
        private Dictionary<string, ModelBullet.ModelBullet> modelBullets = new Dictionary<string, ModelBullet.ModelBullet>();

        // 초기 셋팅 + 함수 실행 순서
        public PresenterSkillsManager(GameObject skillsManager, JArray skillsData)
        {
            iViewSkills = skillsManager.GetComponent<ViewSkillsManager>();
            iViewSkills.SetSkillsManager(this, skillsManager);

            UpdateModelSkills(skillsData);
        }

        // 서버에서 스킬 받고 modelSkills에 저장. 이후, 스킬 합성 함수 호출.
        public void UpdateModelSkills(JArray skillsData)
        {

            for (int i=0; i<skillsData.Count; i++)
            {
                ModelSkill tmp = new ModelSkills.ModelSkill();

                tmp.SkillNumber = (string)skillsData[i]["SkillNumber"];
                tmp.SkillType = (string)skillsData[i]["SkillType"];
                tmp.SkillAttribute = (string)skillsData[i]["SkillAttribute"];
                
                tmp.SkillStartPosition = (string)skillsData[i]["SkillStartPosition"];
                tmp.SkillDestinationPosition = (string)skillsData[i]["SkillDestinationPosition"];
                tmp.SkillMovementType = (string)skillsData[i]["SkillMovementType"];
                tmp.SkillStartPositionReposition = (string)skillsData[i]["SkillStartPositionReposition"];
                tmp.SkillDestinationPositionReposition = (string)skillsData[i]["SkillDestinationPositionReposition"];

                tmp.SkillAttackEffect = (string)skillsData[i]["SkillAttackEffect"];
                tmp.SkillDestroyType = (string)skillsData[i]["SkillDestroyType"];
                tmp.SkillDestroyEffect = (string)skillsData[i]["SkillDestroyEffect"];
                tmp.SkillDestroyCount = (int)skillsData[i]["SkillDestroyCount"];
                tmp.SkillDestroyTime = (int)skillsData[i]["SkillDestroyTime"];
                tmp.SkillBulletSize = (string)skillsData[i]["SkillBulletSize"];
                tmp.SkillDamage = (float)skillsData[i]["SkillDamage"];
                tmp.SkillBulletCount = (int)skillsData[i]["SkillBulletCount"];
                tmp.SkillSpawnTime = (float)skillsData[i]["SkillSpawnTime"];

                modelSkills.Add(tmp);
            }

            modelStatusLevel.Level = 0;
            modelStatusLevel.LevelDamage = 0;
            modelStatusLevel.LevelBulletCount = 0;
            modelStatusLevel.LevelBulletHitCount = 0;
            modelStatusLevel.LevelBulletHoldingTime = 0;
            modelStatusLevel.LevelBulletSpawnTime = 0;

            CompoundSkills();
        }

        // 게임 내 상인을 만나면, 시간 or 그 외 조건에 따라 상인 랭크 서버에 전달
        // 서버에서 랭크를 받고 랭크에 맞는 스킬넘버들 리턴
        // 선택한 스킬 넘버, recordNumber를 넘긴다. 서버에서는 이를 기록 후, 스킬 넘버에 관한 정보를 리턴한다.
        // 이후, 넘어온 스킬 정보를 추가적으로 modelSkills에 추가한다.
        // 추가 성공 후, CompoundSkills()를 재 호출한다.
        // 이후, iViewSkills.StartCreateBullet();를 재 호출한다.
        public void AddSkillModelSkillsInGame()
        {

        }


        public void UpdateStatusLevel(string index)
        {
/*            switch ()
            {
                case    0:
                    break;
            }*/
        }

        // modelSkills에 저장된 스킬들을 구별하여 합성하여 modelBullets를 생성.
        public void CompoundSkills()
        {
            modelBullets = compoundModelSkills.CompoundSkills(modelSkills, modelStatusLevel);

            UpdateViewSkills();

            // View에서 코루틴 실행.
            iViewSkills.StartCreateBullet();
        }

        // Presenter의 내부 필드 갱신
        public void UpdatePresenterSkills()
        {
            modelSkills = iViewSkills.ModelSkills;
            modelBullets = iViewSkills.ModelBullets;
            modelStatusLevel = iViewSkills.ModelStatusLevel;
        }

        // View의 내부 필드 갱신
        public void UpdateViewSkills()
        {
            iViewSkills.ModelSkills = modelSkills;
            iViewSkills.ModelBullets = modelBullets;
            iViewSkills.ModelStatusLevel = modelStatusLevel;
        }
    }

}