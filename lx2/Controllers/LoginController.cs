using lx2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace lx2.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult relogin(plogin plog)
        {

            string user = plog.User;
        string pwd = plog.Password;

        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
      
        JosnResult jr = new JosnResult();

        bool flg = new Common().Login(user, pwd);
        if (flg)
        {
            jr.result = "true";
            jr.state = "complete";
        }
        else
        {
            jr.result = "false";
            jr.state = "fail";
        }
        var jsonResult = jsonSerializer.Serialize(jr);// return what you want

        //Response.Write(jsonResult);
        return Content(jsonResult);

       
       
        }
    }
}
