﻿using System.ComponentModel.DataAnnotations;

namespace Library.DTOs;

public class CategoryAddRequest
{
    [StringLength(255, MinimumLength = 3)]
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
}
