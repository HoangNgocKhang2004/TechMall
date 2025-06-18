// ============================= CATEGORY SERVICE IMPL =============================
package com.example.service.impl;

import com.example.dto.CategoryDTO;
import com.example.entity.Category;
import com.example.exception.ResourceNotFoundException;
import com.example.repository.CategoryRepository;
import com.example.service.CategoryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class CategoryServiceImpl implements CategoryService {
    @Autowired private CategoryRepository repo;

    private CategoryDTO toDTO(Category e) {
        CategoryDTO d = new CategoryDTO();
        d.setId(e.getId()); d.setName(e.getName()); d.setImage(e.getImage()); d.setSlug(e.getSlug());
        d.setShowOnHomePage(e.isShowOnHomePage()); d.setDisplayOrder(e.getDisplayOrder());
        d.setCreatedAt(e.getCreatedAt()); d.setUpdatedAt(e.getUpdatedAt()); d.setDeleted(e.isDeleted());
        return d;
    }

    private Category toEntity(CategoryDTO d) {
        Category e = new Category();
        e.setId(d.getId()); e.setName(d.getName()); e.setImage(d.getImage()); e.setSlug(d.getSlug());
        e.setShowOnHomePage(d.isShowOnHomePage()); e.setDisplayOrder(d.getDisplayOrder());
        e.setCreatedAt(d.getCreatedAt()); e.setUpdatedAt(d.getUpdatedAt()); e.setDeleted(d.isDeleted());
        return e;
    }

    public List<CategoryDTO> getAll() { return repo.findAll().stream().map(this::toDTO).collect(Collectors.toList()); }
    public CategoryDTO getById(int id) { return toDTO(repo.findById(id).orElseThrow(() -> new ResourceNotFoundException("Category not found"))); }
    public CategoryDTO create(CategoryDTO d) { return toDTO(repo.save(toEntity(d))); }
    public CategoryDTO update(int id, CategoryDTO d) {
        Category e = repo.findById(id).orElseThrow(() -> new ResourceNotFoundException("Category not found"));
        e.setName(d.getName()); e.setImage(d.getImage()); e.setSlug(d.getSlug());
        e.setShowOnHomePage(d.isShowOnHomePage()); e.setDisplayOrder(d.getDisplayOrder());
        e.setUpdatedAt(d.getUpdatedAt()); e.setDeleted(d.isDeleted());
        return toDTO(repo.save(e));
    }
    public void delete(int id) { repo.deleteById(id); }
}