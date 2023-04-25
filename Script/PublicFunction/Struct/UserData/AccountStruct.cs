using System;

namespace AccountInfo
{
    // 직렬화를 하기 위해서는 해당 클래스가 직렬화가 가능하다는 것을 명시해 주어야 한다.
    [Serializable]      // SerializableAttribute 클래스 또는 ISerializable 인터페이스 참고
    public class AccountData
    {
        private string userID;
        private string userMACAddress;

        public AccountData(string userID, string userMACAddress)
        {
            this.userID = userID;
            this.userMACAddress = userMACAddress;
        }

        public string UserID
        {
            get{ return userID; } 
        }

        public string UserMACAddress
        {
            get { return userMACAddress; }
        }
    }
}
