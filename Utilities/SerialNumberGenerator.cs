using System;
using System.Linq;

namespace FridgeManagementSystem.Utilities
{
    public static class SerialNumberGenerator
    {
        /// <summary>
        /// Generates a unique serial number for fridges
        /// Format: BRANDCODE-TYPECODE-YYMM-SEQUENCE
        /// Example: LG-MF-2510-001 (LG Mini Fridge from Oct 2025, sequence 001)
        /// </summary>
        public static string GenerateFridgeSerialNumber(string brand, string type, int fridgeId)
        {
            if (string.IsNullOrEmpty(brand))
                brand = "UNK";
            if (string.IsNullOrEmpty(type))
                type = "UNK";

            // Get brand code (first 3 letters uppercase)
            string brandCode = brand.Length >= 3 ? brand.Substring(0, 3).ToUpper() : brand.ToUpper();

            // Get type code (first letter of each word)
            string typeCode = string.Join("", type.Split(' ')
                .Where(word => !string.IsNullOrEmpty(word))
                .Select(word => word[0].ToString().ToUpper()));

            // Get current year and month
            string yearMonth = DateTime.Now.ToString("yyMM");

            // Use fridgeId or generate random number if fridgeId is 0
            int uniqueId = fridgeId > 0 ? fridgeId : new Random().Next(100, 999);
            string sequence = uniqueId.ToString("D3");

            return $"{brandCode}-{typeCode}-{yearMonth}-{sequence}";
        }

        /// <summary>
        /// Generates a unique allocation number for fridge assignments
        /// Format: ALLOC-CUSTID-FRIDGEID-DATE
        /// Example: ALLOC-0001-0003-251027 (Customer 1, Fridge 3, Oct 27 2025)
        /// </summary>
        public static string GenerateAllocationNumber(int customerId, int fridgeId)
        {
            return $"ALLOC-{customerId:D4}-{fridgeId:D4}-{DateTime.Now:yyMMdd}";
        }

        /// <summary>
        /// Generates a unique return number for fridge returns
        /// Format: RET-ALLOCID-DATE
        /// Example: RET-0012-251027 (Allocation 12, Oct 27 2025)
        /// </summary>
        public static string GenerateReturnNumber(int allocationId)
        {
            return $"RET-{allocationId:D4}-{DateTime.Now:yyMMdd}";
        }

        /// <summary>
        /// Generates a purchase request number
        /// Format: PR-YEAR-SEQUENCE
        /// Example: PR-2025-042 (Purchase Request 42 in 2025)
        /// </summary>
        public static string GeneratePurchaseRequestNumber(int sequenceNumber, int year)
        {
            return $"PR-{year}-{sequenceNumber:D3}";
        }

        /// <summary>
        /// Generates a payment reference
        /// Format: PAY-RANDOM
        /// Example: PAY-A1B2C3D4E5
        /// </summary>
        public static string GeneratePaymentReference()
        {
            return "PAY-" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        }

        /// <summary>
        /// Generates a fault code
        /// Format: FLT-DATE-TIME
        /// Example: FLT-20251027-123045
        /// </summary>
        public static string GenerateFaultCode()
        {
            return "FLT-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
    }
}