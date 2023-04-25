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
        // modelskills가 갖는 스킬 타입의 종류를 나타낸다.
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

        // A : Enum을 통해 각 string 값에 대하여 2^(n) [n>=0]값을 줍니다.
        // B : Enum을 통해 각 string 값에 대하여 정수 값을 부여한다. ---> 부여 받은 정수값은 우선순위를 판별하는데 사용한다.
        // C : 보통 int 또는 float 형으로 데이터가 오는 변수들이며, 모든 값을 더한 후, 총 개수로 나누는 연산을 한다.
        // D : 보통 int 또는 float 형으로 데이터가 오는 변수들이며, 모든 값 중, 최댓값을 값을 기록한다.
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

                // 각 skillType별 몇 개의 스킬이 합쳐졌는지 파악하는데 사용.
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
            // 선택한 스킬들을 타입으로 구분하기 위해서,
            // 여러 타입들 중, 선택한 타입에 대해서만 List에 넣어 놓는다.
            for (int i = 0; i < modelSkills.Count; i++)
            {
                if (!eachTypeName.Contains(modelSkills[i].SkillType))
                {
                    eachTypeName.Add(modelSkills[i].SkillType);
                }
            }

            // 분류된 타입들의 이름을 Key로써, Value로는 합처진 스킬의 정보가 저장될 ModelBullet 객체로 Dictionary를 정의한다.
            // 또한 해당 Dictionary에 포함되는 ModelSkill의 개수를 Dictionary(스킬 타입, 포함된 개수)으로 정의하여,
            //                                                      차후, 포함된 총 개수를 통해 Damage 값의 평균과 같은 곳에 사용할 예정이다.
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
