package com.example.controller;

import com.example.entity.OrderDetail;
import com.example.service.OrderDetailService;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/order-details")
public class OrderDetailController {
    private final OrderDetailService service;

    public OrderDetailController(OrderDetailService service) {
        this.service = service;
    }

    @GetMapping
    public List<OrderDetail> getAll() {
        return service.getAll();
    }

    @GetMapping("/{id}")
    public OrderDetail getById(@PathVariable Integer id) {
        return service.getById(id);
    }

    @PostMapping
    public OrderDetail create(@RequestBody OrderDetail orderDetail) {
        return service.create(orderDetail);
    }

    @PutMapping("/{id}")
    public OrderDetail update(@PathVariable Integer id, @RequestBody OrderDetail orderDetail) {
        return service.update(id, orderDetail);
    }

    @DeleteMapping("/{id}")
    public void delete(@PathVariable Integer id) {
        service.delete(id);
    }
    @GetMapping("/order/{orderId}")
    public List<OrderDetail> getByOrderId(@PathVariable Integer orderId) {
        return service.getByOrderId(orderId);
    }

}
