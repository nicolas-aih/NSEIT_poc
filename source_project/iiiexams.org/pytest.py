import pyodbc

def list_databases():
    try:
        conn_str = (
            "DRIVER={ODBC Driver 17 for SQL Server};"
            "SERVER=localhost;"
            "Trusted_Connection=yes;"
        )

        with pyodbc.connect(conn_str, timeout=5) as conn:
            cursor = conn.cursor()
            cursor.execute("SELECT name FROM sys.databases")
            databases = [row.name for row in cursor.fetchall()]
            return databases

    except Exception as e:
        print("Connection failed:", e)
        return []

if __name__ == "__main__":
    dbs = list_databases()
    print("Databases found:")
    for db in dbs:
        
        print(" -", db)
