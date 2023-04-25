using System.Collections.Generic;
using System.Collections;
using System;

using UnityEngine;

using MVP.InGameSkills.ModelBullet;
using MVP.InGameSkills.EnumData;

namespace MVP.InGameSkills.ModelSkills
{
    public class ModelSkill
    {
        // 1 2 5 1 2 2 1 3
        private string skillNumber;

        private string skillType;
        private string skillAttribute;

        private string skillStartPosition;
        private string skillStartPositionReposition;
        private string skillDestinationPosition;
        private string skillDestinationPositionReposition;
        private string skillMovementType;

        private string skillAttackEffect;

        private string skillDestroyType;
        private string skillDestroyEffect;

        private int skillDestroyCount;
        private float skillDestroyTime;

        private string skillBulletSize;
        
        private float skillDamage;
        private int skillBulletCount;
        private float skillSpawnTime;

        // Get, Set
        // 1 2 5 1 2 2 1 3
        public string SkillNumber
        {
            get { return skillNumber; }
            set { skillNumber = value; }
        }

        public string SkillType
        {
            get { return skillType; }
            set { skillType = value; }
        }
        public string SkillAttribute
        {
            get { return skillAttribute; }
            set { skillAttribute = value; }
        }

        public string SkillStartPosition
        {
            get { return skillStartPosition; }
            set { skillStartPosition = value; }
        }
        public string SkillStartPositionReposition
        {
            get { return skillStartPositionReposition; }
            set { skillStartPositionReposition = value; }
        }
        public string SkillDestinationPosition
        {
            get { return skillDestinationPosition; }
            set { skillDestinationPosition = value; }
        }
        public string SkillDestinationPositionReposition
        {
            get { return skillDestinationPositionReposition; }
            set { skillDestinationPositionReposition = value; }
        }
        public string SkillMovementType
        {
            get { return skillMovementType; }
            set { skillMovementType = value; }
        }

        public string SkillAttackEffect
        {
            get { return skillAttackEffect; }
            set { skillAttackEffect = value; }
        }

        public string SkillDestroyType
        {
            get { return skillDestroyType; }
            set { skillDestroyType = value; }
        }
        public string SkillDestroyEffect
        {
            get { return skillDestroyEffect; }
            set { skillDestroyEffect = value; }
        }

        public int SkillDestroyCount
        {
            get { return skillDestroyCount; }
            set { skillDestroyCount = value; }
        }
        public float SkillDestroyTime
        {
            get { return skillDestroyTime; }
            set { skillDestroyTime = value; }
        }

        public string SkillBulletSize
        {
            get { return skillBulletSize; }
            set { skillBulletSize = value; }
        }

        public float SkillDamage
        {
            get { return skillDamage; }
            set { skillDamage = value; }
        }
        public int SkillBulletCount
        {
            get { return skillBulletCount; }
            set { skillBulletCount = value; }
        }
        public float SkillSpawnTime
        {
            get { return skillSpawnTime; }
            set { skillSpawnTime = value; }
        }
    }

    public class ModelStatusLevel
    {
        private int level;
        private int levelDamage;
        private int levelBulletCount;
        private int levelBulletHitCount;
        private float levelBulletHoldingTime;
        private float levelBulletSpawnTime;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        public int LevelDamage
        {
            get { return levelDamage; }
            set { levelDamage = value; }
        }
        public int LevelBulletCount
        {
            get { return levelBulletCount; }
            set { levelBulletCount = value; }
        }
        public int LevelBulletHitCount
        {
            get { return levelBulletHitCount; }
            set { levelBulletHitCount = value; }
        }
        public float LevelBulletHoldingTime
        {
            get { return levelBulletHoldingTime; }
            set { levelBulletHoldingTime = value; }
        }
        public float LevelBulletSpawnTime
        {
            get { return levelBulletSpawnTime; }
            set { levelBulletSpawnTime = value; }
        }
    }

    public class CompoundModelSkills
    {
        // modelskills�� ���� ��ų Ÿ���� ������ ��Ÿ����.
        List<string> eachTypeName = new List<string>();
        Dictionary<string, int> numberOfEachType = new Dictionary<string, int>();
        Dictionary<string, ModelBullet.ModelBullet> classifiedModelSkills = new Dictionary<string, ModelBullet.ModelBullet>();

