package com.example.service.impl;

import com.example.dto.BrandDTO;
import com.example.entity.Brand;
import com.example.exception.ResourceNotFoundException;
import com.example.repository.BrandRepository;
import com.example.service.BrandService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class BrandServiceImpl implements BrandService {
    @Autowired private BrandRepository repo;

    private BrandDTO toDTO(Brand e) {
        BrandDTO d = new BrandDTO();
        d.setId(e.getId()); d.setName(e.getName()); d.setImage(e.getImage()); d.setSlug(e.getSlug());
        d.setShowOnHomePage(e.isShowOnHomePage()); d.setDisplayOrder(e.getDisplayOrder());
        d.setCreatedAt(e.getCreatedAt()); d.setUpdatedAt(e.getUpdatedAt()); d.setDeleted(e.isDeleted());
        return d;
    }

    private Brand toEntity(BrandDTO d) {
        Brand e = new Brand();
        e.setId(d.getId()); e.setName(d.getName()); e.setImage(d.getImage()); e.setSlug(d.getSlug());
        e.setShowOnHomePage(d.isShowOnHomePage()); e.setDisplayOrder(d.getDisplayOrder());
        e.setCreatedAt(d.getCreatedAt()); e.setUpdatedAt(d.getUpdatedAt()); e.setDeleted(d.isDeleted());
        return e;
    }

    public List<BrandDTO> getAll() { return repo.findAll().stream().map(this::toDTO).collect(Collectors.toList()); }
    public BrandDTO getById(int id) { return toDTO(repo.findById(id).orElseThrow(() -> new ResourceNotFoundException("Brand not found"))); }
    public BrandDTO create(BrandDTO d) { return toDTO(repo.save(toEntity(d))); }
    public BrandDTO update(int id, BrandDTO d) {
        Brand e = repo.findById(id).orElseThrow(() -> new ResourceNotFoundException("Brand not found"));
        e.setName(d.getName()); e.setImage(d.getImage()); e.setSlug(d.getSlug());
        e.setShowOnHomePage(d.isShowOnHomePage()); e.setDisplayOrder(d.getDisplayOrder());
        e.setUpdatedAt(d.getUpdatedAt()); e.setDeleted(d.isDeleted());
        return toDTO(repo.save(e));
    }
    public void delete(int id) { repo.deleteById(id); }
}
