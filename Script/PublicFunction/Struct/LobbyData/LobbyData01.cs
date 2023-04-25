using System;
using System.Collections.Generic;

using SkillInformaion;

namespace LobbyInformaion
{
    [Serializable]
    public class LobbyData01
    {
        private string userID;
        private string userNickname;
        private string userImage;
        private string possessedCoin;
        private string possessedSpecialCoin;
        private string selecedCharacterNumber;

        private List<string> characterSkill = new List<string>();
        // { { 유니크, 스킬번호, 카운트, NFT 여부 }, { .... }, { .... } }
        // 로비에서의 content의 count와 nft여부는 유니크 정보를 서버에 넘겨서 파악할 예정.
        private List<UserSkillDataInfo> selecedSkill = new List<UserSkillDataInfo>();

        public LobbyData01(string userID, string userNickname, string userImage, string possessedCoin, string possessedSpecialCoin, string selecedCharacterNumber)
        {
             this.userID = userID;
             this.userNickname = userNickname;
             this.userImage = userImage;
             this.possessedCoin = possessedCoin;
             this.possessedSpecialCoin = possessedSpecialCoin;
             this.selecedCharacterNumber = selecedCharacterNumber;
        }

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public string UserNickname
        {
            get { return userNickname; }
            set { userNickname = value; }
        }
        public string UserImage
        {
            get { return userImage; }
            set { userImage = value; }
        }
        public string PossessedCoin
        {
            get { return possessedCoin; }
            set { possessedCoin = value; }
        }
        public string PossessedSpecialCoin
        {
            get { return possessedSpecialCoin; }
            set { possessedSpecialCoin = value; }
        }
        public string SelecedCharacterNumber
        {
            get { return selecedCharacterNumber; }
            set { selecedCharacterNumber = value; }
        }

        public void ClearCharacterSkill()
        {
            characterSkill.Clear();
        }

        public void AddCharacterSkill(string value)
        {
            this.characterSkill.Add(value);
        }

        public string GetCharacterSkill(int index)
        {
            return this.characterSkill[index];
        }

        public string[] GetCharacterSkills()
        {
            return this.characterSkill.ToArray();
        }

        public void ClearSelecedSkillNumber()
        {
            selecedSkill.Clear();
        }

        public void AddSelecedSkill(UserSkillDataInfo value)
        {
            this.selecedSkill.Add(value);
        }

        public void AddSelectedSkills(UserSkillDataInfo[] value)
        {
            this.selecedSkill.AddRange(value);
        }

        public UserSkillDataInfo GetSelecedSkill(int index)
        {
            return this.selecedSkill[index];
        }

        public UserSkillDataInfo[] GetSelectedSkills()
        {
            return this.selecedSkill.ToArray();
        }
    }
}