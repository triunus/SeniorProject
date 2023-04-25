namespace AccountLinkingInfo
{
    public class ModelAccountLinking
    {
        private string userID;
        private string userPW;
        private string requestType;
        private string phoneNumber;
        private string verificationCode;
        private string walletAddress;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public string UserPW
        {
            get { return userPW; }
            set { userPW = value; }
        }
        public string RequestType
        {
            get { return requestType; }
            set { requestType = value; }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public string VerificationCode
        {
            get { return verificationCode; }
            set { verificationCode = value; }
        }
        public string WalletAddress
        {
            get { return walletAddress; }
            set { walletAddress = value; }
        }
    }
}