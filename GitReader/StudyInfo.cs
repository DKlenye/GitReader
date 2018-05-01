using System;
using System.Collections.Generic;

namespace GitReader
{
    [Serializable]
    public class StudyInfo
    {
        public string StudyName { get; set; }
        public IEnumerable<string> Modules { get; set; }
    }
}