using Newtonsoft.Json.Linq;

using UnityEngine;

using MVP.InGameFunction;

using MVP.InGamePlayer.ModelPlayer;

namespace MVP.InGamePlayer.PresenterPlayer
{
    public interface IViewPlayer
    {
        Vector3 PlayerPosition { get; set; }
        int Level { get; set; }
        float MaxHP { get; set; }
        float CurrentHP { get; set; }
        string MoveType { get; set; }
        int Speed { get; set; }
        float MaxExperience { get; set; }
        float CurrentExperience { get; set; }

        void SetPresenter(PresenterPlayer presenterPlayer, GameObject player);
    }

    public class PresenterPlayer
    {
        Movement2D movement2D = new Movement2D();

        IViewPlayer iViewPlayer = null;
        ModelPlayer.ModelPlayer modelPlayer = new ModelPlayer.ModelPlayer();

        float BaseExperience = 100f;

        public PresenterPlayer(GameObject player, JObject playerData)
        {
            this.iViewPlayer = player.GetComponent<ViewPlayer.ViewPlayer>();
            iViewPlayer.SetPresenter(this, player);

            UpdateModelPlayerFromServer(playerData);
        }

        // �Ʒ��� ���� ���� ��, �÷��̾� ���� �κ��̴�.
        public void UpdateModelPlayerFromServer(JObject playerData)
        {
            modelPlayer.PlayerPosition = new Vector3(0, 0, 0);
            modelPlayer.Level = 1;
            modelPlayer.MaxHP = (float)playerData["maxHP"];
            modelPlayer.CurrentHP = (float)playerData["maxHP"];
            modelPlayer.MoveType = (string)playerData["moveType"];
            modelPlayer.Speed = (int)playerData["speed"];
            modelPlayer.MaxExperience = BaseExperience;
            modelPlayer.CurrentExperience = 0f;

            UpdateViewPlayer();
        }

        // Model ������Ʈ �κ�.
        public void UpdateModelPlayer()
        {
            modelPlayer.PlayerPosition = iViewPlayer.PlayerPosition;
            modelPlayer.Level = iViewPlayer.Level;
            modelPlayer.MaxHP = iViewPlayer.MaxHP;
            modelPlayer.CurrentHP = iViewPlayer.CurrentHP;
            modelPlayer.MoveType = iViewPlayer.MoveType;
            modelPlayer.Speed = iViewPlayer.Speed;
            modelPlayer.MaxExperience = BaseExperience * iViewPlayer.Level;
            modelPlayer.CurrentExperience = iViewPlayer.CurrentExperience;
        }

        // View ������Ʈ
        public void UpdateViewPlayer()
        {
            iViewPlayer.PlayerPosition = modelPlayer.PlayerPosition;
            iViewPlayer.Level = modelPlayer.Level;
            iViewPlayer.MaxHP = modelPlayer.MaxHP;
            iViewPlayer.CurrentHP = modelPlayer.CurrentHP;
            iViewPlayer.MoveType = modelPlayer.MoveType;
            iViewPlayer.Speed = modelPlayer.Speed;
            iViewPlayer.MaxExperience = BaseExperience * modelPlayer.Level;
            iViewPlayer.CurrentExperience = modelPlayer.CurrentExperience;
        }

        // View�� Update�� Ű������ �������� �ν��� ��, Movement2D Ŭ������ ����.
        public void ControllPlayerMovement(Vector3 direction)
        {
            if (iViewPlayer.MoveType.Equals("walk"))
            {
                modelPlayer.PlayerPosition = movement2D.WalkingMoveTo(modelPlayer.PlayerPosition, direction, modelPlayer.Speed);
            }

            UpdateViewPlayer();
        }

        // Player ���� ������
        public void DisplayGameOverPanel()
        {
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().DisplayGameOverPanel();
        }
    }
}