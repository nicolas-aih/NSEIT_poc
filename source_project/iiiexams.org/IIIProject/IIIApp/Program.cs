using System;
using IIIBL;
using System.Data;

namespace IIIApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Testing with real database layer");
                
                var batchMgmt = new BatchMgmt();
                string connString = "Your_Actual_Connection_String";
                
                // Test with a sample transaction ID
                TestVerifyBatch(batchMgmt, connString, "TXN12345");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        static void TestVerifyBatch(BatchMgmt batchMgmt, string connString, string transactionId)
        {
            Console.WriteLine($"\nVerifying batch {transactionId}");
            
            batchMgmt.VerifyBatch(
                connString,
                transactionId,
                out string paymentMode,
                out decimal totalAmount,
                out string message,
                out bool canProceed);
                
            Console.WriteLine($"Results: {message}");
            Console.WriteLine($"Payment Mode: {paymentMode}");
            Console.WriteLine($"Amount: {totalAmount}");
            Console.WriteLine($"Can Proceed: {canProceed}");
        }
    }
}