using System;

namespace RequestRecordData
{
    [Serializable]
    public class RequestRecordStructure
    {
        private string pageNumber;
        private string requestRecord;

        // 디폴트로 ["0", "", ""]을 가질 예정.
        public RequestRecordStructure(string[] searchFilter)
        {
            this.pageNumber = searchFilter[0];
            this.requestRecord = searchFilter[1];
        }

        public string PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value; }
        }

        public string RequestRecord
        {
            get { return requestRecord; }
            set { requestRecord = value; }
        }

    }
}