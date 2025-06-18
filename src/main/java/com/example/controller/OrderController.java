package com.example.controller;

import com.example.entity.Order;
import com.example.service.OrderService;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/orders")
public class OrderController {
    private final OrderService service;

    public OrderController(OrderService service) {
        this.service = service;
    }

    @GetMapping
    public List<Order> getAll() {
        return service.getAll();
    }

    @GetMapping("/{id}")
    public Order getById(@PathVariable Integer id) {
        return service.getById(id);
    }

    @GetMapping("/user/{userId}")
    public List<Order> getByUserId(@PathVariable Integer userId) {
        return service.getByUserId(userId);
    }

    @PostMapping
    public Order create(@RequestBody Order order) {
        return service.create(order);
    }

    @PutMapping("/{id}")
    public Order update(@PathVariable Integer id, @RequestBody Order order) {
        return service.update(id, order);
    }

    @DeleteMapping("/{id}")
    public void delete(@PathVariable Integer id) {
        service.delete(id);
    }
    @GetMapping("/user/{userId}/count")
    public int getOrderCountByUserId(@PathVariable Integer userId) {
        return service.getByUserId(userId).size();
    }
    @GetMapping("/user/{userId}/orders")
    public List<Order> getOrdersByUserId(@PathVariable Integer userId) {
        return service.getByUserId(userId);
    }

}
