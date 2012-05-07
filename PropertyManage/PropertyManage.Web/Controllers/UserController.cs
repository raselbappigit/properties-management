using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PropertyManage.Web;
using PropertyManage.Service;
using PropertyManage.Domain;
using System.Web.Security;
using PropertyManage.Web;

namespace PropertyManage.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ISecurityService _securityService;

        public UserController(ISecurityService securityService)
        {
            this._securityService = securityService;
        }

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        // for display datatable
        public ActionResult GetUsers(DataTableParamModel param)
        {
            var users = _securityService.GetUsers().ToList();
            IEnumerable<User> filteredUsers;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredUsers = users.Where(usr => usr.UserName.Contains(param.sSearch)).ToList();
            }
            else
            {
                filteredUsers = users;
            }

            var viewOdjects = filteredUsers.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from usrMdl in viewOdjects
                         select new[] { Convert.ToString(usrMdl.UserName), Convert.ToString(usrMdl.UserName), Convert.ToString(usrMdl.Email), usrMdl.IsApproved == true ? "yes" : "no", Convert.ToString(usrMdl.DateCreated.ToShortDateString()), usrMdl.DateLastLogin == null ? null : Convert.ToString(usrMdl.DateLastLogin.Value.ToShortDateString()), usrMdl.DateLastActivity == null ? null : Convert.ToString(usrMdl.DateLastActivity.Value.ToShortDateString()), Convert.ToString(usrMdl.DateLastPasswordChange.ToShortDateString()) };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = users.Count(),
                iTotalDisplayRecords = filteredUsers.Count(),
                aaData = result
            },
                            JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /User/Details/

        public ActionResult Details(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                User user = _securityService.GetUser(id);

                if (user == null)
                {
                    return HttpNotFound();
                }

                //return View("_Details", user);
                return View(user);
            }
            return HttpNotFound();
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            //return PartialView("_Create");
            return View();
        }

        //
        // POST: /User/Create/ by object

        [HttpPost]
        public ActionResult Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, createPersistentCookie: false);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", SecurityController.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            //return PartialView("_Create", model);
            return View(model);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                User user = _securityService.GetUser(id);

                UserModel userModel = new UserModel()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    OldPassword = null,
                    NewPassword = null,
                    ConfirmPassword = null,
                    Comment = user.Comment,
                    IsApproved = user.IsApproved,
                    DateCreated = user.DateCreated,
                    DateLastLogin = user.DateLastLogin,
                    DateLastActivity = user.DateLastActivity,
                    DateLastPasswordChange = user.DateLastPasswordChange
                };


                if (user == null)
                {
                    return HttpNotFound();
                }
                //return PartialView("_Edit", userModel);
                return View(userModel);
            }
            return HttpNotFound();
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            if (ModelState.IsValid)
            {

                User user = _securityService.GetUser(model.UserName);

                if (user != null)
                {
                    bool changePasswordSucceeded;

                    if (model.OldPassword != null && model.NewPassword != null && model.ConfirmPassword != null)
                    {
                        try
                        {
                            MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
                            changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                        }
                        catch (Exception)
                        {
                            changePasswordSucceeded = false;
                        }
                    }
                    else
                    {
                        changePasswordSucceeded = true;
                    }

                    if (changePasswordSucceeded)
                    {
                        try
                        {
                            user.Email = model.Email;
                            user.Comment = model.Comment;

                            user.IsApproved = model.IsApproved;
                            user.DateCreated = model.DateCreated;
                            user.DateLastLogin = model.DateLastLogin;
                            user.DateLastActivity = model.DateLastActivity;
                            user.DateLastPasswordChange = model.DateLastPasswordChange;

                            _securityService.UpdateUser(user);
                            return RedirectToAction("Index");
                        }
                        catch (Exception)
                        {
                            //throw;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }


            }
            //return PartialView("_Edit", model);
            return View(model);
        }

        //
        // GET: /User/Delete/by id

        public ActionResult Delete(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                User user = _securityService.GetUser(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                //return PartialView("_Delete", user);
                return View(user);
            }
            return HttpNotFound();
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    User user = _securityService.GetUser(id);

                    if (user != null)
                    {
                        _securityService.DeleteUser(user.UserName);

                        return RedirectToAction("Index");
                    }

                }
                catch (Exception)
                {
                    //throw;
                }

            }

            return HttpNotFound();
        }

        public ActionResult AssignRole(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string userName = id;

                var roles = _securityService.GetRoles().ToList();

                if (roles == null)
                {
                    return HttpNotFound();
                }

                UserRoleModel userRoleModel = new UserRoleModel { UserName = userName };

                var assignRoleModels = roles.Count() == 0 ? null : (roles.Select(role => new AssignRoleModel
                {
                    RoleName = role.RoleName,
                    Assigned = role.Users.Where(x => x.UserName == userName).Count() == 0 ? false : true
                }).ToList());

                userRoleModel.AssignRoleModels = assignRoleModels;

                //return PartialView("_AssignRole", userRoleModel);
                //return View("_AssignRole", userRoleModel);))
                return View(userRoleModel);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult AssignRole(string userName, string[] roleName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                User user = _securityService.GetUser(userName);

                if (user != null)
                {
                    try
                    {
                        List<string> roles = new List<string>();

                        foreach (var item in roleName)
                        {
                            roles.Add(item);
                        }

                        _securityService.AddUserToRole(user.UserName, roles);
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        //throw;
                    }
                }
            }
            //return PartialView("_AssignRole");
            return View();
        }

        public PartialViewResult UsrRoles(string usrId)
        {
            string userName = usrId;

            var roles = _securityService.GetRoles().ToList();

            var assignRoleModels = roles.Count() == 0 ? null : (roles.Select(role => new AssignRoleModel
            {
                RoleName = role.RoleName,
                Assigned = role.Users.Where(x => x.UserName == userName).Count() == 0 ? false : true
            }).ToList());

            return PartialView("_UsrRoles", assignRoleModels);
        }


    }
}