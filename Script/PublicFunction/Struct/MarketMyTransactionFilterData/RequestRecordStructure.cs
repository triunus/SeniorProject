using System;

namespace RequestRecordData
{
    [Serializable]
    public class RequestRecordStructure
    {
        private string pageNumber;
        private string requestRecord;

        // ����Ʈ�� ["0", "", ""]�� ���� ����.
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