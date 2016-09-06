using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace lx2.Models
{
    public class InFo2_Modle
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string PasswordSure { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime BrithDay { get; set; }
        public string Phoen { get; set; }
        public DateTime Enter { get; set; }
        public DateTime EnterEnd { get; set; }
        public DateTime official { get; set; }
        public DateTime officialEnd { get; set; }

        public string Enter_Str { get; set; }
        public string EnterEnd_Str { get; set; }
        public string official_Str { get; set; }
        public string officialEnd_Str { get; set; }



        public DateTime Departure { get; set; }

        public string Department { get; set; }
        public string position { get; set; }

        public string CardID { get; set; }
        public string LandLine { get; set; }
        public string BankId { get; set; }
        public string Educational { get; set; }
        public string ClubLevel { get; set; }
        public string Professionals { get; set; }
        public string HomeTown { get; set; }

        public string City { get; set; }
        public string Area { get; set; }
        public List<string> areaList { get; set; }

        public int MoneyLevel { get; set; }
        public string Dress { get; set; }
        public string AccountLocation { get; set; }

        public int AccessLevel { get; set; }
        public string LoginState { get; set; }
        public int Age { get; set; }
        public int WorkTime { get; set; }




    }
    public class plogin {
        public string User { get; set; }
        public string Password { get; set; }

    }
}