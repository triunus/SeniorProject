using System;

namespace SearchFilterData
{
    [Serializable]
    public class SearchFilterStructure
    {
        private string countFilter;
        private string stringFilter;
        private string rankFilter;

        // 디폴트로 ["0", "", ""]을 가질 예정.
        public SearchFilterStructure(string[] searchFilter)
        {
            this.countFilter = searchFilter[0];
            this.stringFilter = searchFilter[1];
            this.rankFilter = searchFilter[2];
        }

        public string CountFilter
        {
            get { return countFilter; }
            set { countFilter = value; }
        }

        public string StringFilter
        {
            get { return stringFilter; }
            set { stringFilter = value; }
        }

        public string RankFilter
        {
            get { return rankFilter; }
            set { rankFilter = value; }
        }

    }
}