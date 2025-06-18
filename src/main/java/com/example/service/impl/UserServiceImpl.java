package com.example.service.impl;

import com.example.dto.UserDTO;
import com.example.entity.User;
import com.example.exception.ResourceNotFoundException;
import com.example.repository.UserRepository;
import com.example.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;
import java.math.BigInteger;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

@Service
public class UserServiceImpl implements UserService {
    @Autowired private UserRepository repo;

    private UserDTO toDTO(User e) {
        UserDTO d = new UserDTO();
        d.setId(e.getId());
        d.setFirstName(e.getFirstName());
        d.setLastName(e.getLastName());
        d.setEmail(e.getEmail());
        d.setPassword(e.getPassword());
        d.setAdmin(e.isAdmin());
        d.setCreatedAt(e.getCreatedAt());
        return d;
    }


    private User toEntity(UserDTO d) {
        User e = new User();
        e.setId(d.getId());
        e.setFirstName(d.getFirstName());
        e.setLastName(d.getLastName());
        e.setEmail(d.getEmail());
        e.setAdmin(d.isAdmin());
        e.setCreatedAt(d.getCreatedAt());
        e.setPassword(d.getPassword());
        return e;
    }


    public List<UserDTO> getAll() { return repo.findAll().stream().map(this::toDTO).collect(Collectors.toList()); }
    public UserDTO getById(int id) { return toDTO(repo.findById(id).orElseThrow(() -> new ResourceNotFoundException("User not found"))); }
    public UserDTO create(UserDTO d) {
        // Mã hóa mật khẩu trước khi lưu
        d.setPassword(MD5(d.getPassword()));
        return toDTO(repo.save(toEntity(d)));
    }

    private String MD5(String input) {
        if (input == null || input.trim().isEmpty()) {
            throw new IllegalArgumentException("Password không được để trống (null)");
        }

        try {
            MessageDigest md = MessageDigest.getInstance("MD5");
            byte[] messageDigest = md.digest(input.getBytes());
            BigInteger number = new BigInteger(1, messageDigest);
            String hashText = number.toString(16);
            while (hashText.length() < 32) {
                hashText = "0" + hashText;
            }
            return hashText;
        } catch (NoSuchAlgorithmException e) {
            throw new RuntimeException(e);
        }
    }


    public UserDTO update(int id, UserDTO d) {
        User e = repo.findById(id).orElseThrow(() -> new ResourceNotFoundException("User not found"));
        e.setFirstName(d.getFirstName()); e.setLastName(d.getLastName()); e.setEmail(d.getEmail());
        e.setAdmin(d.isAdmin()); return toDTO(repo.save(e));
    }
    public void delete(int id) { repo.deleteById(id); }

    @Override
    public UserDTO findByEmailAndPassword(String email, String password) {
        User user = repo.findByEmailAndPassword(email, password);
        if (user != null) {
            return new UserDTO(user); // <-- Kiểm tra constructor có đúng không
        }
        return null;
    }
}
