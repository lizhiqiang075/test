using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;




namespace lx2.Models {

    /// <summary>
    /// JosnResult 的摘要说明
    /// </summary>
    public class JosnResult
    {
        public string result { get; set; }

        public string state { get; set; }

        public string errorMessage { get; set; }


    }

    public class JosnResult_Page
    {
        public string result { get; set; }

        public string state { get; set; }

        public string errorMessage { get; set; }

        public string TotalCount { get; set; }

        public ArrayList DataList { get; set; }


        private int _CurrentPageSize = 1;


        public int DataCount { get; set; }


        public int TotalPages
        {
            get
            {
                return DataCount / CurrentPageSize + ((DataCount % CurrentPageSize) > 0 ? 1 : 0);
            }
            set { }
        }


        public int CurrentPageIndex { get; set; }


        public int CurrentPageSize
        {
            get
            {
                return _CurrentPageSize == 0 ? 1 : _CurrentPageSize;
            }
            set
            {
                _CurrentPageSize = value;
            }
        }


        public int BeginIndex
        {
            get
            {
                return CurrentPageIndex == 0 ? 0 : CurrentPageSize * CurrentPageIndex + 1;
            }
            set { }
        }


        public int EndIndex
        {
            get
            {
                return BeginIndex + CurrentPageSize - (CurrentPageIndex == 0 ? 0 : 1);
            }
            set { }
        }

    }

}
