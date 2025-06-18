package com.example.service.impl;

import com.example.dto.ProductDTO;
import com.example.entity.Product;
import com.example.exception.ResourceNotFoundException;
import com.example.repository.ProductRepository;
import com.example.service.ProductService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class ProductServiceImpl implements ProductService {

    @Autowired
    private ProductRepository productRepository;

    @Override
    public List<ProductDTO> searchProducts(String query) {
        List<Product> products = productRepository.findByNameContainingIgnoreCaseOrShortDesContainingIgnoreCase(query, query);
        return products.stream().map(this::mapToDTO).collect(Collectors.toList());
    }

    @Override
    public List<ProductDTO> getAllProducts() {
        return productRepository.findAll().stream().map(this::mapToDTO).collect(Collectors.toList());
    }

    @Override
    public ProductDTO getProductById(Integer id) {
        Product product = productRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Product not found"));
        return mapToDTO(product);
    }

    private ProductDTO mapToDTO(Product p) {
        ProductDTO dto = new ProductDTO();
        dto.setId(p.getId());
        dto.setName(p.getName());
        dto.setImage(p.getImage());
        dto.setShortDes(p.getShortDes());
        dto.setPrice(p.getPrice());
        dto.setPriceDiscount(p.getPriceDiscount());
        dto.setCategoryName(p.getCategory().getName());
        dto.setBrandName(p.getBrand().getName());
        return dto;
    }


}