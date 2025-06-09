package utilities;

// Stub for Common class holding constants
public class Common {
    // Example regex - you need to ensure these are correct Java regex patterns
    // C# low ASCII: Matches characters outside standard printable ASCII range (32-126), plus tab, newline, carriage return
    public static final String regexLowAscii = "[\\x00-\\x08\\x0B\\x0C\\x0E-\\x1F\\x7F]";
    // C# email regex - simplified for example, real validation is complex
    public static final String regexEmail = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";

     // Note: These regex patterns might need adjustment to perfectly match C# behavior.
}