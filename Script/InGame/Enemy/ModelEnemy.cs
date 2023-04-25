using System;
using UnityEngine;

namespace MVP.InGameEnemy.ModelEnemy
{
    enum EnumEnemyType
    {
        NomalEnemy,
        BossEnemy
    }
    enum EnumCreateType
    {
        Individual = 1,
        Colony = 10,
        Wall = 6,
        Surround = 12
    }
    public class ModelEnemyInformation
    {
        private int enemyNumber;
        private string enemyType;
        private string createType;
        private float spawnTime;
        private int maxHP;
        private float damage;
        private int speed;
        private float experience;
        private int score;
        private int coin;

        public int EnemyNumber
        {
            get { return enemyNumber; }
            set { enemyNumber = value; }
        }
        public string EnemyType
        {
            get { return enemyType; }
            set { enemyType = value; }
        }
        public string CreateType
        {
            get { return createType; }
            set { createType = value; }
        }
        public float SpawnTime
        {
            get { return spawnTime; }
            set { spawnTime = value; }
        }
        public int MaxHP
        {
            get { return maxHP; }
            set { maxHP = value; }
        }
        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float Experience
        {
            get { return experience; }
            set { experience = value; }
        }
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public int Coin
        {
            get { return coin; }
            set { coin = value; }
        }

        public ModelEnemyInformation DeepCopyToModelEnemyInformation()
        {
            ModelEnemyInformation tmp = new ModelEnemyInformation();

            tmp.EnemyNumber = this.EnemyNumber;
            tmp.EnemyType = this.EnemyType;
            tmp.CreateType = this.CreateType;
            tmp.SpawnTime = this.SpawnTime;
            tmp.MaxHP = this.MaxHP;
            tmp.Damage = this.Damage;
            tmp.Speed = this.Speed;
            tmp.Experience = this.Experience;
            tmp.Score = this.Score;
            tmp.Coin = this.Coin;

            return tmp;
        }


        public ModelEnemy DeepCopyToModelEnemy()
        {
            ModelEnemy tmp = new ModelEnemy();

            tmp.MaxHP = this.MaxHP;
            tmp.CurrentHP = this.MaxHP;
            tmp.Damage = this.Damage;
            tmp.Speed = this.Speed;
            tmp.Experience = this.Experience;
            tmp.Score = this.Score;
            tmp.Coin = this.Coin;

            return tmp;
        }

    }

    public class ModelEnemy
    {
        private Vector3 enemyPosition;
        private int maxHP;
        private int currentHP;
        private float damage;
        private int speed;
        private float experience;
        private int score;
        private int coin;

        public Vector3 EnemyPosition
        {
            get { return enemyPosition; }
            set { enemyPosition = value; }
        }
        public int MaxHP
        {
            get { return maxHP; }
            set { maxHP = value; }
        }
        public int CurrentHP
        {
            get { return currentHP; }
            set { currentHP = value; }
        }
        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float Experience
        {
            get { return experience; }
            set { experience = value; }
        }
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public int Coin
        {
            get { return coin; }
            set { coin = value; }
        }


        public void SetEnemyUpgrade()
        {
            MaxHP = MaxHP + (int)((float)MaxHP * (GameObject.FindWithTag("GameManager").GetComponent<GameManager>().RecordTime / 60));
            CurrentHP = CurrentHP + (int)((float)CurrentHP * (GameObject.FindWithTag("GameManager").GetComponent<GameManager>().RecordTime / 60));
            Damage = Damage + ((float)Damage * (GameObject.FindWithTag("GameManager").GetComponent<GameManager>().RecordTime / 60));
            Coin = Coin + (int)((float)Coin * (GameObject.FindWithTag("GameManager").GetComponent<GameManager>().RecordTime / 60));
        }


        public void GetStartPosition(string createType, float angle, int count)
        {
            switch ((EnumCreateType)Enum.Parse(typeof(EnumCreateType), createType))
            {
                case EnumCreateType.Individual:
                    EnemyPosition = SetEnemyPositionUsedAngle(angle);
                    break;
                case EnumCreateType.Colony:
                    angle = angle + count * 2;
                    EnemyPosition = SetEnemyPositionUsedAngle(angle);
                    break;
                case EnumCreateType.Wall:
                    angle = angle + count * 5;
                    EnemyPosition = SetEnemyPositionUsedAngle(angle);
                    break;
                case EnumCreateType.Surround:
                    angle = angle + count * 20;
                    EnemyPosition = SetEnemyPositionUsedAngle(angle);
                    break;
                default:
                    break;
            }
        }

        public Vector3 SetEnemyPositionUsedAngle(float angle)
        {
            float minDistance = 5;

            Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;

            return new Vector3(
                    (float)(Mathf.Cos(Mathf.Deg2Rad * angle) * 2 * minDistance + playerPosition.x)
                    , (float)(Mathf.Sin(Mathf.Deg2Rad * angle) * 2 * minDistance + playerPosition.y), 0);
        }
    }

}