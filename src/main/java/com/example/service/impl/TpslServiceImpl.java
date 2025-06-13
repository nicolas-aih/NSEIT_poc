package com.example.service.impl;

import com.example.dto.TpslRequestDto;
import com.example.service.TpslService;
import org.springframework.stereotype.Service;
import java.util.Map;

@Service
public class TpslServiceImpl implements TpslService {
    @Override
    public Map<String, Object> pgRequestT(TpslRequestDto requestDto) {
        return Map.of("success", true, "message", "Stub: pgRequestT");
    }

    @Override
    public Map<String, Object> pgResponse() {
        return Map.of("success", true, "message", "Stub: pgResponse");
    }
} 