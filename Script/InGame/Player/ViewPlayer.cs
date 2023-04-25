using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MVP.InGamePlayer.PresenterPlayer;
using MVP.InGamePlayer.ModelPlayer;

namespace MVP.InGamePlayer.ViewPlayer
{
    public partial class ViewPlayer : IViewPlayer
    {
        private PresenterPlayer.PresenterPlayer presenterPlayer;
        Dictionary<KeyCode, Action> keyDirctionary;

        private GameObject player;
        private int level;
        private float maxHP;
        private float currentHP;
        private int speed;
        private string moveType;
        private float maxExperience;
        private float currentExperience;

        public void SetPresenter(PresenterPlayer.PresenterPlayer presenterPlayer, GameObject player)
        {
            this.presenterPlayer = presenterPlayer;
            this.player = player;
        }

        public Vector3 PlayerPosition
        {
            get { return player.transform.position; }
            set { player.transform.position = value; }
        }
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        public float MaxHP
        {
            get { return maxHP; }
            set { maxHP = value; }
        }
        public float CurrentHP
        {
            get { return currentHP; }
            set { currentHP = value; }
        }
        public string MoveType
        {
            get { return moveType; }
            set { moveType = value; }
        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float MaxExperience
        {
            get { return maxExperience; }
            set { maxExperience = value; }
        }
        public float CurrentExperience
        {
            get { return currentExperience; }
            set { currentExperience = value; }
        }
    }

    partial class ViewPlayer : MonoBehaviour
    {
        private Slider sliderExe;
        private Slider playerHP;
        private Animator animator;
        // 애니메이션클립은, animator가 갖는 애니메이션들의 배열을 갖고 있다...?
        // 따라서 특정 애니메이션이 끝났을 때, 새로운 애니메이션 이벤트를 지정하여 -> 원하는 이벤트를 실행
        private AnimationClip animationClip;
        private AnimationEvent animationEvent;

        private bool isDie = false;

        private void Start()
        {
            sliderExe = GameObject.FindWithTag("InGameExeSlider").GetComponent<Slider>();
            playerHP = GameObject.FindWithTag("InGamePlayerHPSlider").GetComponent<Slider>();
            animator = player.GetComponent<Animator>();
            animationClip = animator.runtimeAnimatorController.animationClips[1];
            animationEvent = new AnimationEvent();

            playerHP.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(player.transform.position);
            playerHP.transform.position = playerHP.transform.position + new Vector3(0, -50, 0);

            keyDirctionary = new Dictionary<KeyCode, Action>
            {
                { KeyCode.W, KeyAction_W },
                { KeyCode.S, KeyAction_S },
                { KeyCode.A, KeyAction_A },
                { KeyCode.D, KeyAction_D },
            };
        }

        // Update는 deltaTime과 같이 FPS rate에 기반한 가변적인 프레임을 기반으로 작동한다.
        // 버튼이벤트와 같이 사용자의 입력과 관련된 이벤트는 업데이트에 기록한다.
        public void Update()
        {
            if (Input.anyKey && !isDie)
            {
                foreach (var key in keyDirctionary)
                {
                    if (Input.GetKey(key.Key))
                    {
                        key.Value();
                    }
                }

            }
                
            // 경험치 부분.
            sliderExe.value = CurrentExperience / MaxExperience;
            playerHP.value = CurrentHP / MaxHP;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A))
            {
                player.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                player.GetComponent<SpriteRenderer>().flipX = false;
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
            {
                OnDamaged(collision.GetComponent<InGameEnemy.ViewEnemy.ViewEnemy>().Damage);

                if(CurrentHP <= 0)
                {
                    Debug.Log("i'm dieing");
                    
                    isDie = true;
                    animator.SetBool("Die", isDie);

                    animationEvent.time = animationClip.length;
                    animationEvent.functionName = "AfterDieEvent";

                    animationClip.AddEvent(animationEvent);

                    // 죽는 애니메이션 있으면 한 후~
                    /*                    GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PlayerDstroy();
                                        Destroy(player);
                    */
                }

                presenterPlayer.UpdateModelPlayer();
            }
        }

        public void AfterDieEvent()
        {
            Debug.Log("---");

            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PauseGame();

            presenterPlayer.DisplayGameOverPanel();
        }

        public void UpdateExperienceInEnemyDie(float experience)
        {
            CurrentExperience = CurrentExperience + experience;

            if (CurrentExperience >= MaxExperience)
            {
                Level++;
                CurrentHP = MaxHP;
                CurrentExperience = 0;

                // SkillManager에 가서 LevelUpUI 출력
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PauseGame();
                GameObject.FindWithTag("SkillsManager").GetComponent<MVP.InGameSkills.ViewSkills.ViewSkillsManager>().LevelUPEvent();
            }

            presenterPlayer.UpdateModelPlayer();
        }

        public void OnDamaged(float damage)
        {
            CurrentHP -= damage;

            StopCoroutine("HitColorAnimation");
            StartCoroutine("HitColorAnimation");
        }
        private IEnumerator HitColorAnimation()
        {
            player.GetComponent<SpriteRenderer>().color = Color.red;

            yield return new WaitForSeconds(0.1f);

            player.GetComponent<SpriteRenderer>().color = Color.white;
        }

        public void KeyAction_W()
        {
            presenterPlayer.ControllPlayerMovement(new Vector3(0, 1, 0));
        }
        public void KeyAction_S()
        {
            presenterPlayer.ControllPlayerMovement(new Vector3(0, -1, 0));
        }
        public void KeyAction_A()
        {
            presenterPlayer.ControllPlayerMovement(new Vector3(-1, 0, 0));
        }
        public void KeyAction_D()
        {
            presenterPlayer.ControllPlayerMovement(new Vector3(1, 0, 0));
        }
    }
}