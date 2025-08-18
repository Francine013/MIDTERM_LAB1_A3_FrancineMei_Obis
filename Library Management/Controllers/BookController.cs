using Library_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            var books = BookService.Instance.GetBooks();
            return View(books);
        }

        public IActionResult AddModal()
        {
            return PartialView("_AddBookPartial");
        }

        [HttpPost]
        public IActionResult Add(AddBookViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm); // Or return PartialView if in modal
            }

            BookService.Instance.AddBook(vm);

            return RedirectToAction("Index");
        }



        public IActionResult EditModal(Guid id)
        {
            var editBookViewModel = BookService.Instance.GetBookById(id);
            if (editBookViewModel == null) return NotFound();


            return PartialView("_EditBookPartial", editBookViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditBookViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // If model state is not valid, you can return a view with validation errors
                return BadRequest(ModelState);
            }

            // Assuming BookService has a method to update the book
            BookService.Instance.UpdateBook(vm);

            return Ok();
        }



        [HttpGet]
        public IActionResult DeleteModal(Guid id)
        {
            var book = BookService.Instance.GetBookById(id);
            if (book == null) return NotFound();

            var vm = new BookListViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
            };

            return PartialView("_DeleteBookPartial", vm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            BookService.Instance.DeleteBook(id);
            return RedirectToAction("Index"); // NOT return Ok();
        }
        public IActionResult Details(Guid id)
        {
            var book = BookService.Instance.GetBooks().First(b => b.BookId == id);
            return View(book);
        }
        public IActionResult AddCopyModal(Guid id)
        {
            var vm = new CopyBookViewModel { BookId = id };
            return PartialView("_CopyBookPartial", vm);
        }

        [HttpPost]
        public IActionResult AddCopy(CopyBookViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            BookService.Instance.AddBookCopy(model);

            return RedirectToAction("Details", new { id = model.BookId });
        }




    }
}