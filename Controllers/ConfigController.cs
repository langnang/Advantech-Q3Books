using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Q3Books.DbContexts;
using Q3Books.Models;
using Microsoft.AspNetCore.Cors;

namespace Q3Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly SQLiteDbContext _context;

        public ConfigController(SQLiteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<ResponseModel> GetConfig()
        {
            var books = _context.config.ToList();
            return new ResponseModel
            {
                status = 200,
                statusText = "success",
                data = books
            };
        }
    }
}
