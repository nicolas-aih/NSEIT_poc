import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.Scanner;

public class ListSQLServerDatabases {

    public static void main(String[] args) {
        if (args.length >= 4) {
            // Use command line arguments if provided
            listDatabases(args[0], args[1], args[2], args[3]);
        } else {
            // Otherwise use interactive mode
            Scanner scanner = new Scanner(System.in);
            
            System.out.print("Server name (default: localhost): ");
            String server = scanner.nextLine().trim();
            server = server.isEmpty() ? "localhost" : server;
            
            System.out.print("Database name (default: master): ");
            String database = scanner.nextLine().trim();
            database = database.isEmpty() ? "master" : database;
            
            System.out.print("Username: ");
            String username = scanner.nextLine().trim();
            
            System.out.print("Password: ");
            String password = scanner.nextLine().trim();
            
            scanner.close();
            
            listDatabases(server, database, username, password);
        }
    }

    public static void listDatabases(String serverName, String databaseName, String user, String password) {
        // Connection URL for SQL Server authentication (username/password)
        String connectionUrl = "jdbc:sqlserver://" + "LAPTOP-AMFV5IUN" + ";databaseName=" + "AgencyLicensingPortal" + 
                              ";user=" + "sa" + 
                              ";password=" + "sa123" + 
                              ";encrypt=true;trustServerCertificate=true;";

        try (Connection con = DriverManager.getConnection(connectionUrl);
             Statement stmt = con.createStatement();
             ResultSet rs = stmt.executeQuery("SELECT name FROM sys.databases")) {

            System.out.println("Connected to SQL Server!");
            System.out.println("Databases:");

            while (rs.next()) {
                System.out.println(rs.getString("name"));
            }

        } catch (SQLException e) {
            System.err.println("Database connection error: " + e.getMessage());
            e.printStackTrace();
        }
    }
}
