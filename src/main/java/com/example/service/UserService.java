// UserService.java
package com.example.service;

import com.example.dto.UserDTO;
import java.util.List;

public interface UserService {
    List<UserDTO> getAll();
    UserDTO getById(int id);
    UserDTO create(UserDTO dto);
    UserDTO update(int id, UserDTO dto);
    void delete(int id);
    UserDTO findByEmailAndPassword(String email, String password);
}