        public List<string> GetEachTypeName()
        {
            return eachTypeName;
        }

        public Dictionary<string, int> GetNumberOfEachType()
        {
            return numberOfEachType;
        }

        // A : Enum�� ���� �� string ���� ���Ͽ� 2^(n) [n>=0]���� �ݴϴ�.
        // B : Enum�� ���� �� string ���� ���Ͽ� ���� ���� �ο��Ѵ�. ---> �ο� ���� �������� �켱������ �Ǻ��ϴµ� ����Ѵ�.
        // C : ���� int �Ǵ� float ������ �����Ͱ� ���� �������̸�, ��� ���� ���� ��, �� ������ ������ ������ �Ѵ�.
        // D : ���� int �Ǵ� float ������ �����Ͱ� ���� �������̸�, ��� �� ��, �ִ��� ���� ����Ѵ�.
        public Dictionary<string, ModelBullet.ModelBullet> CompoundSkills(List<ModelSkill> modelSkills, ModelStatusLevel modelStatusLevel)
        {
            ClearClassInnerField();
            ClassifyModelSkillsToDictionary(modelSkills);

            for (int i = 0; i < modelSkills.Count; i++)
            {
                // A
                classifiedModelSkills[modelSkills[i].SkillType].SkillAttribute = classifiedModelSkills[modelSkills[i].SkillType].SkillAttribute | (int)Enum.Parse(typeof(AttributeData), modelSkills[i].SkillAttribute);

                classifiedModelSkills[modelSkills[i].SkillType].SkillMovementType = classifiedModelSkills[modelSkills[i].SkillType].SkillMovementType | (int)Enum.Parse(typeof(MovementTypeData), modelSkills[i].SkillMovementType);

                classifiedModelSkills[modelSkills[i].SkillType].SkillAttackEffect = classifiedModelSkills[modelSkills[i].SkillType].SkillAttackEffect | (int)Enum.Parse(typeof(AttackEffectData), modelSkills[i].SkillAttackEffect);

                classifiedModelSkills[modelSkills[i].SkillType].SkillDestroyEffect = classifiedModelSkills[modelSkills[i].SkillType].SkillDestroyEffect | (int)Enum.Parse(typeof(DestroyEffectData), modelSkills[i].SkillDestroyEffect);


                // B
                classifiedModelSkills[modelSkills[i].SkillType].SkillStartPosition = Math.Max(classifiedModelSkills[modelSkills[i].SkillType].SkillStartPosition, (int)Enum.Parse(typeof(StartPositionData), modelSkills[i].SkillStartPosition));
                classifiedModelSkills[modelSkills[i].SkillType].SkillDestinationPosition = Math.Max(classifiedModelSkills[modelSkills[i].SkillType].SkillDestinationPosition, (int)Enum.Parse(typeof(DestinationPositionData), modelSkills[i].SkillDestinationPosition));
                classifiedModelSkills[modelSkills[i].SkillType].SkillStartPositionReposition = Math.Max(classifiedModelSkills[modelSkills[i].SkillType].SkillStartPositionReposition, (int)Enum.Parse(typeof(StartPositionReposition), modelSkills[i].SkillStartPositionReposition));
                classifiedModelSkills[modelSkills[i].SkillType].SkillDestinationPositionReposition = Math.Max(classifiedModelSkills[modelSkills[i].SkillType].SkillDestinationPositionReposition, (int)Enum.Parse(typeof(DestinationPosistionReposition), modelSkills[i].SkillDestinationPositionReposition));


                classifiedModelSkills[modelSkills[i].SkillType].SkillDestroyType = Math.Max(classifiedModelSkills[modelSkills[i].SkillType].SkillDestroyType, (int)Enum.Parse(typeof(DestroyTypeData), modelSkills[i].SkillDestroyType));
                classifiedModelSkills[modelSkills[i].SkillType].SkillBulletSize = Math.Max(classifiedModelSkills[modelSkills[i].SkillType].SkillBulletSize, (int)Enum.Parse(typeof(BulletSizeData), modelSkills[i].SkillBulletSize));


                // C
                classifiedModelSkills[modelSkills[i].SkillType].SkillDestroyTime = classifiedModelSkills[modelSkills[i].SkillType].SkillDestroyTime + modelSkills[i].SkillDestroyTime;
                classifiedModelSkills[modelSkills[i].SkillType].SkillDamage = classifiedModelSkills[modelSkills[i].SkillType].SkillDamage + modelSkills[i].SkillDamage;
                classifiedModelSkills[modelSkills[i].SkillType].SkillSpawnTime = classifiedModelSkills[modelSkills[i].SkillType].SkillSpawnTime + modelSkills[i].SkillSpawnTime;


                // D
                classifiedModelSkills[modelSkills[i].SkillType].SkillDestroyCount = Math.Max(classifiedModelSkills[modelSkills[i].SkillType].SkillDestroyCount, modelSkills[i].SkillDestroyCount);
                classifiedModelSkills[modelSkills[i].SkillType].SkillBulletCount = Math.Max(classifiedModelSkills[modelSkills[i].SkillType].SkillBulletCount, modelSkills[i].SkillBulletCount);

                // �� skillType�� �� ���� ��ų�� ���������� �ľ��ϴµ� ���.
                numberOfEachType[modelSkills[i].SkillType]++;
            }

            for (int i = 0; i < eachTypeName.Count; i++)
            {
                classifiedModelSkills[eachTypeName[i]].SkillDamage = classifiedModelSkills[eachTypeName[i]].SkillDamage * ( 10 + modelStatusLevel.LevelDamage * numberOfEachType[eachTypeName[i]]) / 10 * numberOfEachType[eachTypeName[i]];

                classifiedModelSkills[eachTypeName[i]].SkillBulletCount = classifiedModelSkills[eachTypeName[i]].SkillBulletCount + modelStatusLevel.LevelBulletCount;
                classifiedModelSkills[eachTypeName[i]].SkillDestroyCount = classifiedModelSkills[eachTypeName[i]].SkillDestroyCount + modelStatusLevel.LevelBulletHitCount;

                classifiedModelSkills[eachTypeName[i]].SkillSpawnTime = classifiedModelSkills[eachTypeName[i]].SkillSpawnTime * (10 - modelStatusLevel.LevelBulletSpawnTime * numberOfEachType[eachTypeName[i]]) / 10 * numberOfEachType[eachTypeName[i]];
                classifiedModelSkills[eachTypeName[i]].SkillDestroyTime = classifiedModelSkills[eachTypeName[i]].SkillDestroyTime * (10 + modelStatusLevel.LevelBulletHoldingTime * numberOfEachType[eachTypeName[i]]) / 10 * numberOfEachType[eachTypeName[i]];
            }

            return classifiedModelSkills;
        }

