using UnityEngine;

namespace MVP.InGamePlayer.ModelPlayer
{
    public class ModelPlayer
    {
        private Vector3 playerPosition;
        private int level;
        private float maxHP;
        private float currentHP;
        private string moveType;
        private int speed;
        private float maxExperience;
        private float currentExperience;

        public Vector3 PlayerPosition
        {
            get { return playerPosition; }
            set { playerPosition = value; }
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

}