using System;

using UnityEngine;

using MVP.InGameFunction;

using MVP.InGameSkills.ModelSkills;
using MVP.InGameSkills.ViewBullet;

using MVP.InGameSkills.ModelBullet;

namespace MVP.InGameSkills.PresenterBullet
{
    public interface IViewBullet
    {
        void SetSkillsManager(PresenterBullet PresenterBullet, GameObject bullet);
        ModelBullet.ModelBullet BulletData { get; set; }
        bool Setting { get; set; }
    }
        
    public class PresenterBullet
    {
        Movement2D movement2D = new Movement2D();
        IViewBullet iViewBullet = null;
        ModelBullet.ModelBullet bulletData = new ModelBullet.ModelBullet();
        

        public PresenterBullet(GameObject bullet, ModelBullet.ModelBullet bulletData)
        {
            iViewBullet = bullet.GetComponent<ViewBullet.ViewBullet>();
            this.bulletData = bulletData;

            iViewBullet.SetSkillsManager(this, bullet);

            UpdateBulletData();

            UpdateViewBulletData();
            iViewBullet.Setting = true;
        }

        public void UpdateBulletData()
        {
            // 움직이는 방식 설정.
            bulletData.SetAndGetMovementMethod();           // 실행 시킬 코루틴(움직임)의 숫자를 정한다.
            bulletData.SetBulletStartPosition();            // Bullet의 시작 위치를 지정한다.
            bulletData.SetBulletDestinationPosition();      // Bullet의 목표 위치를 지정한다.
            bulletData.SetBulletDirectionPosition();        // 시작 위치와 목표 위치를 이용하여, 방향 벡터를 구한다.

            bulletData.Speed = 3;
        }

        // Presenter의 내부 필드 갱신
        public void UpdatePresenterBulletData()
        {
            this.bulletData = iViewBullet.BulletData;
        }

        // View의 내부 필드 갱신
        public void UpdateViewBulletData()
        {
            iViewBullet.BulletData = this.bulletData;
        }

        public void MoveToTarget()
        {
            for(int i =0; i< bulletData.MovementType.Count; i++)
            {
                if (Convert.ToBoolean(bulletData.SkillStartPositionReposition))
                {
                    bulletData.SetBulletStartPosition();
                }
                if (Convert.ToBoolean(bulletData.SkillDestinationPositionReposition))
                {
                    bulletData.SetBulletDestinationPosition();
                }

                Debug.Log("bulletData.MovementType[i] : " +  bulletData.MovementType[i]);

                switch (bulletData.MovementType[i])
                {
                    case 0:                     // 0은 가만히 있다.
                        bulletData.StartPosition = bulletData.StartPosition;
                        break;
                    case 1:                    // 1은 정해진 방향으로 등속도 운동하는 움직임이다.
                        bulletData.StartPosition = movement2D.UniformMotion(bulletData.StartPosition, bulletData.DirectionVector, bulletData.Speed);    // 시작, 방향, 속도
                        break;
                    case 2:
                        // 원 운동

                        break;
                    case 3:
                        // 싸인 운동
                        break;
                }

                UpdateViewBulletData();
            }
        }

        public void DestroyBullet()
        {
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().DestroyBullet(this);
        }
    }
}
