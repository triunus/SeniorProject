using UnityEngine;

using AccountInfo;

// ���� ����ȭ�� �ʿ�.
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace GetSetAccountInfo
{
    public class GetSetAccountData
    {
        IFormatter binaryFormatter;
        AccountData accountData;
        public string[] GetAccountData()
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("GetAccountData ����.");
                // Open�� � ü������ ���� ������ ������ �����մϴ�.
                // ������ �� �� �ִ��� ���δ� FileAccess ���������� ������ ���� ���� �޶����ϴ�.
                // ������ ������ FileNotFoundException ���ܰ� throw�˴ϴ�.
                Stream readInfo = new FileStream("./Assets/LocalData/usr/AccountData/AccountData.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                accountData = (AccountData)binaryFormatter.Deserialize(readInfo);
                readInfo.Close();

                return new string[] { accountData.UserID, accountData.UserMACAddress };
            }
            catch (FileNotFoundException error01)
            {
                // ���� ���� �ڵ� 21
                Debug.Log("���� ���� : " + error01);
                return null;
            }
        }

        public AccountData GetAccountDataInAccountDataType()
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("GetAccountData ����.");
                // Open�� � ü������ ���� ������ ������ �����մϴ�.
                // ������ �� �� �ִ��� ���δ� FileAccess ���������� ������ ���� ���� �޶����ϴ�.
                // ������ ������ FileNotFoundException ���ܰ� throw�˴ϴ�.
                Stream readInfo = new FileStream("./Assets/LocalData/usr/AccountData/AccountData.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                accountData = (AccountData)binaryFormatter.Deserialize(readInfo);
                readInfo.Close();

                return accountData;
            }
            catch (FileNotFoundException error01)
            {
                // ���� ���� �ڵ� 21
                Debug.Log("���� ���� : " + error01);
                return null;
            }
        }

        public string GetAccountDataInJsonType()
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("GetAccountData ����.");

                Stream readInfo = new FileStream("./Assets/LocalData/usr/AccountData/AccountData.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                accountData = (AccountData)binaryFormatter.Deserialize(readInfo);
                readInfo.Close();

                return JsonConvert.SerializeObject(accountData, Formatting.Indented);
            }
            catch (FileNotFoundException error01)
            {
                // ���� ���� �ڵ� 21
                Debug.Log("���� ���� : " + error01);
                return null;
            }
        }

        public void SetAccountData(string[] userData)
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("SetAccountData ����");
                // CreatNew�� � ü������ �� ������ ���鵵�� �����մϴ�. Write ������ �ʿ��մϴ�. ������ �̹� ������ IOException ���ܰ� throw�˴ϴ�.
                // Creat�� � ü������ �� ������ ���鵵�� �����մϴ�. Write ������ �ʿ��մϴ�. ������ �̹� ������ ����ϴ�. ����� ���� ������ ��� UnauthorizedAccessException ���ܰ� throw�˴ϴ�.
                Stream writeInfo = new FileStream("./Assets/LocalData/usr/AccountData/AccountData.txt", FileMode.Create, FileAccess.Write, FileShare.None);

                Debug.Log("�Ű����� userData : " + userData[0] + ", " + userData[1]);
                // �������� ���� ������ ��ü�� ����.
                accountData = new AccountData(userData[0], userData[1]);
                Debug.Log("accountData : " + accountData.UserID + ", " + accountData.UserMACAddress);

                // ���� ���Ϸ� ����ȭ ����
                binaryFormatter.Serialize(writeInfo, accountData);

                writeInfo.Close();
                Debug.Log("Write Complete");
            }
            catch(UnassignedReferenceException error01)
            {
                Debug.Log(error01);
            }
        }

        public void DeleteAccountData()
        {
            Debug.Log("DeleteAccountData ����");

            FileInfo willBeDeletedFile = new FileInfo("./Assets/LocalData/usr/AccountData/AccountData.txt");

            try
            {
                willBeDeletedFile.Delete();
            }
            catch(IOException error01)
            {
                Debug.Log(error01);
            }

            Debug.Log("DeleteAccount Complete");
        }
    }
}
