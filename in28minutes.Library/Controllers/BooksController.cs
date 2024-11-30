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
    public BooksController(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task <IActionResult> GetAllBooks()
    {
        var books = await _unitOfWork.BookRepository.GetAllAsync();
        var booksRepsonse = _mapper.Map<IEnumerable<BookResponse>>(books);
        return Ok(booksRepsonse);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetBookId(Guid id)
    {
        var book = await _unitOfWork.BookRepository.GetAsync(tmp=>tmp.Id == id);
        if(book == null)
        {
            return NotFound();
        }
        var bookResponse = _mapper.Map<BookResponse>(book);
        return Ok(bookResponse);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateBook([FromBody] BookAddRequest bookAddRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var book = _mapper.Map<Book>(bookAddRequest);
        await _unitOfWork.BookRepository.AddAsync(book);
        _unitOfWork.SaveChangesAsync();
        var bookResponse = _mapper.Map<BookResponse>(book);
        return Ok(bookResponse);
    }


}
