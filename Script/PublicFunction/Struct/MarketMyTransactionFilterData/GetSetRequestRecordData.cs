using UnityEngine;
using RequestRecordData;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace GetSetRequestRecordInfo
{
    public class SetRequestRecordData
    {
        RequestRecordStructure recordType;
        IFormatter binaryFormatter;

        public void SetFilterData(string[] data)
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("SetFilterData 시작");
                Stream writeInfo = new FileStream("./Assets/LocalData/Game/Market/MyTransactionFilterData.txt", FileMode.Create, FileAccess.Write, FileShare.None);

                recordType = new RequestRecordStructure(data);

                binaryFormatter.Serialize(writeInfo, recordType);

                writeInfo.Close();
                Debug.Log("Write Complete");
            }
            catch (UnassignedReferenceException error01)
            {
                Debug.Log(error01);
            }
        }
    }

    public class GetRequestRecordData
    {
        RequestRecordStructure recordType;
        IFormatter binaryFormatter;

        public string[] GetFilterData()
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("GetFilterData 시작");

                Stream readInfo = new FileStream("./Assets/LocalData/Game/Market/MyTransactionFilterData.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                recordType = (RequestRecordStructure)binaryFormatter.Deserialize(readInfo);
                readInfo.Close();

                return new string[] { recordType.PageNumber, recordType.RequestRecord };
            }
            catch (FileNotFoundException error01)
            {
                Debug.Log(error01);
                return null;
            }
        }
    }
}