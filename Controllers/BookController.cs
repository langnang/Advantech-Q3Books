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
    public class BookController : ControllerBase
    {
        private readonly SQLiteDbContext _context;

        public BookController(SQLiteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<ResponseModel> GetVersion()
        {
            return new ResponseModel
            {
                status = 200,
                statusText = "success",
            };
        }

        [HttpPut("add")]
        public ActionResult<ResponseModel> PutAdd([FromBody] BookModel _book)
        {
            var exist = _context.book.SingleOrDefault(tb => tb.user == _book.user && tb.book == _book.book);
            if (exist != null) {
                return new ResponseModel
                {
                    status = 404,
                    statusText = "failure",
                };
            }
            _book.datetime = DateTime.Now;
            _context.book.Add(_book);
            if (_context.SaveChanges() > 0)
            {
                return new ResponseModel
                {
                    status = 200,
                    statusText = "success",
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

        [HttpDelete("delete")]
        public ActionResult<ResponseModel> Delete([FromBody] BookModel _book) {
            var book = _context.book.SingleOrDefault(tb => tb.user == _book.user && tb.book == _book.book);
            if (book == null)
            {
                return new ResponseModel
                {
                    status = 404,
                    statusText = "failure",
                };
            }
            _context.book.Remove(book);
            if (_context.SaveChanges() > 0)
            {
                return new ResponseModel
                {
                    status = 200,
                    statusText = "success",
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

        [HttpPost("list")]
        public ActionResult<ResponseModel> PostList([FromBody] UserModel _user)
        {
            var books = _context.book.Where(tb => tb.user == _user.job_number);
            return new ResponseModel
            {
                status = 200,
                statusText = "success",
                data = books
            };
        }

        [HttpGet("all")]
        public ActionResult<ResponseModel> GetAll()
        {
            var books = _context.book.ToList();
            return new ResponseModel
            {
                status = 200,
                statusText = "success",
                data = books
            };
        }
    }
    
}
