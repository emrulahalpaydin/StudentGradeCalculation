using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Common.Encryption;
using Common.Extensions;
using Common.Helpers;
using Database.Entities;
using StudentGradeCalculation.Models;

namespace StudentGradeCalculation.Controllers
{
    public class AdministrationController : BaseController
    {
		//[AllowAnonymous]
		//public async Task<ActionResult> Register()
		//{
		//	return View();
		//}
		//[AllowAnonymous]
		//[HttpPost]
		//public async Task<JsonResult> Register(RegisterViewModel model)
		//{
		//	bool hasError = false;
		//	string message = "";
		//	try
		//	{
		//		if (model.Password != model.PasswordConfirmation)
		//		{
		//			hasError = true;
		//			message = "Girdiğiniz şifreler birbirinden farklı.";
		//		}
		//		else
		//		{
		//			if (model.NameSurname != null)
		//			{
		//				if (model.StudentNumber != 0)
		//				{
		//					var student = new Student
		//					{
		//						NameSurname = model.NameSurname,
		//						StudentNumber = model.StudentNumber,
		//						Password = EncryptionSpiral.SpiralEncrypt(model.Password),
		//						Active = true,
		//						CreateDate = DateTime.Now,
		//						LastLoginDate = DateTime.Now,
		//					};
		//					using (var db = DBInstance.CreateInstance())
		//					{
		//						db.Students.Add(student);
		//						if (await db.SaveChangesAsync() > 0)
		//						{
		//							hasError = false;
		//							message = "Kayıt başarılı.";
		//						}
		//						else
		//						{
		//							hasError = true;
		//							message = "Kayıt yapılırken bir sorun oluştu.";
		//						}
		//					}
		//				}
		//				else
		//				{
		//					hasError = true;
		//					message = "Öğrenci numarası alınamadı.";
		//				}
		//			}
		//			else
		//			{
		//				hasError = true;
		//				message = "Ad soyad alınamadı.";
		//			}
		//		}
		//	}
		//	catch
		//	{
		//		throw;
		//	}
		//	return Json(new {hasError = hasError,message = message});
		//}
		//[AllowAnonymous]
		//public async Task<ActionResult> Login()
		//{
		//	return View();
		//}
		//[AllowAnonymous]
		//[ValidateAntiForgeryToken]
		//[HttpPost]
		//public async Task<JsonResult> Login(LoginViewModel model)
		//{
		//	bool hasError = false;
		//	string message = "";
		//	try
		//	{
		//		if (ModelState.IsValid)
		//		{
		//			model.Password = EncryptionSpiral.SpiralEncrypt(model.Password);
		//			using (var db = DBInstance.CreateInstance())
		//			{
		//				var student = await db.Students.FirstOrDefaultAsync(a => a.StudentNumber == model.StudentNumber && a.Password == model.Password);
		//				if (student != null)
		//				{
		//					if (student.Active)
		//					{
		//						FormsAuthentication.SignOut();
		//						FormsAuthentication.SetAuthCookie(EncryptionSpiral.SpiralEncrypt(student.StudentNumber.ToString()), false);
		//						student.LastLoginDate = DateTime.Now;
		//						await db.SaveChangesAsync();
		//						SessionHelper.Set("StudentNumber", student.StudentNumber);
		//						SessionHelper.Set("UserId", student.ID);
		//						SessionHelper.Set("StudentName", student.NameSurname);
		//						CookieHelper.CreateCookie("StudentNumber", student.StudentNumber.ToString(), null);
		//						CookieHelper.CreateCookie("UserId", student.ID.ToString(), null);
		//						CookieHelper.CreateCookie("StudentName", student.NameSurname.ToString(), null);
		//						hasError = false;
		//						message = "Giriş Başarılı";
		//					}
		//					else
		//					{
		//						hasError = true;
		//						message = "Hesabınız aktif değil, yöneticiye başvurunuz.";
		//					}
		//				}
		//				else
		//				{
		//					hasError = true;
		//					message = "Böyle bir kullanıcı bulunamadı";
		//				}
		//			}
		//		}
		//		else
		//		{
		//			hasError = true;
		//			message = "Lütfen Kullanıcı adı ve şifre giriniz.";
		//		}
		//	}
		//	catch
		//	{
		//		throw;
		//	}
		//	return Json(new { hasError = hasError, message = message, ReturnUrl = model.ReturnUrl });
		//}
		//public async Task<ActionResult> Logout()
		//{
		//	FormsAuthentication.SignOut();
		//	Session.Abandon();
		//	Session.Clear();
		//	CookieHelper.DeleteCookie("UserName");
		//	CookieHelper.DeleteCookie("UserId");
		//	CookieHelper.DeleteCookie("StudentName");
		//	return RedirectToAction("Login");
		//}
		//public async Task<ActionResult> ChangePassword()
		//{
		//	try
		//	{
		//		var userId = Convert.ToInt32(CookieHelper.GetCookie("UserId"));
		//		if (userId != 0)
		//		{
		//			using (var db = DBInstance.CreateInstance())
		//			{
		//				var user = await db.Students.FirstOrDefaultAsync(a => a.ID == userId);
		//				if (user != null)
		//				{
		//					return View(user);
		//				}
		//			}
		//		}
		//	}
		//	catch
		//	{
		//		throw;
		//	}
		//	return View();
		//}
		//[HttpPost]
		//public async Task<JsonResult> ChangePassword(ChangePasswordModel model)
		//{
		//	bool hasError = false;
		//	string message = "";
		//	try
		//	{
		//		if (model.OldPassword != null)
		//		{
		//			if (model.NewPassword != null && model.NewPasswordConfirmation != null)
		//			{
		//				if (model.NewPassword.Trim() == model.NewPasswordConfirmation.Trim())
		//				{
		//					var userId = Convert.ToInt32(CookieHelper.GetCookie("UserId"));
		//					if (userId != 0)
		//					{
		//						using (var db = DBInstance.CreateInstance())
		//						{
		//							var user = await db.Students.FirstOrDefaultAsync(a => a.ID == userId);
		//							if (user != null)
		//							{
		//								var hashingOldPassword = EncryptionSpiral.SpiralEncrypt(model.OldPassword);
		//								if (user.Password == hashingOldPassword)
		//								{
		//									user.Password = EncryptionSpiral.SpiralEncrypt(model.NewPassword);
		//									if (await db.SaveChangesAsync() > 0)
		//									{
		//										hasError = false;
		//										message = "Şifre güncelleme başarılı.";
		//									}
		//									else
		//									{
		//										hasError = true;
		//										message = "Şifre güncellenirken bir sorun oluştu.";
		//									}
		//								}
		//								else
		//								{
		//									hasError = true;
		//									message = "Eski parolanızı lütfen doğru giriniz.";
		//								}
		//							}
		//							else
		//							{
		//								hasError = true;
		//								message = "Kullanıcı bulunamadı.";
		//							}
		//						}
		//					}
		//					else
		//					{
		//						hasError = true;
		//						message = "Kullanıcı ID'si alınamadı.";
		//					}
		//				}
		//				else
		//				{
		//					hasError |= true;
		//					message = "Şifre ve Şifre Tekrarı Uyuşmamaktadır.";
		//				}
		//			}
		//			else
		//			{
		//				hasError = true;
		//				message = "Lütfen yeni parolanızı giriniz.";
		//			}
		//		}
		//		else
		//		{
		//			hasError = true;
		//			message = "Lütfen eski parolanızı giriniz.";
		//		}
		//	}
		//	catch
		//	{
		//		throw;
		//	}
		//	return Json(new { hasError = hasError, message = message });
		//}
	}
}