package com.example.dto;

import com.example.entity.User;
import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.Date;

public class UserDTO {
    private int id;
    private String firstName;
    private String lastName;
    private String email;

    // Cho phép nhận password khi client gửi JSON, nhưng không hiển thị khi trả về
    @JsonProperty(access = JsonProperty.Access.WRITE_ONLY)
    private String password;

    private boolean admin;
    private Date createdAt;

    // === Constructor rỗng (bắt buộc khi dùng Jackson hoặc Spring) ===
    public UserDTO() {}

    // === Constructor tạo từ entity User ===
    public UserDTO(User user) {
        this.id = user.getId();
        this.firstName = user.getFirstName();
        this.lastName = user.getLastName();
        this.email = user.getEmail();
        this.password = user.getPassword(); // vẫn lưu trong DTO để xử lý nội bộ nếu cần
        this.admin = user.isAdmin();
        this.createdAt = user.getCreatedAt();
    }

    // === Getters & Setters ===
    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public boolean isAdmin() {
        return admin;
    }

    public void setAdmin(boolean admin) {
        this.admin = admin;
    }

    public Date getCreatedAt() {
        return createdAt;
    }

    public void setCreatedAt(Date createdAt) {
        this.createdAt = createdAt;
    }
}
