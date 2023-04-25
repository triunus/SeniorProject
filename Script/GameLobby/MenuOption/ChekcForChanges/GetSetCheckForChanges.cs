using UnityEngine;

// ���� ����ȭ�� �ʿ�.
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using CheckForChangesInfo;


namespace GetSetCheckForChangesInfo
{
    public class GetSetCheckForChanges
    {
        IFormatter binaryFormatter;
        CheckForChanges checkForChanges;

        public bool GetCheckForChanges()
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("GetCheckForChanges ����.");
                // Open�� � ü������ ���� ������ ������ �����մϴ�.
                // ������ �� �� �ִ��� ���δ� FileAccess ���������� ������ ���� ���� �޶����ϴ�.
                // ������ ������ FileNotFoundException ���ܰ� throw�˴ϴ�.
                Stream readInfo = new FileStream("./Assets/LocalData/usr/CheckForChanges/CheckForChanges.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                checkForChanges = (CheckForChanges)binaryFormatter.Deserialize(readInfo);
                readInfo.Close();

                return checkForChanges.BeChanged;
            }
            catch (FileNotFoundException error01)
            {
                // ���� ���� �ڵ� 21
                Debug.Log("���� ���� : " + error01);
                return true;
            }
        }

        public void SetCheckForChanges(bool beChanged)
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("SetCheckForChanges ����");
                // CreatNew�� � ü������ �� ������ ���鵵�� �����մϴ�. Write ������ �ʿ��մϴ�. ������ �̹� ������ IOException ���ܰ� throw�˴ϴ�.
                // Creat�� � ü������ �� ������ ���鵵�� �����մϴ�. Write ������ �ʿ��մϴ�. ������ �̹� ������ ����ϴ�. ����� ���� ������ ��� UnauthorizedAccessException ���ܰ� throw�˴ϴ�.
                Stream writeInfo = new FileStream("./Assets/LocalData/usr/CheckForChanges/CheckForChanges.txt", FileMode.Create, FileAccess.Write, FileShare.None);

                // �������� ���� ������ ��ü�� ����.
                checkForChanges = new CheckForChanges();
                checkForChanges.BeChanged = beChanged;

                // ���� ���Ϸ� ����ȭ ����
                binaryFormatter.Serialize(writeInfo, checkForChanges);

                writeInfo.Close();
                Debug.Log("Write Complete");
            }
            catch (UnassignedReferenceException error01)
            {
                Debug.Log(error01);
            }
        }
    }
}
