using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace XML_Lab5.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            IList<Models.Book> bookList = new List<Models.Book>();

            //loading books.xml
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList books = doc.GetElementsByTagName("book");

                foreach (XmlElement b in books)
                {
                    Models.Book book = new Models.Book();
                    book.ID = b.GetElementsByTagName("id")[0].InnerText;
                    book.Title = b.GetElementsByTagName("title")[0].InnerText;
                    book.FirstName = b.GetElementsByTagName("firstname")[0].InnerText;
                    book.LastName = b.GetElementsByTagName("lastname")[0].InnerText;

                    bookList.Add(book);
                }
            }


            return View(bookList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var book = new Models.Book();
            return View(book);
        }


        [HttpPost]
        public IActionResult Create(Models.Book b)
        {
            //loading books.xml
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                // if file exists, load file, and create new book
                doc.Load(path);

                //creating a new book 
                XmlElement book = _CreateBookElement(doc, b);

                //append the new book
                doc.DocumentElement.AppendChild(book);
            }
            else
            {
                //if file doesn't exist, create file + create new book
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("book");

                //creating a new book 
                XmlElement book = _CreateBookElement(doc, b);
                root.AppendChild(book);

                //append root element to the doc
                doc.AppendChild(root);

            }
            doc.Save(path);

            return View();
        }

        private XmlElement _CreateBookElement(XmlDocument doc, Models.Book newBook)
        {
            XmlElement book = doc.CreateElement("book");

            // TO DO:
            // Figure out how to get ID and increment it (hint: lastchild + int32.parse)
            XmlNode id = doc.LastChild;



            XmlNode title = doc.CreateElement("title");
            title.InnerText = newBook.Title;
            book.AppendChild(title);

            XmlNode author = doc.CreateElement("author");

            XmlNode firstname = doc.CreateElement("firstname");
            firstname.InnerText = newBook.FirstName;
            XmlNode lastname = doc.CreateElement("lastname");
            lastname.InnerText = newBook.LastName;

            author.AppendChild(firstname);
            author.AppendChild(lastname);

            book.AppendChild(author);

            return book;
        }
    }
}
