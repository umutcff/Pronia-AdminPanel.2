using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProniaUmut.Models;
using ProniaUmut.ViewModels.UserViewModels;

namespace ProniaUmut.Controllers
{

    public class AccountController(UserManager<AppUser> _userManager,SignInManager<AppUser> _signInManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if(ModelState.IsValid)
            {
                return View(vm);
            }

            var existUser = await _userManager.FindByEmailAsync(vm.EmailAddress);

            if(existUser != null)
            {
                ModelState.AddModelError("EmailAddress", "This Email is already exist!");
                return View(vm);
            }


            AppUser user = new()
            {
                FullName=vm.LastName+" "+vm.FirstName,
                Email=vm.EmailAddress
            };

            var result=await _userManager.CreateAsync(user, vm.Password);
            if(result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            return Ok("ok");
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userManager.FindByEmailAsync(vm.EmailAddress);
            if(user != null)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(vm);
            }

            var loginResult=await _userManager.CheckPasswordAsync(user,vm.Password);
            if (!loginResult)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(vm);
            }

            await _signInManager.SignInAsync(user, false);

            return Ok($"{user.FullName} neye gelmisiniz");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
