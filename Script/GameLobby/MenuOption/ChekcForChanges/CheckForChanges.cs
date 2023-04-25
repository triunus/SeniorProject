using System;

namespace CheckForChangesInfo
{
    [Serializable]
    public class CheckForChanges
    {
        private bool beChanged = false;

        public bool BeChanged
        {
            get { return beChanged; }
            set { beChanged = value; }
        }
    }
}


