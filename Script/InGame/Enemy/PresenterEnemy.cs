using System;

using UnityEngine;

using MVP.InGameFunction;

using MVP.InGameEnemy.ModelEnemy;

namespace MVP.InGameEnemy.PresenterEnemy
{
    public interface IViewEnemy
    {
        Vector3 EnemyPosition { get; set; }
        int MaxHP { get; set; }
        int CurrentHP { get; set; }
        float Damage { get; set; }
        int Speed { get; set; }
        float Experience { get; set; }
        int Score { get; set; }
        int Coin { get; set; }

        void SetEnemy(PresenterEnemy presenterEnemy, GameObject enemey);
    }

    public class PresenterEnemy
    {
        Movement2D movement2D = new Movement2D();
        MVP.InGameEnemy.PresenterEnemyManager.PresenterEnemyManager presenterEnemyManager;

        ModelEnemy.ModelEnemy modelEnemy = new ModelEnemy.ModelEnemy();   // ���.
        IViewEnemy iViewEnemy = null;

        public PresenterEnemy(GameObject enemy, ModelEnemy.ModelEnemy enemyData)
        {
            this.iViewEnemy = enemy.GetComponent<ViewEnemy.ViewEnemy>();
            iViewEnemy.SetEnemy(this, enemy);

            modelEnemy = enemyData;
            presenterEnemyManager = GameObject.FindWithTag("EnemyManager").GetComponent<PresenterEnemyManager.PresenterEnemyManager>();
            UpdateViewEnemry();
        }

        public void UpdateModelEnemry()
        {
            modelEnemy.EnemyPosition = iViewEnemy.EnemyPosition;
            modelEnemy.MaxHP = iViewEnemy.MaxHP;
            modelEnemy.CurrentHP = iViewEnemy.CurrentHP;
            modelEnemy.Damage = iViewEnemy.Damage;
            modelEnemy.Speed = iViewEnemy.Speed;
            modelEnemy.Experience = iViewEnemy.Experience;
            modelEnemy.Score = iViewEnemy.Score;
            modelEnemy.Coin = iViewEnemy.Coin;
        }

        public void UpdateViewEnemry()
        {
            iViewEnemy.EnemyPosition = modelEnemy.EnemyPosition;
            iViewEnemy.MaxHP = modelEnemy.MaxHP;
            iViewEnemy.CurrentHP = modelEnemy.CurrentHP;
            iViewEnemy.Damage = modelEnemy.Damage;
            iViewEnemy.Speed = modelEnemy.Speed;
            iViewEnemy.Experience = modelEnemy.Experience;
            iViewEnemy.Score = modelEnemy.Score;
            iViewEnemy.Coin = modelEnemy.Coin;
        }

        public void ChasingPlayer()
        {
            modelEnemy.EnemyPosition = movement2D.MovementChasing(modelEnemy.EnemyPosition, GetPlayerPosition(), modelEnemy.Speed);

            UpdateViewEnemry();
        }

        // player ��ġ�� �ʿ��Ͽ� ȣ���ϴ� �Լ�.
        public Vector3 GetPlayerPosition()
        {
            return GameObject.FindWithTag("Player").transform.position;
        }

        // bullet�� enemy ��ġ ������ �ʿ��� �� ȣ���ϴ� �Լ�.
        public Vector3 GetEnemyPosition()
        {
            return modelEnemy.EnemyPosition;
        }

        // ���� ���� ��.
        public void DyingEnemy()
        {
            // ���ʹ� ����Ʈ���� �ش� ���ʹ̸� �����ϴ� �Լ��� ȣ���Ѵ�.
            presenterEnemyManager.DestroyPresenterEnemy(this);
        }
    }
}