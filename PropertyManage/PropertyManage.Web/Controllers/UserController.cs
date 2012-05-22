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
using PropertyManage.Web.Helpers;

namespace PropertyManage.Web.Controllers
{
    //[Authorize]
    //[System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class UserController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly IProfileService _profileService;

        public UserController(ISecurityService securityService, IProfileService profileService)
        {
            this._securityService = securityService;
            this._profileService = profileService;
        }

        //
        // GET: /User/

        public ActionResult Index()
        {
            this.ShowTitle("User Management");
            this.ShowBreadcrumb("User", "Index");
            return View();
        }

        // for display datatable
        public ActionResult GetUsers(DataTableParamModel param)
        {
            var users = _securityService.GetUsers().ToList();

            var viewUsers = users.Select(u => new UserTableModels() { UserName = u.UserName, Email = u.Email, FullName = u.Profile == null ? null : Convert.ToString(u.Profile.FullName), Address = u.Profile == null ? null : Convert.ToString(u.Profile.Address), Phone = u.Profile == null ? null : Convert.ToString(u.Profile.PhoneNumber), Mobile = u.Profile == null ? null : Convert.ToString(u.Profile.MobileNumber), CreateDate = u.Profile == null ? null : Convert.ToString(u.DateCreated) });

            IEnumerable<UserTableModels> filteredUsers;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredUsers = viewUsers.Where(usr => (usr.UserName ?? "").Contains(param.sSearch) || (usr.FullName ?? "").Contains(param.sSearch)).ToList();
            }
            else
            {
                filteredUsers = viewUsers;
            }

            var viewOdjects = filteredUsers.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from usrMdl in viewOdjects
                         select new[] { usrMdl.UserName, usrMdl.UserName, usrMdl.Email, usrMdl.FullName, usrMdl.Address, usrMdl.Phone, usrMdl.Mobile, usrMdl.CreateDate };

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
            this.ShowTitle("User Management");
            this.ShowBreadcrumb("User", "Details");

            if (!string.IsNullOrEmpty(id))
            {
                var roles = _securityService.GetRoles().ToList();

                var appPrivilegeModels = roles.Count() == 0 ? null : (roles.Select(role => new AppPrivilegeModel
                                                                                                    {
                                                                                                        PrivilegeName = role.Users.Where(x => x.UserName.ToLower() == id.ToLower()).Count() == 0 ? null : role.RoleName,
                                                                                                    }).ToList());

                User user = _securityService.GetUser(id);

                Profile profile = _profileService.GetProfiles().Where(x => x.UserName.ToLower() == id.ToLower()).FirstOrDefault();

                if (user == null)
                {
                    this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                    return RedirectToAction("Index");
                }

                CreateUserModel viewUserModel = new CreateUserModel();

                if (profile == null)
                {
                    viewUserModel.UserName = user.UserName;
                    viewUserModel.Email = user.Email;
                    viewUserModel.Password = null;
                    viewUserModel.ConfirmPassword = null;
                }
                else
                {
                    viewUserModel.UserName = user.UserName;
                    viewUserModel.Email = user.Email;
                    viewUserModel.Password = null;
                    viewUserModel.ConfirmPassword = null;
                    viewUserModel.FirstName = profile.FirstName;
                    viewUserModel.LastName = profile.LastName;
                    viewUserModel.SurName = profile.SurName;
                    viewUserModel.DateOfBirth = profile.DateOfBirth == null ? null : profile.DateOfBirth.Value.ToString("MM/dd/yyyy");
                    viewUserModel.Address = profile.Address;
                    viewUserModel.PhoneNumber = profile.PhoneNumber;
                    viewUserModel.MobileNumber = profile.MobileNumber;
                    viewUserModel.ThumbImageUrl = profile.ThumbImageUrl;
                    viewUserModel.SmallImageUrl = profile.SmallImageUrl;
                }

                viewUserModel.AppPrivilegeModels = appPrivilegeModels;

                //return View("_Details", viewUserModel);
                return View(viewUserModel);
            }
            this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
            return RedirectToAction("Index");
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            this.ShowTitle("User Management");
            this.ShowBreadcrumb("User", "Create");

            var roles = _securityService.GetRoles().ToList();

