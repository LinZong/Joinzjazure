﻿using Joinzjazure.Data;
using Joinzjazure.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Joinzjazure.Controllers
{
    public class HomeController : Controller
    {
        public VerificationCode GetVerificationCode()
        {
            var verificationCodes = VerificationCodeXmlStore.GetAll();
            var random = new Random();
            var index = random.Next(0, verificationCodes.Count);
            var code = verificationCodes[index];
            return code;
        }

        [HttpGet]
        public ActionResult GetVerificationCodeAjax()
        {
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return Json(GetVerificationCode(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RandomEgg()
        {
            return View();
        }

        public ActionResult Index()
        {
            if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion <= 8)
            {
                return View("BrowserIncompatible");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(ApplicationForm model)
        {
            bool verCorrect;
            if (!(verCorrect = CheckVerificationCode(model.VerificationCodeId, model.VerificationCodeAnswer))
                || !ModelState.IsValid)
            {
                if (!verCorrect)
                {
                    ModelState.AddModelError("VerificationCodeAnswer", "验证码错误");
                }
                return View(model);
            }
            Task.Run(() =>
                {
                    var store = StoreFactory.GetStore();
                    store.Save(model);
                });
            return View("Succeed");
        }

        [HttpGet]
        public ActionResult VerificationCodeCheck(string verificationCodeAnswer, int verificationCodeId)
        {
            return Json(CheckVerificationCode(verificationCodeId, verificationCodeAnswer), JsonRequestBehavior.AllowGet);
        }

        private bool CheckVerificationCode(int verificationCodeId, string verificationCodeAnswer)
        {
            var query = VerificationCodeXmlStore.GetAll()
                    .Where(q => q.Id == verificationCodeId && q.Answer == verificationCodeAnswer);
            return query.ToList().Count > 0;
        }
    }
}