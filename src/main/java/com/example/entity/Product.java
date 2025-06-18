package com.example.entity;

import jakarta.persistence.*;
import java.time.LocalDateTime;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
@Entity
public class Product {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    private String name;
    private String image;
    private String shortDes;
    private String fullDescription;
    private Boolean showOnHomePage = false;
    private Double price;
    private Double priceDiscount;
    private String slug;
    private Integer typeId;
    private LocalDateTime createdAt = LocalDateTime.now();
    private LocalDateTime updatedAt;
    private Boolean deleted = false;

    @ManyToOne
    @JoinColumn(name = "CategoryId")
    private Category category;

    @ManyToOne
    @JoinColumn(name = "BrandId")
    private Brand brand;

    // Getters and setters
}
