using UnityEngine;

// 이진 직렬화에 필요.
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
                Debug.Log("GetCheckForChanges 시작.");
                // Open은 운영 체제에서 기존 파일을 열도록 지정합니다.
                // 파일을 열 수 있는지 여부는 FileAccess 열거형에서 지정된 값에 따라 달라집니다.
                // 파일이 없으면 FileNotFoundException 예외가 throw됩니다.
                Stream readInfo = new FileStream("./Assets/LocalData/usr/CheckForChanges/CheckForChanges.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                checkForChanges = (CheckForChanges)binaryFormatter.Deserialize(readInfo);
                readInfo.Close();

                return checkForChanges.BeChanged;
            }
            catch (FileNotFoundException error01)
            {
                // 파일 없음 코드 21
                Debug.Log("파일 없음 : " + error01);
                return true;
            }
        }

        public void SetCheckForChanges(bool beChanged)
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("SetCheckForChanges 시작");
                // CreatNew는 운영 체제에서 새 파일을 만들도록 지정합니다. Write 권한이 필요합니다. 파일이 이미 있으면 IOException 예외가 throw됩니다.
                // Creat는 운영 체제에서 새 파일을 만들도록 지정합니다. Write 권한이 필요합니다. 파일이 이미 있으면 덮어씁니다. 대상이 숨긴 파일일 경우 UnauthorizedAccessException 예외가 throw됩니다.
                Stream writeInfo = new FileStream("./Assets/LocalData/usr/CheckForChanges/CheckForChanges.txt", FileMode.Create, FileAccess.Write, FileShare.None);

                // 서버에서 받은 데이터 객체에 대입.
                checkForChanges = new CheckForChanges();
                checkForChanges.BeChanged = beChanged;

                // 로컬 파일로 직렬화 저장
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
