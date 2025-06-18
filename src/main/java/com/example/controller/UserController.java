package com.example.controller;

import com.example.dto.UserDTO;
import com.example.dto.LoginRequest;
import com.example.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.math.BigInteger;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.List;

@RestController
@RequestMapping("/api/users")
public class UserController {

    @Autowired
    private UserService userService;

    // ✅ Đặt trước để tránh nhầm "login" thành id
    @PostMapping("/login")
    public ResponseEntity<?> login(@RequestBody LoginRequest loginRequest) {
        String hashed = MD5(loginRequest.getPassword());

        UserDTO user = userService.findByEmailAndPassword(
                loginRequest.getEmail(),
                hashed
        );

        if (user != null) {
            return ResponseEntity.ok(user);
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body("Sai tài khoản hoặc mật khẩu");
        }
    }

    @GetMapping
    public List<UserDTO> getAll() {
        return userService.getAll();
    }

    @PostMapping
    public UserDTO create(@RequestBody UserDTO dto) {
        return userService.create(dto);
    }

    // ❗ Đặt sau login để không nhầm đường dẫn
    @GetMapping("/{id}")
    public UserDTO getById(@PathVariable int id) {
        return userService.getById(id);
    }

    @PutMapping("/{id}")
    public UserDTO update(@PathVariable int id, @RequestBody UserDTO dto) {
        return userService.update(id, dto);
    }

    @DeleteMapping("/{id}")
    public void delete(@PathVariable int id) {
        userService.delete(id);
    }

    private String MD5(String input) {
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
}
