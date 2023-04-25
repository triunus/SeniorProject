using System;
using System.Collections.Generic;
using UnityEngine;

using MVP.InGameFunction;
using MVP.InGameSkills.EnumData;

namespace MVP.InGameSkills.ModelBullet
{

    // A : Enum을 통해 각 string 값에 대하여 2^(n) [n>=0]값을 줍니다.
    // B : Enum을 통해 각 string 값에 대하여 정수 값을 부여한다. ---> 부여 받은 정수값은 우선순위를 판별하는데 사용한다.
    // C : 보통 int 또는 float 형으로 데이터가 오는 변수들이며, 모든 값을 더한 후, 총 개수로 나누는 연산을 한다.
    // D : 보통 int 또는 float 형으로 데이터가 오는 변수들이며, 모든 값 중, 최댓값을 값을 기록한다.
    public class ModelBullet
    {
        Movement2D movement2D = new Movement2D();
        
        // 이곳의 정보를 통해 움직임을 결정한다.
        // 2 5 1 2 2 1 3
        private string skillType;
        private int skillAttribute;                 // A        --

        private int skillStartPosition;                  // B   --
        private int skillStartPositionReposition;        // B   --
        private int skillDestinationPosition;            // B   --
        private int skillDestinationPositionReposition;  // B   --
        private int skillMovementType;                   // A   --

        private int skillAttackEffect;              // A        

        private int skillDestroyType;               // B
        private int skillDestroyEffect;          // A

        private int skillDestroyCount;              // max
        private float skillDestroyTime;               // max

        private int skillBulletSize;

        private float skillDamage;                  // sum / n        
        private int skillBulletCount;
        private float skillSpawnTime;               // sum / n

        // 2 5 1 2 2 1 3
        public string SkillType
        {
            get { return skillType; }
            set { skillType = value; }
        }
        public int SkillAttribute
        {
            get { return skillAttribute; }
            set { skillAttribute = value; }
        }

        public int SkillStartPosition
        {
            get { return skillStartPosition; }
            set { skillStartPosition = value; }
        }
        public int SkillStartPositionReposition
        {
            get { return skillStartPositionReposition; }
            set { skillStartPositionReposition = value; }
        }
        public int SkillDestinationPosition
        {
            get { return skillDestinationPosition; }
            set { skillDestinationPosition = value; }
        }
        public int SkillDestinationPositionReposition
        {
            get { return skillDestinationPositionReposition; }
            set { skillDestinationPositionReposition = value; }
        }
        public int SkillMovementType
        {
            get { return skillMovementType; }
            set { skillMovementType = value; }
        }

        public int SkillAttackEffect
        {
            get { return skillAttackEffect; }
            set { skillAttackEffect = value; }
        }

        public int SkillDestroyType
        {
            get { return skillDestroyType; }
            set { skillDestroyType = value; }
        }
        public int SkillDestroyEffect
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

        public int SkillBulletSize
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


        // 알고리즘을 통해 나온 결과 값들이 저장된다.
        private int index;
        private Vector3 startPosition;
        private Vector3 destinationPosition;
        private Vector3 directionVector;
        // Bullet의 움직임 방법들을 기록.
        private List<int> movementType = new List<int>();

