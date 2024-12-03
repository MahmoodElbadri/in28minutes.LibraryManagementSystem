using AutoMapper;
using in28minutes.Library.Repository;
using Library.DTOs;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace in28minutes.Library.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoriesController> _logger;
    public CategoriesController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<CategoriesController> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategories()
    {
        _logger.LogInformation("Fetching all categories from the database.");
        var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
        var categoriesResponse = _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        _logger.LogInformation("Successfully fetched {Count} categories.", categoriesResponse.Count());
        return Ok(categoriesResponse);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        _logger.LogInformation("Fetching category with ID: {CategoryId}", id);
        var category = await _unitOfWork.CategoryRepository.GetAsync(tmp => tmp.Id == id);
        if (category == null)
        {
            _logger.LogWarning("Category with ID: {CategoryId} not found.", id);
            return NotFound();
        }
        var categoryResponse = _mapper.Map<CategoryResponse>(category);
        _logger.LogInformation("Successfully fetched category with ID: {CategoryId}", id);
        return Ok(categoryResponse);
    }

    //[Authorize(Roles = "ADMIN")]
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryAddRequest categoryAddRequest)
    {
        _logger.LogInformation("Attempting to create a new category.");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for category creation request.");
            return BadRequest(ModelState);
        }
        var category = _mapper.Map<Category>(categoryAddRequest);
        await _unitOfWork.CategoryRepository.AddAsync(category);
        _unitOfWork.SaveChangesAsync();
        var categoryResponse = _mapper.Map<CategoryResponse>(category);
        _logger.LogInformation("Successfully created a new category with ID: {CategoryId}", category.Id);
        return Ok(categoryResponse);
    }
    [AllowAnonymous]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryUpdateRequest categoryUpdateRequest)
    {
        _logger.LogInformation("Attempting to update category with ID: {CategoryId}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for category update request.");
            return BadRequest(ModelState);
        }
        var category = _mapper.Map<Category>(categoryUpdateRequest);
        var updatedCategory = await _unitOfWork.CategoryRepository.UpdateAsync(category);
        var categoryResponse = _mapper.Map<CategoryResponse>(updatedCategory);
        _logger.LogInformation("Successfully updated category with ID: {CategoryId}", id);
        return Ok(categoryResponse);
    }

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        _logger.LogWarning("Attempting to delete category with ID: {CategoryId}", id);
        var category = await _unitOfWork.CategoryRepository.GetAsync(tmp => tmp.Id == id);
        if (category == null)
        {
            _logger.LogWarning("Category with ID: {CategoryId} not found for deletion.", id);
            return NotFound();
        }
        await _unitOfWork.CategoryRepository.RemoveAsync(category);
        _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Successfully deleted category with ID: {CategoryId}", id);
        return NoContent();

    }
}