        public void ClassifyModelSkillsToDictionary(List<ModelSkill> modelSkills)
        {
            // ������ ��ų���� Ÿ������ �����ϱ� ���ؼ�,
            // ���� Ÿ�Ե� ��, ������ Ÿ�Կ� ���ؼ��� List�� �־� ���´�.
            for (int i = 0; i < modelSkills.Count; i++)
            {
                if (!eachTypeName.Contains(modelSkills[i].SkillType))
                {
                    eachTypeName.Add(modelSkills[i].SkillType);
                }
            }

            // �з��� Ÿ�Ե��� �̸��� Key�ν�, Value�δ� ��ó�� ��ų�� ������ ����� ModelBullet ��ü�� Dictionary�� �����Ѵ�.
            // ���� �ش� Dictionary�� ���ԵǴ� ModelSkill�� ������ Dictionary(��ų Ÿ��, ���Ե� ����)���� �����Ͽ�,
            //                                                      ����, ���Ե� �� ������ ���� Damage ���� ��հ� ���� ���� ����� �����̴�.
            for (int i = 0; i < eachTypeName.Count; i++)
            {
                ModelBullet.ModelBullet test = new ModelBullet.ModelBullet();
                test.SkillType = eachTypeName[i];
                classifiedModelSkills.Add(eachTypeName[i], test);
                numberOfEachType.Add(eachTypeName[i], 0);
            }
        }

        public void ClearClassInnerField()
        {
            classifiedModelSkills.Clear();
            eachTypeName.Clear();
            numberOfEachType.Clear();
        }
    }
}
