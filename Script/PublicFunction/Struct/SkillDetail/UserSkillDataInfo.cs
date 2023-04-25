using System;

namespace SkillInformaion
{
    [Serializable]
    public class UserSkillDataInfo
    {
        private string uniqueNumber;
        private string skillNumber;
        private string count;
        private string isNFT;

        public UserSkillDataInfo(string uniqueNumber, string skillNumber, string count, string isNFT)
        {
            this.uniqueNumber = uniqueNumber;
            this.skillNumber = skillNumber;
            this.count = count;
            this.isNFT = isNFT;
        }
        public string UniqueNumber
        {
            get { return uniqueNumber; }
            set { uniqueNumber = value; }
        }

        public string SkillNumber
        {
            get { return skillNumber; }
            set { skillNumber = value; }
        }

        public string Count
        {
            get { return count; }
            set { count = value; }
        }

        public string IsNFT
        {
            get { return isNFT; }
            set { isNFT = value; }
        }
    }

}