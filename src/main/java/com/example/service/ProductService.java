package com.example.service;

import com.example.dto.ProductDTO;
import java.util.List;

public interface ProductService {
    List<ProductDTO> getAllProducts();
    ProductDTO getProductById(Integer id);
    List<ProductDTO> searchProducts(String query);
}