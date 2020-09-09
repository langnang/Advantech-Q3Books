using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Q3Books.DbContexts;
using Q3Books.Models;
using Microsoft.AspNetCore.Cors;
using HtmlAgilityPack;

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
            // var exist = _context.book.SingleOrDefault(tb => tb.user == _book.user && tb.book == _book.book);
            // if (exist != null) {
            //    return new ResponseModel
            //    {
            //        status = 404,
            //        statusText = "failure",
            //    };
            // }
            _book.datetime = DateTime.Now;
            _context.book.Add(_book);
            if (_context.SaveChanges() > 0)
            {
                var added = _context.book.SingleOrDefault(tb => tb.user == _book.user && tb.book == _book.book && tb.datetime == _book.datetime);
                return new ResponseModel
                {
                    status = 200,
                    statusText = "success",
                    data = added
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
            var book = _context.book.SingleOrDefault(tb => tb.user == _book.user && tb.book == _book.book && tb.ID == _book.ID);
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
            var books = _context.book.OrderBy(book=>book.user).ToList();
            return new ResponseModel
            {
                status = 200,
                statusText = "success",
                data = books
            };
        }

        [HttpGet("{id}")]
        public ActionResult<ResponseModel> GetInfo(string id) {
            var book = new BookItemModel {ID = id };
            var url = "http://product.dangdang.com/" + id + ".html";
            var html = @url;
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='product_info']");
            if (node == null)
            {
                return new ResponseModel
                {
                    status = 404,
                    statusText = "failure",
                };
            }
            var name_node = node.SelectSingleNode("//div[@class='name_info']");
            var name_info = name_node.SelectSingleNode("//h1");
            if (name_info != null)
            {
                book.name = name_info.Attributes["title"].Value;
            }

            var ddzy_info = name_node.SelectSingleNode("//img[@class='icon_name']");
            if (ddzy_info != null)
            {
                book.ddzy = true;
            }
            else
            {
                book.ddzy = false;
            }
            var author_info = node.SelectSingleNode("//span[@id='author']");
            if (author_info != null)
            {
                book.author = author_info.InnerText.Replace("作者:", "");
            }
            var publisher_info = node.SelectSingleNode("//a[@dd_name='出版社']");
            if (publisher_info != null)
            {
                book.publisher = publisher_info.InnerText;
            }

            var price_info = node.SelectSingleNode("//div[@id='original-price']");
            if (price_info != null)
            {
                book.price = float.Parse(price_info.InnerText.Replace("&yen;", ""));
            }
            return new ResponseModel
            {
                status = 200,
                statusText = "success",
                data = book
            };
        }
    }
    
}