            CreateUserModel createUserModel = new CreateUserModel();

            var appPrivilegeModels = roles.Count() == 0 ? null : (roles.Select(role => new AppPrivilegeModel
                                                                                            {
                                                                                                PrivilegeName = role.RoleName
                                                                                            }).ToList());

            createUserModel.AppPrivilegeModels = appPrivilegeModels;

            //return PartialView("_Create", appUserModel);
            return View(createUserModel);
        }

        //
        // POST: /User/Create/by object

        [HttpPost]
        public ActionResult Create(CreateUserModel model, string[] privilegeName)
        {
            this.ShowTitle("User Management");
            this.ShowBreadcrumb("User", "Create");

            var roles = _securityService.GetRoles().ToList();

            var appPrivilegeModels = roles.Count() == 0 ? null : (roles.Select(role => new AppPrivilegeModel
                                                                                            {
                                                                                                PrivilegeName = role.RoleName
                                                                                            }).ToList());

            model.AppPrivilegeModels = appPrivilegeModels;

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {

                    User user = _securityService.GetUser(model.UserName);

                    if (user != null)
                    {
                        Profile profile = new Profile
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            SurName = model.SurName,
                            Address = model.Address,
                            DateOfBirth = Convert.ToDateTime(model.DateOfBirth),
                            MobileNumber = model.MobileNumber,
                            PhoneNumber = model.PhoneNumber,
                            ThumbImageUrl = model.ThumbImageUrl,
                            SmallImageUrl = model.SmallImageUrl,
                            UserName = model.UserName
                        };

                        //Profile create for user
                        _profileService.CreateProfile(profile);

                        var selectRoles = roles;

                        var lstRoles = new List<Role>();

                        foreach (var roleName in privilegeName)
                        {
                            string id = roleName;
                            lstRoles.Add(selectRoles.Where(x => x.RoleName == id).FirstOrDefault());
                        }

                        user.Roles = lstRoles;
                        user.Profile = profile;

                        //User Update
                        _securityService.UpdateUser(user);
                        this.ShowMessage("User created successfully", MessageType.Success);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user is invalid.");
                    }

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
        // GET: /User/Edit/by id

        public ActionResult Edit(string id = null)
        {
            this.ShowTitle("User Management");
            this.ShowBreadcrumb("User", "Edit");

            if (!string.IsNullOrEmpty(id))
            {
                var roles = _securityService.GetRoles().ToList();

                var appPrivilegeModels = roles.Count() == 0 ? null : (roles.Select(role => new AppPrivilegeModel
                                                                                                {
                                                                                                    PrivilegeName = role.RoleName,
                                                                                                    Assigned = role.Users.Where(x => x.UserName.ToLower() == id.ToLower()).Count() == 0 ? false : true
                                                                                                }).ToList());

                User user = _securityService.GetUser(id);

                Profile profile = _profileService.GetProfiles().Where(x => x.UserName.ToLower() == id.ToLower()).FirstOrDefault();

                if (user == null)
                {
                    this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                    return RedirectToAction("Index");
                }

                EditUserModel editUserModel = new EditUserModel();

                if (profile == null)
                {
                    editUserModel.UserName = user.UserName;
                    editUserModel.Email = user.Email;
                    editUserModel.OldPassword = null;
                    editUserModel.NewPassword = null;
                    editUserModel.ConfirmPassword = null;

                }
                else
                {
                    editUserModel.UserName = user.UserName;
                    editUserModel.Email = user.Email;
                    editUserModel.OldPassword = null;
                    editUserModel.NewPassword = null;
                    editUserModel.ConfirmPassword = null;
                    editUserModel.FirstName = profile.FirstName;
                    editUserModel.LastName = profile.LastName;
                    editUserModel.SurName = profile.SurName;
                    editUserModel.DateOfBirth = profile.DateOfBirth == null ? null : profile.DateOfBirth.Value.ToString("MM/dd/yyyy");
                    editUserModel.Address = profile.Address;
                    editUserModel.PhoneNumber = profile.PhoneNumber;
                    editUserModel.MobileNumber = profile.MobileNumber;
                    editUserModel.ThumbImageUrl = profile.ThumbImageUrl;
                    editUserModel.SmallImageUrl = profile.SmallImageUrl;
                }

                editUserModel.AppPrivilegeModels = appPrivilegeModels;

                //return PartialView("_Edit", editUserModel);
                return View(editUserModel);
            }
            this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
            return RedirectToAction("Index");
        }

        //
        // POST: /User/Edit/by object

        [HttpPost]
        public ActionResult Edit(EditUserModel model, string[] privilegeName)
        {
            this.ShowTitle("User Management");
            this.ShowBreadcrumb("User", "Edit");

            var roles = _securityService.GetRoles().ToList();

            var appPrivilegeModels = roles.Count() == 0 ? null : (roles.Select(role => new AppPrivilegeModel
                                                                                                {
                                                                                                    PrivilegeName = role.RoleName,
                                                                                                    Assigned = role.Users.Where(x => x.UserName.ToLower() == model.UserName.ToLower()).Count() == 0 ? false : true
                                                                                                }).ToList());

            model.AppPrivilegeModels = appPrivilegeModels;

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
                            MembershipUser currentUser = Membership.GetUser(model.UserName, userIsOnline: true);
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
                            var selectRoles = roles;

                            var lstRoles = new List<Role>();

                            foreach (var roleName in privilegeName)
                            {
                                string id = roleName;
                                lstRoles.Add(selectRoles.Where(x => x.RoleName == id).FirstOrDefault());
                            }

                            user.Roles = lstRoles;

                            Profile profile = _profileService.GetProfiles().Where(x => x.UserName.ToLower() == model.UserName.ToLower()).FirstOrDefault();

                            if (profile != null)
                            {
                                profile.FirstName = model.FirstName;
                                profile.LastName = model.LastName;
                                profile.SurName = model.SurName;
                                profile.Address = model.Address;
                                profile.DateOfBirth = Convert.ToDateTime(model.DateOfBirth);
                                profile.MobileNumber = model.MobileNumber;
                                profile.PhoneNumber = model.PhoneNumber;
                                profile.ThumbImageUrl = model.ThumbImageUrl;
                                profile.SmallImageUrl = model.SmallImageUrl;
                                profile.UserName = model.UserName;

                                _profileService.UpdateProfile(profile);

                                user.Profile = profile;
                            }
                            else
                            {
                                Profile tempProfile = new Profile
                                {
                                    FirstName = model.FirstName,
                                    LastName = model.LastName,
                                    SurName = model.SurName,
                                    Address = model.Address,
                                    DateOfBirth = Convert.ToDateTime(model.DateOfBirth),
                                    MobileNumber = model.MobileNumber,
                                    PhoneNumber = model.PhoneNumber,
                                    ThumbImageUrl = model.ThumbImageUrl,
                                    SmallImageUrl = model.SmallImageUrl,
                                    UserName = model.UserName
                                };

                                _profileService.CreateProfile(tempProfile);

                                user.Profile = tempProfile;
                            }

                            _securityService.UpdateUser(user);
                            this.ShowMessage("User updated successfully", MessageType.Success);
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
            this.ShowTitle("User Management");
            this.ShowBreadcrumb("User", "Delete");

            if (!string.IsNullOrEmpty(id))
            {
                var roles = _securityService.GetRoles().ToList();

                var appPrivilegeModels = roles.Count() == 0 ? null : (roles.Select(role => new AppPrivilegeModel
                                                                                                {
                                                                                                    PrivilegeName = role.Users.Where(x => x.UserName.ToLower() == id.ToLower()).Count() == 0 ? null : role.RoleName,
                                                                                                }).ToList());

                User user = _securityService.GetUser(id);

                Profile profile = _profileService.GetProfiles().Where(x => x.UserName.ToLower() == id.ToLower()).FirstOrDefault();

                if (user == null)
                {
                    this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                    return RedirectToAction("Index");
                }

                CreateUserModel viewUserModel = new CreateUserModel();

                if (profile == null)
                {
                    viewUserModel.UserName = user.UserName;
                    viewUserModel.Email = user.Email;
                    viewUserModel.Password = null;
                    viewUserModel.ConfirmPassword = null;
                }
                else
                {
                    viewUserModel.UserName = user.UserName;
                    viewUserModel.Email = user.Email;
                    viewUserModel.Password = null;
                    viewUserModel.ConfirmPassword = null;
                    viewUserModel.FirstName = profile.FirstName;
                    viewUserModel.LastName = profile.LastName;
                    viewUserModel.SurName = profile.SurName;
                    viewUserModel.DateOfBirth = profile.DateOfBirth == null ? null : profile.DateOfBirth.Value.ToString("MM/dd/yyyy");
                    viewUserModel.Address = profile.Address;
                    viewUserModel.PhoneNumber = profile.PhoneNumber;
                    viewUserModel.MobileNumber = profile.MobileNumber;
                    viewUserModel.ThumbImageUrl = profile.ThumbImageUrl;
                    viewUserModel.SmallImageUrl = profile.SmallImageUrl;
                }

                viewUserModel.AppPrivilegeModels = appPrivilegeModels;

                //return PartialView("_Delete", viewUserModel);
                return View(viewUserModel);
            }
            this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
            return RedirectToAction("Index");
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            this.ShowTitle("User Management");
            this.ShowBreadcrumb("User", "Delete");

            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    User user = _securityService.GetUser(id);

                    if (user != null)
                    {
                        _securityService.DeleteUser(user.UserName);
                        this.ShowMessage("User deleted successfully", MessageType.Success);
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }

            }

            this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
            return RedirectToAction("Index");
        }

        public ActionResult Privilege(string id)
        {
            this.ShowTitle("User Management");
            this.ShowBreadcrumb("User", "Privilege");

            if (!string.IsNullOrEmpty(id))
            {
                string userName = id;

                var roles = _securityService.GetRoles().ToList();

                if (roles == null)
                {
                    return HttpNotFound();
                }

                CreateUserModel createUserModel = new CreateUserModel { UserName = userName };

                var appPrivilegeModels = roles.Count() == 0 ? null : (roles.Select(role => new AppPrivilegeModel
                                                                                                {
                                                                                                    PrivilegeName = role.RoleName,
                                                                                                    Assigned = role.Users.Where(x => x.UserName == userName).Count() == 0 ? false : true
                                                                                                }).ToList());

                createUserModel.AppPrivilegeModels = appPrivilegeModels;

                //return PartialView("_Privilege", createUserModel);
                //return View("_Privilege", createUserModel);))
                return View(createUserModel);
            }
            this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Privilege(string userName, string[] privilegeName)
        {
            this.ShowTitle("User Management");
            this.ShowBreadcrumb("User", "Privilege");

            var tempRoles = _securityService.GetRoles().ToList();

            CreateUserModel createUserModel = new CreateUserModel { UserName = userName };

            var appPrivilegeModels = tempRoles.Count() == 0 ? null : (tempRoles.Select(role => new AppPrivilegeModel
                                                                                                {
                                                                                                    PrivilegeName = role.RoleName,
                                                                                                    Assigned = role.Users.Where(x => x.UserName == userName).Count() == 0 ? false : true
                                                                                                }).ToList());

            createUserModel.AppPrivilegeModels = appPrivilegeModels;

            if (!string.IsNullOrEmpty(userName))
            {
                User user = _securityService.GetUser(userName);

                if (user != null)
                {
                    try
                    {
                        List<string> roles = new List<string>();

                        foreach (var item in privilegeName)
                        {
                            roles.Add(item);
                        }

                        _securityService.AddUserToRole(user.UserName, roles);
                        this.ShowMessage("User privilege seted successfully", MessageType.Success);
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                    }
                }
            }
            //return PartialView("_Privilege", createUserModel);
            return View(createUserModel);
        }

        public PartialViewResult UserRoles(string usrId)
        {
            string userName = usrId;

            var user = _securityService.GetUser(userName);

            var selectUsers = user.Roles.ToList();

            IEnumerable<AppPrivilegeModel> appPrivilegeModels = selectUsers.Count() == 0 ? null : (selectUsers.Select(role => new AppPrivilegeModel
                                                                                                {
                                                                                                    PrivilegeName = role.RoleName
                                                                                                }).ToList());


            return PartialView("_UserRoles", appPrivilegeModels);
        }


    }
}