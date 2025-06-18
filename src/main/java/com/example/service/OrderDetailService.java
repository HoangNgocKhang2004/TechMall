package com.example.service;

import com.example.entity.OrderDetail;
import java.util.List;

public interface OrderDetailService {
    List<OrderDetail> getAll();
    OrderDetail getById(Integer id);
    OrderDetail create(OrderDetail orderDetail);
    OrderDetail update(Integer id, OrderDetail orderDetail);
    void delete(Integer id);
    List<OrderDetail> getByOrderId(Integer orderId);

}
