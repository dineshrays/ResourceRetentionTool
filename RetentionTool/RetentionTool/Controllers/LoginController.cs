﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using RetentionTool.Models;
using RetentionTool.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace RetentionTool.Controllers
{
    public class LoginController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: Login
        public ActionResult Index()
        {
            getRoleDetails();
            return View();
                //RedirectToAction("Test");
                //View();
        }
        public ActionResult loginIndex()
        {
            getRoleDetails();
            return View();
        }
        public ActionResult Test()
        {
            int a = 10;
            int b =  0;
            int c = a / b;
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserDetailsViewModel uservm)
        {
            ViewBag.Message = null;
            //  UserDetail userdetails = new UserDetail();
            //  userdetails = uservm.userDetail;
            UserDetail userResult = db.UserDetails.FirstOrDefault(a => a.Email == uservm.userDetail.Email && a.Password==uservm.userDetail.Password && a.IsActive==true && a.Role_Id==uservm.userDetail.Role_Id);
            int empRoleId = fetchdet.getDefaultEmployeeRoleId();
            int managerRoleid = fetchdet.getDefaultManagerRoleId();
            int adminRoleid = fetchdet.getDefaultAdminRoleId();
            int trainerRoleid = fetchdet.getDefaultTrainerRoleId();
          
            if (userResult!=null)
            {
                Session["RoleId"] = userResult.Role_Id;
               

             //   int internRoleid = fetchdet.getDefaultInternRoleId();
                Session["userId"] = userResult.Emp_Id;
                Session["uid"] = userResult.Id;
                PersonalInfo personalInfo = db.PersonalInfoes.Find(userResult.Emp_Id);
                Session["userpath"] = personalInfo.Image;
                if(Session["userpath"]==null)
                {
                    Session["userpath"] = "/UserImages/DefaultImage.png"; 
                        
                        //"~/UserImages/DefaultImage.png";
                }
                // Session["Notifict"] = db.Notifications.Where(a => a.User_Id == userResult.Id && a.IsActive == true && a.IsNotified == true).Count();
                //Image.PerformImageResizeAndPutOnCanvas(imgPath, filename, Convert.ToInt16(txtWidth.Text), Convert.ToInt16(txtHeight.Text), txtOutputFileName.Text.ToString() + ".jpg");
                fetchdet.getNotificationCount(userResult.Id);
                fetchdet.getDefaultPrimarySkillId();
                fetchdet.updateCircular();
                if (empRoleId==userResult.Role_Id)
                {
                    //fetchdet.getNotificationCount(userResult.Id);
                    //fetchdet.updateCircular();
                    //return RedirectToAction("")

                    // return RedirectToAction("Index", "Module", new { Area = "Admin" });
                    return RedirectToAction("Index", "EmployeeHome", new { Area="Employee"});
                    //return RedirectToAction("Index", "Training", new { Area = "Trainer" });

                    //return RedirectToAction("Index", "ProjectWorked", new { Area = "Manager" });

                }
                else if (managerRoleid == userResult.Role_Id)
                {
                 
                    return RedirectToAction("Index", "ManagerHome", new { Area = "Manager" });

                }
                else if (adminRoleid == userResult.Role_Id)
                {
                    return RedirectToAction("Index", "AdminDashboard", new { Area = "Admin" });

                }else if (trainerRoleid == userResult.Role_Id)
                {
                    return RedirectToAction("Index", "TrainerHome", new { Area = "Trainer" });
                }
                //else if (internRoleid == userResult.Role_Id)
                //{
                //    return RedirectToAction("Index", "Training", new { Area = "Trainer" });
                //}
                else
                {
                   
                    ViewBag.Message = "Invalid Username or Password or Role";
                    getRoleDetails();
                    ModelState.Clear();
                    return View();
                        //Content("<script language='javascript' type='text/javascript'>alert('Invalid Username or Password or Role');</script>");
                }

            }
            else
            {
              
                ViewBag.Message = "Invalid Username or Password";
                getRoleDetails();
                ModelState.Clear();
                
                return View();

            }

        }
        public void getRoleDetails()
        {
            fetchdet.getDefaultAdminId();
            fetchdet.addDefaultAdminName();
            fetchdet.getDefaultAdminRoleId();
            fetchdet.getDefaultEmployeeRoleId();
            fetchdet.getDefaultManagerRoleId();
            fetchdet.getDefaultTrainerRoleId();
            var val = new SelectList(db.Roles.ToList(), "Id", "Name");
            ViewData["roledetails"] = val;
            
        }
    }
}