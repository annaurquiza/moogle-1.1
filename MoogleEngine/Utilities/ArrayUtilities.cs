public static class ArrayUtilities
{
    public static int findSmallestDifference(int []A, int []B)
        {
            
            // Sort both arrays using
            // sort function
            Array.Sort(A);
            Array.Sort(B);
        
            int a = 0, b = 0, m = A.Length, n = B.Length;
        
            // Initialize result as max value
            int result = int.MaxValue;
        
            // Scan Both Arrays upto
            // sizeof of the Arrays
            while (a < m && b < n)
            {
                if (Math.Abs(A[a] - B[b]) < result)
                    result = Math.Abs(A[a] - B[b]);
        
                // Move Smaller Value
                if (A[a] < B[b])
                    a++;
        
                else
                    b++;
            }
            
            // return final sma result
            return result;
        }
}

