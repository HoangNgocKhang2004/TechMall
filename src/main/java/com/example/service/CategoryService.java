// CategoryService.java
package com.example.service;

import com.example.dto.CategoryDTO;
import java.util.List;

public interface CategoryService {
    List<CategoryDTO> getAll();
    CategoryDTO getById(int id);
    CategoryDTO create(CategoryDTO dto);
    CategoryDTO update(int id, CategoryDTO dto);
    void delete(int id);
}
