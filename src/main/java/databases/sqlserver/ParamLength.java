package databases.sqlserver;

// Helper class equivalent to C#'s ParamLength struct/class
public class ParamLength {
    private int size;
    private int precision;
    private int scale;

    public ParamLength(int size, int precision, int scale) {
        this.size = size;
        this.precision = precision;
        this.scale = scale;
    }

    public int getSize() { return size; }
    public int getPrecision() { return precision; }
    public int getScale() { return scale; }

    // Optional: add setters if needed, but constructor is sufficient based on C# usage
}