using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using MVP.InGameEnemy.ModelEnemy;
using MVP.InGameEnemy.PresenterEnemy;

namespace MVP.InGameEnemy.ViewEnemy
{
    // 상속 받은 것들을 정의
    public partial class ViewEnemy : IViewEnemy
    {
        private int maxHP;
        private int currentHP;
        private float damage;
        private int speed;
        private float experience;
        private int score;
        private int coin;

        public void SetEnemy(PresenterEnemy.PresenterEnemy presenterEnemy, GameObject enemy)
        {
            this.presenterEnemy = presenterEnemy;
            this.enemy = enemy;
        }

        public Vector3 EnemyPosition
        {
            get { return enemy.transform.position; }
            set { enemy.transform.position = value; }
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
    }

    // 해당 클래스에서 새로 선언한 것을 정의.
    partial class ViewEnemy : MonoBehaviour
    {
        private PresenterEnemy.PresenterEnemy presenterEnemy;

        private GameObject enemy;
        private bool moveable = true;

        private void Update()
        {
//            if (moveable)
            presenterEnemy.ChasingPlayer();

            if (presenterEnemy.GetPlayerPosition().x > EnemyPosition.x)
            {
                enemy.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                enemy.GetComponent<SpriteRenderer>().flipX = true;
            }

            moveable = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("InGameBulletProjectile"))
            {
                CurrentHP -= (int)collision.GetComponent<InGameSkills.ViewBullet.ViewBullet>().BulletData.SkillDamage;
                Debug.Log("EnemyHP : " + CurrentHP);

                StopCoroutine("HitColorAnimation");
                StartCoroutine("HitColorAnimation");

                if (CurrentHP <= 0)
                {
                    // 죽는 이펙트
                    Instantiate(Resources.Load<GameObject>("Prefab/InGame/Effect/ParticleExplosion"), EnemyPosition, Quaternion.identity);

                    // GameManager의 score와 coin을 증가시킨다.
                    GameObject.FindWithTag("GameManager").GetComponent<GameManager>().UpdateGameDataInEnemyDie(Score, Coin);
                    // Player의 experience 증가시킨다. + 이후 레벨업 작업도 기술되어 있다.
                    GameObject.FindWithTag("Player").GetComponent<MVP.InGamePlayer.ViewPlayer.ViewPlayer>().UpdateExperienceInEnemyDie(Experience);

                    // Enemy 삭제 시퀀스
                    presenterEnemy.DyingEnemy();
                    Destroy(enemy);
                }
            }
            
/*            // OnTriggerEnter2D 다음에 Update 함수가 실행된다.
            // 따라서 이곳에서, Enmey의 움직임 제어 bool 값을 정하면, Update에서 움직임 관련 함수의 수행 여부를 제어할 수 있다.
            if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
            {
                Vector3 playerPosition = GameObject.FindWithTag("Player").GetComponent<MVP.InGamePlayer.ViewPlayer.ViewPlayer>().PlayerPosition;

                if (Vector3.Distance(enemy.transform.position, playerPosition) > Vector3.Distance(collision.transform.position, playerPosition))
                {
                    moveable = false;
                }
                else
                {
                    collision.GetComponent<ViewEnemy>().moveable = false;
                }
            }*/
        }

        /*        public void OnHitEventFromBullet(float bulletDamage)
                {
                    CurrentHP -= (int)bulletDamage;

                    StopCoroutine("HitColorAnimation");
                    StartCoroutine("HitColorAnimation");s

                    if (CurrentHP <= 0)
                    {
                        Debug.Log("Enemy die");
                        // 죽는 이펙트
                        Instantiate(Resources.Load<GameObject>("Prefab/InGame/Effect/ParticleExplosion"), EnemyPosition, Quaternion.identity);

                        // GameManager의 score와 coin을 증가시킨다.
                        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().UpdateGameDataInEnemyDie(Score, Coin);
                        // Player의 experience 증가시킨다. + 이후 레벨업 작업도 기술되어 있다.
                        GameObject.FindWithTag("Player").GetComponent<MVP.InGamePlayer.ViewPlayer.ViewPlayer>().UpdateExperienceInEnemyDie(Experience);

                        // Enemy 삭제 시퀀스
                        presenterEnemy.DyingEnemy();
                        Destroy(enemy);
                    }
                }*/

        private IEnumerator HitColorAnimation()
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.red;
            enemy.layer = 9;

            yield return new WaitForSeconds(0.1f);

            enemy.GetComponent<SpriteRenderer>().color = Color.white;
            enemy.layer = 8;

        }
    }
}