using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace MVP.CheckingIsPlaying.ModelIsPlayingCheck
{
    [Serializable]
    public class IsPlayingCheck
    {
        private bool isPlaying;

        public IsPlayingCheck(bool isPlaying)
        {
            this.isPlaying = isPlaying;
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
            set { isPlaying = value; }
        }
    }

    public class SerializationIsPlayingCheck
    {
        IFormatter binaryFormatter;

        public IsPlayingCheck GetInGameRecord()
        {
            binaryFormatter = new BinaryFormatter();
            IsPlayingCheck isPlayingCheck;

            try
            {
                Stream readInfo = new FileStream("./Assets/LocalData/InGame/CheckingIsPlaying.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                isPlayingCheck = (IsPlayingCheck)binaryFormatter.Deserialize(readInfo);
                readInfo.Close();

                return isPlayingCheck;
            }
            catch (FileNotFoundException error01)
            {
                // 파일 없음 코드 21
                Console.WriteLine(error01);
                return null;
            }
        }

        public void SetInGameRecord(IsPlayingCheck isPlayingCheck)
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Stream writeInfo = new FileStream("./Assets/LocalData/InGame/CheckingIsPlaying.txt", FileMode.Create, FileAccess.Write, FileShare.None);

                binaryFormatter.Serialize(writeInfo, isPlayingCheck);

                writeInfo.Close();
            }
            catch (UnauthorizedAccessException error01)
            {
                // error
                Console.WriteLine(error01);
            }
        }
    }
}