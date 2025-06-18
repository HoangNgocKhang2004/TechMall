package com.example.dto;

import lombok.Data;

@Data
public class ProductDTO {
    private Integer id;
    private String name;
    private String image;
    private String shortDes;
    private Double price;
    private Double priceDiscount;
    private String categoryName;
    private String brandName;
}