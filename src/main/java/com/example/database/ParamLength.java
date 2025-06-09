package com.example.database;

public class ParamLength {
    private int defaultLength;
    private int maxLength;
    private int minLength;

    public ParamLength(int defaultLength, int maxLength, int minLength) {
        this.defaultLength = defaultLength;
        this.maxLength = maxLength;
        this.minLength = minLength;
    }

    // Getters and setters
    public int getDefaultLength() {
        return defaultLength;
    }

    public void setDefaultLength(int defaultLength) {
        this.defaultLength = defaultLength;
    }

    public int getMaxLength() {
        return maxLength;
    }

    public void setMaxLength(int maxLength) {
        this.maxLength = maxLength;
    }

    public int getMinLength() {
        return minLength;
    }

    public void setMinLength(int minLength) {
        this.minLength = minLength;
    }
}