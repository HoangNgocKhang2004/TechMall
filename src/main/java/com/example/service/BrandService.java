// BrandService.java
package com.example.service;

import com.example.dto.BrandDTO;
import java.util.List;

public interface BrandService {
    List<BrandDTO> getAll();
    BrandDTO getById(int id);
    BrandDTO create(BrandDTO dto);
    BrandDTO update(int id, BrandDTO dto);
    void delete(int id);
}
