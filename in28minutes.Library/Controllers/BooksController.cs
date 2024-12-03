using AutoMapper;
using in28minutes.Library.Repository;
using Library.DTOs;
using Library.Enum;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace in28minutes.Library.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BooksController> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBooks()
    {
        _logger.LogInformation("Fetching all books from the database.");
        var books = await _unitOfWork.BookRepository.GetAllAsync();
        var booksResponse = _mapper.Map<IEnumerable<BookResponse>>(books);
        _logger.LogInformation("Successfully fetched {Count} books.", booksResponse.Count());
        return Ok(booksResponse);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBookId(Guid id)
    {
        _logger.LogInformation("Fetching book with ID: {BookId}", id);
        var book = await _unitOfWork.BookRepository.GetAsync(tmp => tmp.Id == id);
        if (book == null)
        {
            _logger.LogWarning("Book with ID: {BookId} not found.", id);
            return NotFound();
        }
        var bookResponse = _mapper.Map<BookResponse>(book);
        _logger.LogInformation("Successfully fetched book with ID: {BookId}", id);
        return Ok(bookResponse);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateBook([FromBody] BookAddRequest bookAddRequest)
    {
        _logger.LogInformation("Attempting to create a new book.");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for book creation request.");
            return BadRequest(ModelState);
        }
        var book = _mapper.Map<Book>(bookAddRequest);
        await _unitOfWork.BookRepository.AddAsync(book);
        _unitOfWork.SaveChangesAsync();
        var bookResponse = _mapper.Map<BookResponse>(book);
        _logger.LogInformation("Successfully created a new book with ID: {BookId}", book.Id);
        return Ok(bookResponse);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookUpdateRequest bookUpdateRequest)
    {
        _logger.LogInformation("Attempting to update book with ID: {BookId}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for book update request.");
            return BadRequest(ModelState);
        }

        var existingBook = await _unitOfWork.BookRepository.GetAsync(b => b.Id == id);
        if (existingBook == null)
        {
            _logger.LogWarning("Book with ID: {BookId} not found for update.", id);
            return NotFound("Book not found.");
        }

        _mapper.Map(bookUpdateRequest, existingBook);
        var updatedBook = await _unitOfWork.BookRepository.UpdateAsync(existingBook);
        _unitOfWork.SaveChangesAsync();
        var bookResponse = _mapper.Map<BookResponse>(updatedBook);

        _logger.LogInformation("Successfully updated book with ID: {BookId}", id);
        return Ok(bookResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        _logger.LogInformation("Attempting to delete book with ID: {BookId}", id);
        var book = await _unitOfWork.BookRepository.GetAsync(tmp => tmp.Id == id);
        if (book == null)
        {
            _logger.LogWarning("Book with ID: {BookId} not found for deletion.", id);
            return NotFound();
        }
        await _unitOfWork.BookRepository.RemoveAsync(book);
        _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Successfully deleted book with ID: {BookId}", id);
        return NoContent();
    }
}
