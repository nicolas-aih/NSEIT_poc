package com.example.service;

import com.example.dto.TpslRequestDto;
import java.util.Map;

public interface TpslService {
    Map<String, Object> pgRequestT(TpslRequestDto requestDto);
    Map<String, Object> pgResponse();
} 