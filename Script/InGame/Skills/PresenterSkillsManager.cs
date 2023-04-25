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
    // View������ ��ȭ�� ������Ʈ�ϸ�, Model������ ��ȭ�� ������Ʈ�ϴ� �������� �����ϴµ� ����� �� �ִ�.
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
        // �ش� Ŭ������ ���� Ŭ����
        private CompoundModelSkills compoundModelSkills = new CompoundModelSkills();
        private ModelStatusLevel modelStatusLevel = new ModelStatusLevel();
        // ���� ����� ����ü
        private Dictionary<string, ModelBullet.ModelBullet> modelBullets = new Dictionary<string, ModelBullet.ModelBullet>();

        // �ʱ� ���� + �Լ� ���� ����
        public PresenterSkillsManager(GameObject skillsManager, JArray skillsData)
        {
            iViewSkills = skillsManager.GetComponent<ViewSkillsManager>();
            iViewSkills.SetSkillsManager(this, skillsManager);

            UpdateModelSkills(skillsData);
        }

        // �������� ��ų �ް� modelSkills�� ����. ����, ��ų �ռ� �Լ� ȣ��.
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

        // ���� �� ������ ������, �ð� or �� �� ���ǿ� ���� ���� ��ũ ������ ����
        // �������� ��ũ�� �ް� ��ũ�� �´� ��ų�ѹ��� ����
        // ������ ��ų �ѹ�, recordNumber�� �ѱ��. ���������� �̸� ��� ��, ��ų �ѹ��� ���� ������ �����Ѵ�.
        // ����, �Ѿ�� ��ų ������ �߰������� modelSkills�� �߰��Ѵ�.
        // �߰� ���� ��, CompoundSkills()�� �� ȣ���Ѵ�.
        // ����, iViewSkills.StartCreateBullet();�� �� ȣ���Ѵ�.
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

        // modelSkills�� ����� ��ų���� �����Ͽ� �ռ��Ͽ� modelBullets�� ����.
        public void CompoundSkills()
        {
            modelBullets = compoundModelSkills.CompoundSkills(modelSkills, modelStatusLevel);

            UpdateViewSkills();

            // View���� �ڷ�ƾ ����.
            iViewSkills.StartCreateBullet();
        }

        // Presenter�� ���� �ʵ� ����
        public void UpdatePresenterSkills()
        {
            modelSkills = iViewSkills.ModelSkills;
            modelBullets = iViewSkills.ModelBullets;
            modelStatusLevel = iViewSkills.ModelStatusLevel;
        }

        // View�� ���� �ʵ� ����
        public void UpdateViewSkills()
        {
            iViewSkills.ModelSkills = modelSkills;
            iViewSkills.ModelBullets = modelBullets;
            iViewSkills.ModelStatusLevel = modelStatusLevel;
        }
    }

}