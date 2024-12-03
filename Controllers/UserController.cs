using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using ShoesStor.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoesStor.Models;
using ShoesStor.Services;

namespace ShoesStor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        //    private IUserService UserService;
        // public UserController(IUserService UserService)
        // {
        //     this.UserService = UserService;
        // }

        IUserServices UsersService;

        public UserController(IUserServices UserServices)
        {
            this.UsersService = UserServices;
        }

        // [Route("[action]")]
        [HttpGet]
        [Authorize(Policy = "Admin")]

        public ActionResult<List<User>> Get()
        {
            return UsersService.GetAll();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "User")]

        public ActionResult<User> Get(int id)
        {
            if ((int.Parse(User.FindFirst("id")?.Value!) != id) && User.FindFirst("type")?.Value != "Admin")
                return Unauthorized();
            var user = UsersService.GetById(id);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public ActionResult Post(User newUser)
        {
            var newId = UsersService.Add(newUser);
            return CreatedAtAction("Post",
                new { id = newId }, UsersService.GetById(newId));
        }


        [HttpPost]
        [Route("/login")]
        public ActionResult<objectToReturn> Login([FromBody] User User)
        {

            int UserExistID = UsersService.ExistUser(User.Username, User.Password);
            if (UserExistID == -1)
            {
                return Unauthorized();
            }

            var claims = new List<Claim> { };
            if (User.Password == "1234" && User.Username == "eti")
                claims.Add(new Claim("type", "Admin"));
            else
                claims.Add(new Claim("type", "User"));

            claims.Add(new Claim("id", UserExistID.ToString()));

            var token = UserTokenServices.GetToken(claims);
            return new OkObjectResult(new { Id = UserExistID, token = UserTokenServices.WriteToken(token) });
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "User")]

        public ActionResult Put(int id, User newUser)
        {

            var result = UsersService.Update(id, newUser);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            bool result = UsersService.Delete(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // [HttpPost]
        // [Route("[action]")]
        // [Authorize(Policy = "Admin")]
        // public IActionResult GenerateBadge([FromBody] Agent Agent)
        // {
        //     var claims = new List<Claim>
        //     {
        //         new Claim("type", "Agent"),
        //         new Claim("ClearanceLevel", Agent.ClearanceLevel.ToString()),
        //     };

        //     var token = UserTokenServices.GetToken(claims);

        //     return new OkObjectResult(UserTokenServices.WriteToken(token));
        // }
    }
}

public class objectToReturn
{
    public int Id { get; set; }

    public string token { get; set; }
}

