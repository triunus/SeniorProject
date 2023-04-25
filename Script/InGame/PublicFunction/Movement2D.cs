using UnityEngine;

namespace MVP.InGameFunction
{
    public class Movement2D
    {
        public Vector3 WalkingMoveTo(Vector3 position, Vector3 direction, int speed)
        {
//            Debug.Log(position.X + ", " + position.Y + ", " + Time.deltaTime);
            return new Vector3(position.x + direction.x * Time.deltaTime * speed, position.y + direction.y * Time.deltaTime * speed, 0);
        }

        public Vector3 FlashingMoveTo(Vector3 position, Vector3 direction, int speed)
        {
//            Debug.Log(position.X + ", " + position.Y + ", " + Time.deltaTime);
            return new Vector3(position.x + direction.x * Time.deltaTime * speed, position.y + direction.y * Time.deltaTime * speed, 0);
        }

        public Vector3 MovementChasing(Vector3 currentPosition, Vector3 directionPosition, float speed)
        {
            return new Vector3(Mathf.MoveTowards(currentPosition.x, directionPosition.x, speed * Time.deltaTime), Mathf.MoveTowards(currentPosition.y, directionPosition.y, speed * Time.deltaTime), 0);
        }

        public Vector3 UniformMotion(Vector3 currentPosition, Vector3 directionVector, float speed)
        {
            return new Vector3(currentPosition.x + directionVector.x * speed * Time.deltaTime, currentPosition.y + directionVector.y * speed * Time.deltaTime, 0);
        }
    }

}