package com.example.service.impl;

import com.example.entity.Order;
import com.example.repository.OrderRepository;
import com.example.service.OrderService;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class OrderServiceImpl implements OrderService {
    private final OrderRepository repository;

    public OrderServiceImpl(OrderRepository repository) {
        this.repository = repository;
    }

    @Override
    public List<Order> getAll() {
        return repository.findAll();
    }

    @Override
    public Order getById(Integer id) {
        return repository.findById(id).orElse(null);
    }

    @Override
    public Order create(Order order) {
        return repository.save(order);
    }

    @Override
    public Order update(Integer id, Order order) {
        if (!repository.existsById(id)) return null;
        order.setId(id);
        return repository.save(order);
    }

    @Override
    public void delete(Integer id) {
        repository.deleteById(id);
    }

    @Override
    public List<Order> getByUserId(Integer userId) {
        return repository.findByUserId(userId);
    }

}
