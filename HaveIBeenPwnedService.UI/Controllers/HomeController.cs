using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PwnedPassword.Models;
using HaveIBeenPwned.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace PwnedPassword.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            var service = GetSevice();
            var model = GetModel();
            var userList = GetUserList();
        }

        private HaveIBeenPwnedService GetSevice()
        {
            HaveIBeenPwnedService service = AppDomain.CurrentDomain.GetData("Service") as HaveIBeenPwnedService;
            if (service == null)
            {
                service = GetHaveIBeenPwnedService();
                AppDomain.CurrentDomain.SetData("Service", service);
            }
            return service;
        }

        private PwnedPasswordsModel GetModel()
        {
            PwnedPasswordsModel model = AppDomain.CurrentDomain.GetData("PwnedPasswordsModel") as PwnedPasswordsModel;
            if (model == null)
            {
                model = new PwnedPasswordsModel();
                model.Password = "password";
                model.Message = "";
                model.Frequency = 0;
                model.Name = "user name";
                SetModel(model);
            }

            return model;
        }

        private void SetModel(PwnedPasswordsModel model)
        {
            AppDomain.CurrentDomain.SetData("PwnedPasswordsModel", model);
        }

        private List<string> GetUserList()
        {
            List<string> userList = AppDomain.CurrentDomain.GetData("UserList") as List<string>;
            if (userList == null)
            {
                userList = new List<string>();
                AppDomain.CurrentDomain.SetData("UserList", userList);
            }
            return userList;
        }

        private void SetUserList(string userName)
        {
            var userList = GetUserList();
            if (!userList.Contains(userName))
            {
                userList.Add(userName);
                AppDomain.CurrentDomain.SetData("UserList", userList);
            }
        }

        private HaveIBeenPwnedService GetHaveIBeenPwnedService()
        {
            var services = new ServiceCollection();
            services.AddPwnedPasswordHttpClient();
            var provider = services.BuildServiceProvider();

            //all called in one method to easily enforce timout
            var service = new HaveIBeenPwnedService(
                provider.GetService<IHttpClientFactory>().CreateClient(HaveIBeenPwnedService.DefaultName),
                MockHelpers.StubLogger<HaveIBeenPwnedService>());

            return service;
        }

        public IActionResult Index()
        {
            var model = GetModel();
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Have I Been Pwned Description Page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Have I Been Pwned Contact Page.";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CheckPwnedPassword(PwnedPasswordsModel model)
        {
            if (ModelState.IsValid)
            {
                var service = GetSevice();
                (bool isPwned, int frequency) response = await service.HasPasswordBeenPwned(model.Password);

                model.Frequency = response.frequency;
                model.IsPwned = response.isPwned;

                var userList = GetUserList();
                if (userList.Contains(model.Name))
                {
                    model.Message = string.Format("Welcome back {0} !", model.Name);
                }else
                {
                    model.Message = string.Format("Welcome new user: {0} !", model.Name);
                    SetUserList(model.Name);
                }

                SetModel(model);
            }
            return RedirectToAction("Index");
        }

    }
}
