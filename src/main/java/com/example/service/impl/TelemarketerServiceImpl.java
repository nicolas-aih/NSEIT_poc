package com.example.service.impl;

import com.example.dto.TelemarketerDto;
import com.example.service.TelemarketerService;
import org.springframework.stereotype.Service;
import java.util.Map;

@Service
public class TelemarketerServiceImpl implements TelemarketerService {
    @Override
    public Map<String, Object> registerTelemarketer(TelemarketerDto telemarketerDto) {
        return Map.of("success", true, "message", "Stub: registerTelemarketer");
    }

    @Override
    public Map<String, Object> editTelemarketer(TelemarketerDto telemarketerDto) {
        return Map.of("success", true, "message", "Stub: editTelemarketer");
    }

    @Override
    public Map<String, Object> loadTelemarketerData() {
        return Map.of("success", true, "message", "Stub: loadTelemarketerData");
    }

    @Override
    public Map<String, Object> deleteTelemarketer(Long tmId) {
        return Map.of("success", true, "message", "Stub: deleteTelemarketer");
    }

    @Override
    public Map<String, Object> downloadData() {
        return Map.of("success", true, "message", "Stub: downloadData");
    }
} 