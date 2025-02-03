using Entities;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public char? Gender { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public PersonUpdateRequest ToUpdateRequest() => new PersonUpdateRequest()
        {
            PersonId = this.PersonId, 
            FirstName = this.FirstName,
            LastName = this.LastName,
            DateOfBirth = this.DateOfBirth,
            Gender = this.Gender,
            CountryId = this.CountryId,
            ReceiveNewsLetters = this.ReceiveNewsLetters,
        };

        public Person ToPerson() => new Person()
        {
            PersonId = this.PersonId,
            FirstName = this.FirstName,
            LastName = this.LastName,
            Email = this.Email,
            DateOfBirth = this.DateOfBirth,
            Gender = this.Gender,
            CountryId = this.CountryId,
            Country=new Country()
            {
                CountryId= this.CountryId.HasValue? this.CountryId.Value : 0, 
                CountryName=this.CountryName != null ? this.CountryName: null
            },
            ReceiveNewsLetters = this.ReceiveNewsLetters,
        };


        public static class PersonExtensions
        {
            private static int? GetAge(DateTime? dateTime)
            {
                if (dateTime == null) return null;

                int age = new DateTime().Year - dateTime.Value.Year;

                if (dateTime.Value.AddYears(age) > new DateTime()) age--; 
                return age;
            }
        }
    }
}

public class Solution
{
    public int GetUniqueNumbersCount(int[] arr)
    {
        int total = 1;
        int n = arr.Length;

        for (int i = 1; i < n; i++)
            if (arr[i] != arr[i - 1]) total++;
        return total;
    }


    public int DeleteAndEarn(int[] arr)
    {
        // element should be sorted in ascending order 
        // for the following steps and the algorithm to work 
        Array.Sort(arr);

        // get the count of unique elements 
        int n = GetUniqueNumbersCount(arr);

        // delcare two arrays one for storing the unique elements 
        // and another array for storing the frequency of each element 
        var nums = new int[n];
        var numsFrequency = new int[n];

        int c = -1;
        nums[++c] = arr[0];
        numsFrequency[c] = 1;
        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] != arr[i - 1])
            {
                nums[++c] = arr[i];
                numsFrequency[c] = 1;
            }
            else
                numsFrequency[c]++;
        }

        int[] dp = new int[n];
        Array.Fill(dp, -1);

        return Solver(0);

        int Solver(int idx)
        {
            if (idx >= n)
                return 0;

            if (dp[idx] != -1) return dp[idx];
            int max = 0;

            //int pick = 0;
            // idx + 1 cannot be less than idx 
            // idx + 1 cannot equal idx 
            // only possible options are, there is not next or 
            // next is equal to nums[idx] + 1 or next is greater than nums[idx] + 1
            if (idx == n - 1 || nums[idx + 1] > nums[idx] + 1)
            {
                max = nums[idx] * numsFrequency[idx] + Solver(idx + 1);
            }
            else
            {
                // pick, skip the next element 
                max = nums[idx] * numsFrequency[idx] + Solver(idx + 2);

                // leave the current element 
                max = Math.Max(max, Solver(idx + 1));
            }

            dp[idx] = max;
            return max;
        }
    }
}