        private float speed;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public Vector3 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }
        public Vector3 DestinationPosition
        {
            get { return destinationPosition; }
            set { destinationPosition = value; }
        }
        public Vector3 DirectionVector
        {
            get { return directionVector; }
            set { directionVector = value; }
        }
        public List<int> MovementType
        {
            get { return movementType; }
            set { movementType = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        // 시작 지점을 설정한다.
        public void SetBulletStartPosition()
        {
            switch (SkillStartPosition)
            {
                case 1:
                    StartPosition = GetPlayerPosition();
                    break;
                case 2:
                    break;
                case 3:
                    GetSingleTarget(ref startPosition);
                    break;
                case 4:
                    GetMultipleTargers(ref startPosition);
                    break;
                case 5:
                    break;
                default:
                    break;
            }
        }

        // 목적 지점을 설정한다.
        public void SetBulletDestinationPosition()
        {
            int value = 0;

            for (int i = 0; SkillDestinationPosition > Math.Pow(2, i); i++)
            {
                value = (int)Math.Pow(2, i);
            }

            switch (SkillDestinationPosition)
            {
                case 0:
                    DestinationPosition = StartPosition;
                    break;
                case 1:
                    DestinationPosition = GetPlayerPosition();
                    break;
                case 2:
                    DestinationPosition = GetPlayerPosition() + new Vector3(1, 0, 0);
                    break;
                case 3:
                    GetSingleTarget(ref destinationPosition);
                    break;
                case 4:
                    GetMultipleTargers(ref destinationPosition);
                    break;
                case 5:
                    // 랜덤.
                    break;
                default:
                    break;
            }
        }

        // 움직이는 방향 벡터를 설정한다.
        public void SetBulletDirectionPosition()
        {
            // ->SD = ->OD - ->OS
            DirectionVector = (DestinationPosition - StartPosition).normalized;
        }

        // List<int> movementType에 움직일 방식을 기록.
        // 여러 움직임을 동시에 작동 시키면서, 복잡한 움직임을 만드는 역할을 합니다.
        // movementType에는 [0, 1, ..., 4, ..., 6 ...]의 값이 들어간다.
        public void SetAndGetMovementMethod()
        {
            int tmp = SkillMovementType;
            movementType.Clear();


            Debug.Log("movementType : " + tmp);

            for (int i = 0; tmp > 0; i++)
            {
                if (tmp % 2 == 0  && i == 0)
                {
                    movementType.Add(++i);
                }
                if (tmp % 2 == 1)
                {
                    movementType.Add(++i);
                }

                tmp = tmp >> 1;
            }
        }






        // 위치 참조.               // 알고리즘 내에서(Model 내에서) 호출하는 함수
        private void GetSingleTarget(ref Vector3 position)
        {
            float tmpMaxDistance = 100;
            int enemyListIndex = 0;

            List<MVP.InGameEnemy.PresenterEnemy.PresenterEnemy> enemyList = GetEnemiesPosition();

            for (int i = 0; i < enemyList.Count; i++)
            {
                float distance = Vector3.Distance(enemyList[i].GetEnemyPosition(), startPosition);

                if (distance <= 10 && distance <= tmpMaxDistance)
                {
                    tmpMaxDistance = distance;
                    enemyListIndex = i;
                }
            }

            position = enemyList[enemyListIndex].GetEnemyPosition();
        }

        private void GetMultipleTargers(ref Vector3 position)
        {
            List<MVP.InGameEnemy.PresenterEnemy.PresenterEnemy> enemyList = GetEnemiesPosition();
            List<DistanceStartToDestination> distanceStartToDestinationes = new List<DistanceStartToDestination>();

            for (int i = 0; i < enemyList.Count; i++)
            {
                distanceStartToDestinationes.Add(new DistanceStartToDestination(i, Vector3.Distance(enemyList[i].GetEnemyPosition(), startPosition)));
            }

            distanceStartToDestinationes.Sort(delegate (DistanceStartToDestination x, DistanceStartToDestination y)
            {
                return x.Distance.CompareTo(y.Distance);
            });

            position = enemyList[distanceStartToDestinationes[Index].Index].GetEnemyPosition();
        }

/* 
        private void GetMultipleTargers(List<Vector3> position)
        {
            List<MVP.InGameEnemy.PresenterEnemy.PresenterEnemy> enemyList = GetEnemiesPosition();
            List<DistanceStartToDestination> distanceStartToDestinationes = new List<DistanceStartToDestination>();

            for(int  i = 0; i < enemyList.Count; i++)
            {
                distanceStartToDestinationes.Add(new DistanceStartToDestination(i, Vector3.Distance(enemyList[i].GetEnemyPosition(), startPosition)));
            }

            distanceStartToDestinationes.Sort(delegate (DistanceStartToDestination x, DistanceStartToDestination y)
            {
                return x.Distance.CompareTo(y.Distance);
            });

            for (int i = 0; i < SkillBulletCount; i++){
                position.Add(enemyList[distanceStartToDestinationes[i].Index].GetEnemyPosition());
            }
        }*/

        private Vector3 GetPlayerPosition()
        {
            return GameObject.FindWithTag("Player").transform.position;
        }

        private List<MVP.InGameEnemy.PresenterEnemy.PresenterEnemy> GetEnemiesPosition()
        {
            return GameObject.FindWithTag("EnemyManager").GetComponent<MVP.InGameEnemy.PresenterEnemyManager.PresenterEnemyManager>().EnemyList;
        }




        // 깊은 복사
        public ModelBullet DeepCopy()
        {
            ModelBullet passBulletData = new ModelBullet();

            passBulletData.SkillType = this.SkillType;
            passBulletData.SkillAttribute = this.SkillAttribute;

            passBulletData.SkillStartPosition = this.SkillStartPosition;
            passBulletData.SkillDestinationPosition = this.SkillDestinationPosition;
            passBulletData.SkillMovementType = this.SkillMovementType;
            passBulletData.SkillStartPositionReposition = this.SkillStartPositionReposition;
            passBulletData.SkillDestinationPositionReposition = this.SkillDestinationPositionReposition;

            passBulletData.SkillAttackEffect = this.SkillAttackEffect;
            passBulletData.SkillDestroyType = this.SkillDestroyType;
            passBulletData.SkillDestroyEffect = this.SkillDestroyEffect;
            passBulletData.SkillDestroyCount = this.SkillDestroyCount;
            passBulletData.SkillDestroyTime = this.SkillDestroyTime;
            passBulletData.SkillBulletSize = this.SkillBulletSize;
            passBulletData.SkillDamage = this.SkillDamage;
            passBulletData.SkillBulletCount = this.SkillBulletCount;
            passBulletData.SkillSpawnTime = this.SkillSpawnTime;

            return passBulletData;
        }
    }

    public class DistanceStartToDestination 
    {
        private int index;
        private float distance;

        public DistanceStartToDestination(int index, float distance)
        {
            this.index = index;
            this.distance = distance;
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }
    }
        
}

