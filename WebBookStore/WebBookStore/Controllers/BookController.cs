using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        public BookController(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<ActionResult> Index()
        {
            var books = await _bookRepository.GetAllAsync();
            return View(books);
        }
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories,"Id","Name");
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Add(Book book) 
        {
            if(ModelState.IsValid)
            {
                await _bookRepository.AddAsync(book);
                return RedirectToAction("Index");
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(book);
        }
        //show detail of a book
        public async Task <ActionResult> Detail(int id)
        {
            var book= await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        //show update page of the book
        public async Task<IActionResult> Update(int id)
        {
            var book =await _bookRepository.GetByIdAsync(id);
            if(book == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", book.CategoryId);
            return View(book);
        }
        //Process the book update
        [HttpPost]
        public async Task<IActionResult> Update(int id, Book book)
        {
            if (id != book.ID)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                await _bookRepository.UpdateAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }
        //show the book delete congirmation
        public async Task<IActionResult> Delete(int id)
        {
            var book= await _bookRepository.GetByIdAsync(id);
            if( book == null )
            {
                return NotFound();
            }
            return View(book);
        }
        //process the book deletion
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
