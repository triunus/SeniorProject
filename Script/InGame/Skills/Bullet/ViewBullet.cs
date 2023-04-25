using System;
using System.Collections;
using System.Collections.Generic
    ;
using UnityEngine;
using UnityEngine.UI;

using MVP.InGameSkills.ModelSkills;
using MVP.InGameSkills.PresenterBullet;

using MVP.InGameSkills.ModelBullet;

namespace MVP.InGameSkills.ViewBullet
{
    public partial class ViewBullet : IViewBullet
    {
        public void SetSkillsManager(PresenterBullet.PresenterBullet PresenterBullet, GameObject bullet)
        {
            this.PresenterBullet = PresenterBullet;
            this.bullet = bullet;
        }

        public ModelBullet.ModelBullet BulletData
        {
            get { return bulletData; }
            set
            { 
                bulletData = value;
                bullet.transform.position = bulletData.StartPosition;
            }
        }
        public bool Setting
        {
            get { return setting; }
            set { setting = value; }
        }

    }

    partial class ViewBullet : MonoBehaviour
    {
        private PresenterBullet.PresenterBullet PresenterBullet;
        private ModelBullet.ModelBullet bulletData = new ModelBullet.ModelBullet();
        private GameObject bullet;
        private bool setting = false;

        // Update�� �������� ������ ������ ����ȴ�.
        // ���� for������ ���� �ٸ� �������� ���ϸ�, �������� ���� ���̴�.
        private void Update()
        {
            if (setting)
            {
                DestroyBulletForTiem();

                PresenterBullet.MoveToTarget();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
            {
//                collision.GetComponent<MVP.InGameEnemy.ViewEnemy.ViewEnemy>().OnHitEventFromBullet(BulletData.SkillDamage);
                bulletData.SkillDestroyCount--;

                if (bulletData.SkillDestroyCount <= 0)
                {
                    PresenterBullet.DestroyBullet();
                    Destroy(bullet);
                }
            }
        }

        private IEnumerator DestroySequence()
        {
            yield return new WaitForSecondsRealtime(bulletData.SkillDestroyTime);

            PresenterBullet.DestroyBullet();
            Destroy(bullet);
        }

        // �ð��� ���� �پ��� ���� �ʿ�.
        private void DestroyBulletForTiem()
        {
            if (bulletData.SkillDestroyTime <= 0)
            {
                PresenterBullet.DestroyBullet();
                Destroy(bullet);
            }

            bulletData.SkillDestroyTime = bulletData.SkillDestroyTime - Time.deltaTime;
        }

    }

}