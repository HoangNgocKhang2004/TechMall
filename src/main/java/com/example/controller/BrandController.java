package com.example.controller;

import com.example.dto.BrandDTO;
import com.example.service.BrandService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/brands")
public class BrandController {

    @Autowired
    private BrandService brandService;

    @GetMapping
    public List<BrandDTO> getAll() {
        return brandService.getAll();
    }

    @GetMapping("/{id}")
    public BrandDTO getById(@PathVariable int id) {
        return brandService.getById(id);
    }

    @PostMapping
    public BrandDTO create(@RequestBody BrandDTO dto) {
        return brandService.create(dto);
    }

    @PutMapping("/{id}")
    public BrandDTO update(@PathVariable int id, @RequestBody BrandDTO dto) {
        return brandService.update(id, dto);
    }

    @DeleteMapping("/{id}")
    public void delete(@PathVariable int id) {
        brandService.delete(id);
    }
}
