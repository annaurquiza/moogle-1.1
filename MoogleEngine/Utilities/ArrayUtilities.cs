public static class ArrayUtilities
{
    public static int FindSmallestDifference(int []A, int []B)
        {            
            Array.Sort(A);
            Array.Sort(B);
        
            int a = 0, b = 0, m = A.Length, n = B.Length;
        
            int result = int.MaxValue;
        
            while (a < m && b < n)
            {
                if (Math.Abs(A[a] - B[b]) < result)
                    result = Math.Abs(A[a] - B[b]);
        
                if (A[a] < B[b])
                    a++;
        
                else
                    b++;
            }
            
            return result;
        }
}

