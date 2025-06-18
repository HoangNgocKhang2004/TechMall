package com.example.service;

import com.example.entity.Order;
import java.util.List;

public interface OrderService {
    List<Order> getAll();
    Order getById(Integer id);
    Order create(Order order);
    Order update(Integer id, Order order);
    void delete(Integer id);
    List<Order> getByUserId(Integer userId);

}
