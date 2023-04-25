using UnityEngine;
using SearchFilterData;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace GetSetSearchFilterInfo
{
    public class SetSearchFilterData
    {
        SearchFilterStructure filter;
        IFormatter binaryFormatter;

        public void SetFilterData(string[] data)
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("SetFilterData 시작");
                Stream writeInfo = new FileStream("./Assets/LocalData/Game/Market/FilterData.txt", FileMode.Create, FileAccess.Write, FileShare.None);

                filter = new SearchFilterStructure(data);

                binaryFormatter.Serialize(writeInfo, filter);

                writeInfo.Close();
                Debug.Log("Write Complete");
            }
            catch (UnassignedReferenceException error01)
            {
                Debug.Log(error01);
            }
        }
    }

    public class GetSearchFilterData
    {
        SearchFilterStructure filter;
        IFormatter binaryFormatter;

        public string[] GetFilterData()
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("GetFilterData 시작");

                Stream readInfo = new FileStream("./Assets/LocalData/Game/Market/FilterData.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                filter = (SearchFilterStructure)binaryFormatter.Deserialize(readInfo);
                readInfo.Close();

                return new string[] { filter.CountFilter, filter.StringFilter, filter.RankFilter };
            }
            catch (FileNotFoundException error01)
            {
                Debug.Log(error01);
                return null;
            }
        }
    }
}