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
            // �����̴� ��� ����.
            bulletData.SetAndGetMovementMethod();           // ���� ��ų �ڷ�ƾ(������)�� ���ڸ� ���Ѵ�.
            bulletData.SetBulletStartPosition();            // Bullet�� ���� ��ġ�� �����Ѵ�.
            bulletData.SetBulletDestinationPosition();      // Bullet�� ��ǥ ��ġ�� �����Ѵ�.
            bulletData.SetBulletDirectionPosition();        // ���� ��ġ�� ��ǥ ��ġ�� �̿��Ͽ�, ���� ���͸� ���Ѵ�.

            bulletData.Speed = 3;
        }

        // Presenter�� ���� �ʵ� ����
        public void UpdatePresenterBulletData()
        {
            this.bulletData = iViewBullet.BulletData;
        }

        // View�� ���� �ʵ� ����
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
                    case 0:                     // 0�� ������ �ִ�.
                        bulletData.StartPosition = bulletData.StartPosition;
                        break;
                    case 1:                    // 1�� ������ �������� ��ӵ� ��ϴ� �������̴�.
                        bulletData.StartPosition = movement2D.UniformMotion(bulletData.StartPosition, bulletData.DirectionVector, bulletData.Speed);    // ����, ����, �ӵ�
                        break;
                    case 2:
                        // �� �

                        break;
                    case 3:
                        // ���� �
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
