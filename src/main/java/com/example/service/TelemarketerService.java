package com.example.service;

import com.example.dto.TelemarketerDto;
import java.util.Map;

public interface TelemarketerService {
    Map<String, Object> registerTelemarketer(TelemarketerDto telemarketerDto);
    Map<String, Object> editTelemarketer(TelemarketerDto telemarketerDto);
    Map<String, Object> loadTelemarketerData();
    Map<String, Object> deleteTelemarketer(Long tmId);
    Map<String, Object> downloadData();
} 