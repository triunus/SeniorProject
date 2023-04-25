using System;
using UnityEngine;

using ServerInfomation;
using System.Net;

/*using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;*/

using System.IO;
using System.Text;

namespace ConnectServer
{
    public class LoginRegisterRequest
    {
        ServerInfo server = new ServerInfo();
        string responseFromServer;
        string url;

        public string Register(string userMACAddress)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/register_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            Debug.Log(userMACAddress);
            string requestData = userMACAddress;
//            Debug.Log("requestData : " + requestData);
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();

                Console.ReadLine();

                Debug.Log("responseFromServer : " + responseFromServer);
            }

            response.Close();

            return responseFromServer;
        }

        public string Login(string userDataInLocal)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/login_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = userDataInLocal;
//            Debug.Log(requestData);
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();

                Debug.Log(responseFromServer);
            }

            response.Close();

            return responseFromServer;
        }

        public string Delete(string userDataInLocal)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/delete_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = userDataInLocal;
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();

                Debug.Log(responseFromServer);
            }

            response.Close();

            return responseFromServer;
        }

        /*public string GetURL()
        {
            IFormatter formatter = new BinaryFormatter();
            ServerInfo server;

            string serverInfo = null;

            try
            {
                // Open�� � ü������ ���� ������ ������ �����մϴ�. ������ �� �� �ִ��� ���δ� FileAccess ���������� ������ ���� ���� �޶����ϴ�.
                // ������ ������ FileNotFoundException ���ܰ� throw�˴ϴ�.
                Stream readInfo = new FileStream("./Assets/srv/Server/ServerData.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                server = (ServerInfo)formatter.Deserialize(readInfo);

                readInfo.Close();
                serverInfo = server.GetURI();

                Console.WriteLine(server.GetURI());
                Console.ReadLine();
            }
            catch (FileNotFoundException error01)
            {
                Console.WriteLine("Error : " + error01);
                Console.ReadLine();
            }

            return serverInfo;
        }*/


    }

    public class RequestUserData
    {
        ServerInfo server = new ServerInfo();
        string responseFromServer;
        string url;

        // �κ񿡼� �������� ��û
        public string RequestGameLobbyUserInfomaion(string userData)
        {
            // GetURL�Լ��� �̿��Ͽ� �α��ΰ� ���� ���.
            // �ڿ� �ٴ� ���� �����Ͽ�, �������� �����ϴ� �ൿ?�� ����.
            url = string.Format("{0}{1}", server.GetURL(), "/RequestGameLobbyUserInformaion_test");

            WebRequest request = WebRequest.Create(url);

            // �� ���� �ڵ忡 ���� ������, CShapStudy/ChsapStudy08/Program08.cs�� ��������.
            request.Method = "POST";

            // data�� key=value&key=value �������� �����Ѵ�.
            string requestData = userData;
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            // �����ϰ��� �ϴ� ������ Request Stream�� ����.
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // ������ ��û �������� ���� ��û.
            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                // System.IO.StreamReader Class : Ư�� ���ڵ��� ����Ʈ ��Ʈ������ ���ڸ� �д� TextReader �� �����մϴ�.
                // �Ű������� ���� ��Ʈ���� �н��ϴ�.
                StreamReader reader = new StreamReader(dataStream);

                // ���� ��ġ���� ��Ʈ�� �������� ��� ���ڸ� �д´�.
                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            Debug.Log(responseFromServer);

            return responseFromServer;
        }



        // ĳ���� �޴����� ������ ������ ĳ���� ����Ʈ ��û
        public string RequestPossessedCharacterList(string[] UserDataInLocal)
        {
            // GetURL�Լ��� �̿��Ͽ� �α��ΰ� ���� ���.
            // �ڿ� �ٴ� ���� �����Ͽ�, �������� �����ϴ� �ൿ?�� ����.
            url = string.Format("{0}{1}", server.GetURL(), "/RequestCharacterList_test");

            WebRequest request = WebRequest.Create(url);

            // �� ���� �ڵ忡 ���� ������, CShapStudy/ChsapStudy08/Program08.cs�� ��������.
            request.Method = "POST";

            // data�� key=value&key=value �������� �����Ѵ�.
            string requestData = "userID=" + UserDataInLocal[0] + "&userPW=" + UserDataInLocal[1];
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            // �����ϰ��� �ϴ� ������ Request Stream�� ����.
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // ������ ��û �������� ���� ��û.
            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                // System.IO.StreamReader Class : Ư�� ���ڵ��� ����Ʈ ��Ʈ������ ���ڸ� �д� TextReader �� �����մϴ�.
                // �Ű������� ���� ��Ʈ���� �н��ϴ�.
                StreamReader reader = new StreamReader(dataStream);

                // ���� ��ġ���� ��Ʈ�� �������� ��� ���ڸ� �д´�.
                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }



        // �κ��丮 �޴����� ������ ������ ��ų ����Ʈ ��û.
        public string RequestPossessedSkillInfo(string UserDataInLocal)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestInventoryList_test");

            WebRequest request = WebRequest.Create(url);

            // �� ���� �ڵ忡 ���� ������, CShapStudy/ChsapStudy08/Program08.cs�� ��������.
            request.Method = "POST";

            // data�� key=value&key=value �������� �����Ѵ�.
            string requestData = UserDataInLocal;
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            // �����ϰ��� �ϴ� ������ Request Stream�� ����.
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // ������ ��û �������� ���� ��û.
            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                // System.IO.StreamReader Class : Ư�� ���ڵ��� ����Ʈ ��Ʈ������ ���ڸ� �д� TextReader �� �����մϴ�.
                // �Ű������� ���� ��Ʈ���� �н��ϴ�.
                StreamReader reader = new StreamReader(dataStream);

                // ���� ��ġ���� ��Ʈ�� �������� ��� ���ڸ� �д´�.
                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }


        // ����
        public string RequestVenderProductInfo(string[] UserDataInLocal)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestVenderProductList_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = "userID=" + UserDataInLocal[0] + "&userPW=" + UserDataInLocal[1] + "&requestTpye=" + UserDataInLocal[2];
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestVenderProductBuy(string[] buyProductData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestVenderBuy_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = "userID=" + buyProductData[0] + "&userPW=" + buyProductData[1] + "&productNumber=" + buyProductData[2];
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;

        }



        // �ŷ���
        public string RequestMarketAccessibility_test(string UserDataInLocal)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestMarketAccessibility_test");

            WebRequest request = WebRequest.Create(url);

            // �� ���� �ڵ忡 ���� ������, CShapStudy/ChsapStudy08/Program08.cs�� ��������.
            request.Method = "POST";

            // data�� key=value&key=value �������� �����Ѵ�.
            string requestData = UserDataInLocal;
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            // �����ϰ��� �ϴ� ������ Request Stream�� ����.
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // ������ ��û �������� ���� ��û.
            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                // System.IO.StreamReader Class : Ư�� ���ڵ��� ����Ʈ ��Ʈ������ ���ڸ� �д� TextReader �� �����մϴ�.
                // �Ű������� ���� ��Ʈ���� �н��ϴ�.
                StreamReader reader = new StreamReader(dataStream);

                // ���� ��ġ���� ��Ʈ�� �������� ��� ���ڸ� �д´�.
                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }
        public string RequestProductsInfo(string[] filterData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestMarketList_test");

            WebRequest request = WebRequest.Create(url);

            // �� ���� �ڵ忡 ���� ������, CShapStudy/ChsapStudy08/Program08.cs�� ��������.
            request.Method = "POST";

            // data�� key=value&key=value �������� �����Ѵ�.
            string requestData = "pageNumber=" + filterData[0] + "&searchString=" + filterData[1] + "&skillRank=" + filterData[2];
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            // �����ϰ��� �ϴ� ������ Request Stream�� ����.
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // ������ ��û �������� ���� ��û.
            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                // System.IO.StreamReader Class : Ư�� ���ڵ��� ����Ʈ ��Ʈ������ ���ڸ� �д� TextReader �� �����մϴ�.
                // �Ű������� ���� ��Ʈ���� �н��ϴ�.
                StreamReader reader = new StreamReader(dataStream);

                // ���� ��ġ���� ��Ʈ�� �������� ��� ���ڸ� �д´�.
                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

//            Debug.Log("responseFromServer : " + responseFromServer);
            return responseFromServer;
        }

        public string RequestProductEnrollPrice()
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestMarketEnrollPrice_test");

            WebRequest request = WebRequest.Create(url);

            // �� ���� �ڵ忡 ���� ������, CShapStudy/ChsapStudy08/Program08.cs�� ��������.
            request.Method = "POST";

            // Miniting�� �ϴµ� �ʿ��� ������ ������ �������� ���̶�, �Է� �����Ͱ� ���� �������� �ʴ´�.
            string requestData = "";
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestProductEnroll(string[] enrollSkillData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestMarketEnroll_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = "userID=" + enrollSkillData[0] + "&userPW=" + enrollSkillData[1] + "&skillUniqueNumber=" + enrollSkillData[2] + "&skillSalePrice=" + enrollSkillData[3];
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestProductBuy(string[] buyProductData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestMarketBuy_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = "userID=" + buyProductData[0] + "&userPW=" + buyProductData[1] + "&registrationNumber=" + buyProductData[2];
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestTransactionHistorysInfo(string[] historyCategory)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestTransactionRecord_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = "userID=" + historyCategory[0] + "&userPW=" + historyCategory[1] + "&pageNumber=" + historyCategory[2] + "&historyCategory=" + historyCategory[3];
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestTransactionCancel(string[] cancelProductData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestTransactionCancel_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = "userID=" + cancelProductData[0] + "&userPW=" + cancelProductData[1] + "&registrationNumber=" + cancelProductData[2];
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }



        // 옵션
        public string RequestVerificationCode(string requestData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestVerificationCode_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

//            string requestData = "";
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestWhetherAuthenticationSucceeded(string requestData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestWhetherAuthenticationSucceeded_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

//            string requestData = "";
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestRegisterWalletAddress(string requestData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestRegisterWalletAddress_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestModifyUserNickname(string[] modifyInfo)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestModifyUserNickname_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = "userID=" + modifyInfo[0] + "&userPW=" + modifyInfo[1] + "&nickname=" + modifyInfo[2];
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }




        // In Game
        public string RequestSettingGameData(string startData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestSettingGameData_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = startData;
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestCharacterData(string recordData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestCharacterData_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = recordData;
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestEnemyData()
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestEnemyData_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = "";
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestSkillsData(string recordData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestSkillsData_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = recordData;
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }

        public string RequestGameResult(string gameResultData)
        {
            url = string.Format("{0}{1}", server.GetURL(), "/RequestGameResult_test");

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string requestData = gameResultData;
            byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            Debug.Log(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }
    }

}