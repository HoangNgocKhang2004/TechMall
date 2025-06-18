// CategoryDTO.java
package com.example.dto;

import lombok.Data;
import java.util.Date;

@Data
public class CategoryDTO {
    private int id;
    private String name;
    private String image;
    private String slug;
    private boolean showOnHomePage;
    private int displayOrder;
    private Date createdAt;
    private Date updatedAt;
    private boolean deleted;
}
