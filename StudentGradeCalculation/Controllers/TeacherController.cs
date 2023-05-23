using Common.Encryption;
using Common.Helpers;
using Database.Entities;
using StudentGradeCalculation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static Database.Enums.Enums;

namespace StudentGradeCalculation.Controllers
{
    public class TeacherController : BaseController
    {
		[AllowAnonymous]
		public async Task<ActionResult> Login()
		{
			return View();
		}
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<JsonResult> Login(TeacherLoginViewModel model)
		{
			bool hasError = false;
			string message = "";
			try
			{
				if (ModelState.IsValid)
				{
					model.Password = EncryptionSpiral.SpiralEncrypt(model.Password);
					using (var db = DBInstance.CreateInstance())
					{
						var teacher = await db.Teachers.FirstOrDefaultAsync(a => a.UserName == model.UserName && a.Password == model.Password);
						if (teacher != null)
						{
							if (teacher.Active)
							{
								FormsAuthentication.SignOut();
								FormsAuthentication.SetAuthCookie(EncryptionSpiral.SpiralEncrypt(teacher.UserName.ToString()), false);
								teacher.LastLoginDate = DateTime.Now;
								await db.SaveChangesAsync();
								SessionHelper.Set("UserName", teacher.UserName);
								SessionHelper.Set("UserId", teacher.ID);
								SessionHelper.Set("TeacherName", teacher.NameSurname);
								CookieHelper.CreateCookie("UserName", teacher.UserName.ToString(), null);
								CookieHelper.CreateCookie("UserId", teacher.ID.ToString(), null);
								CookieHelper.CreateCookie("NameSurname", teacher.NameSurname.ToString(), null);
								CookieHelper.CreateCookie("UserType", UserType.Student.ToString(), null);
								hasError = false;
								message = "Giriş Başarılı";
							}
							else
							{
								hasError = true;
								message = "Hesabınız aktif değil, yöneticiye başvurunuz.";
							}
						}
						else
						{
							hasError = true;
							message = "Böyle bir kullanıcı bulunamadı";
						}
					}
				}
				else
				{
					hasError = true;
					message = "Lütfen Kullanıcı adı ve şifre giriniz.";
				}
			}
			catch
			{
				throw;
			}
			return Json(new { hasError = hasError, message = message, ReturnUrl = model.ReturnUrl });
		}
		public async Task<ActionResult> Logout()
		{
			FormsAuthentication.SignOut();
			Session.Abandon();
			Session.Clear();
			CookieHelper.DeleteCookie("UserName");
			CookieHelper.DeleteCookie("UserId");
			CookieHelper.DeleteCookie("StudentName");
			return RedirectToAction("Login");
		}
		public async Task<ActionResult> ChangePassword()
		{
			try
			{
				var userId = Convert.ToInt32(CookieHelper.GetCookie("UserId"));
				if (userId != 0)
				{
					using (var db = DBInstance.CreateInstance())
					{
						var user = await db.Teachers.FirstOrDefaultAsync(a => a.ID == userId);
						if (user != null)
						{
							return View(user);
						}
					}
				}
			}
			catch
			{
				throw;
			}
			return View();
		}
		[HttpPost]
		public async Task<JsonResult> ChangePassword(ChangePasswordModel model)
		{
			bool hasError = false;
			string message = "";
			try
			{
				if (model.OldPassword != null)
				{
					if (model.NewPassword != null && model.NewPasswordConfirmation != null)
					{
						if (model.NewPassword.Trim() == model.NewPasswordConfirmation.Trim())
						{
							var userId = Convert.ToInt32(CookieHelper.GetCookie("UserId"));
							if (userId != 0)
							{
								using (var db = DBInstance.CreateInstance())
								{
									var user = await db.Teachers.FirstOrDefaultAsync(a => a.ID == userId);
									if (user != null)
									{
										var hashingOldPassword = EncryptionSpiral.SpiralEncrypt(model.OldPassword);
										if (user.Password == hashingOldPassword)
										{
											user.Password = EncryptionSpiral.SpiralEncrypt(model.NewPassword);
											if (await db.SaveChangesAsync() > 0)
											{
												hasError = false;
												message = "Şifre güncelleme başarılı.";
											}
											else
											{
												hasError = true;
												message = "Şifre güncellenirken bir sorun oluştu.";
											}
										}
										else
										{
											hasError = true;
											message = "Eski parolanızı lütfen doğru giriniz.";
										}
									}
									else
									{
										hasError = true;
										message = "Kullanıcı bulunamadı.";
									}
								}
							}
							else
							{
								hasError = true;
								message = "Kullanıcı ID'si alınamadı.";
							}
						}
						else
						{
							hasError |= true;
							message = "Şifre ve Şifre Tekrarı Uyuşmamaktadır.";
						}
					}
					else
					{
						hasError = true;
						message = "Lütfen yeni parolanızı giriniz.";
					}
				}
				else
				{
					hasError = true;
					message = "Lütfen eski parolanızı giriniz.";
				}
			}
			catch
			{
				throw;
			}
			return Json(new { hasError = hasError, message = message });
		}
	}
}