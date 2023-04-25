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

        // Update는 움직임을 프레임 단위로 실행된다.
        // 따라서 for문으로 각기 다른 움직임을 취하면, 움직임이 섞일 것이다.
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

        // 시간에 따라 줄어드는 것이 필요.
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