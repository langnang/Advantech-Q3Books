using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Q3Books.DbContexts;
using Q3Books.Models;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Q3Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly SQLiteDbContext _context;

        public UserController(SQLiteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<ResponseModel> GetVersion() {
            return new ResponseModel
            {
                status = 200,
                statusText = "success",
            };
        }
        [HttpPost("login")]
        public ActionResult<ResponseModel> PostLogin([FromBody] UserModel _table) {
            var user = _context.user.SingleOrDefault(tb => tb.job_number == _table.job_number);

            if (user!=null)
            {
                return new ResponseModel
                {
                    status = 200,
                    statusText = "success",
                    data=user
                };
            }
            else
            {
                return new ResponseModel
                {
                    status = 404,
                    statusText = "failure",
                };
            }
        }

        [HttpGet("all")]
        public ActionResult<ResponseModel> GetAll()
        {
            var users = _context.user.ToList();

            return new ResponseModel
            {
                status = 200,
                statusText = "success",
                data = users
            };
        }
    }
}
