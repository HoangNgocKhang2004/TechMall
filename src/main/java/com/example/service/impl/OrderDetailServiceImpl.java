package com.example.service.impl;

import com.example.entity.OrderDetail;
import com.example.repository.OrderDetailRepository;
import com.example.service.OrderDetailService;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class OrderDetailServiceImpl implements OrderDetailService {
    private final OrderDetailRepository repository;

    public OrderDetailServiceImpl(OrderDetailRepository repository) {
        this.repository = repository;
    }

    @Override
    public List<OrderDetail> getAll() {
        return repository.findAll();
    }

    @Override
    public OrderDetail getById(Integer id) {
        return repository.findById(id).orElse(null);
    }

    @Override
    public OrderDetail create(OrderDetail orderDetail) {
        return repository.save(orderDetail);
    }

    @Override
    public OrderDetail update(Integer id, OrderDetail orderDetail) {
        if (!repository.existsById(id)) return null;
        orderDetail.setId(id);
        return repository.save(orderDetail);
    }

    @Override
    public void delete(Integer id) {
        repository.deleteById(id);
    }
    @Override
    public List<OrderDetail> getByOrderId(Integer orderId) {
        return repository.findByOrderId(orderId);
    }

}
