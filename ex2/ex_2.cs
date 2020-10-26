using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex2
{
    /* 1. Define an enum called AccountType with the values Checking and Deposit. 
     *    Define two AccountType variables, one for Checking and one for Deposit.
     * 2. Define a struct called BankAccount having the following variables: 
     *    Owner, amount of money in the account and an AccountType variable.
     * 3. Create a BankAccount variable, initialize its variables and print them.
     * 4. Write a method named SumOfBalances which receives an unknown number of BankAccount arguments, 
     *    and returns the sum of all amounts of money in all accounts.
     * 5. Create a collection of BankAccount variables. Write two methods finding the accounts with the largest 
     *    and smallest amounts of money, GetRichestAccount and GetPoorestAccount. Sort the accounts in an increasing 
     *    order.
     * 6. Write a method called AddMoney which receives a BankAccount argument by reference, as well as an integer 
     *    representing an amount of money, that will add to the BankAccount amount of money this integer amount.
     * 7. Add to BankAccount the operator + so we could use it to directly update the amount of money in the account.
     * 8. Write a method called IsPalindrome which receives an integer and checks whether it is a palindrome.
     */
    class Program
    {
        // 1
        enum AccountType { Checking, Deposit }

        // 2
        struct BankAccount : IComparable<BankAccount>
        { 
            internal string owner; 
            internal int moneyInAccount; 
            internal AccountType accountType; 
            // Made comparable to easily sort and find minimum/maximum.
            public int CompareTo(BankAccount other)
            {
                return moneyInAccount.CompareTo(other.moneyInAccount) > 0 ? 1 : -1;
            }
            // 7
            public static BankAccount operator +(BankAccount account, int money)
            {
                BankAccount newAccount = new BankAccount
                {
                    owner = account.owner,
                    moneyInAccount = account.moneyInAccount + money,
                    accountType = account.accountType
                };
                return newAccount;
            }
            public override string ToString()
            {
                return $"Owner: {owner}\nMoney In Account: {moneyInAccount}\nAccount Type: {accountType}";
            }
        }

        // 4
        static int SumOfBalances(params BankAccount[] accounts)
        {
            int sum = 0;
            if (accounts == null) return sum;
            foreach (BankAccount accountElement in accounts)
            {
                sum += accountElement.moneyInAccount;
            }
            return sum;
        }

        // 5
        static BankAccount GetRichestAccount(List<BankAccount> accounts)
        {
            return accounts.Max();
        }
        static BankAccount GetPoorestAccount(List<BankAccount> accounts)
        {
            return accounts.Min();
        }

        // 6
        static void AddMoney(ref BankAccount account, int money) { account.moneyInAccount += money; }

        // 8
        static bool IsPalindrome(int num)
        {
            string strnum = num.ToString();
            for (int i=0; i < (strnum.Length + 1) / 2; i++)
            {
                if (!(strnum[i] == strnum[strnum.Length - i - 1])) return false;
            }
            return true;
        }

        static void Main()
        {
            // 1 
            AccountType check = AccountType.Checking;
            AccountType deposit = AccountType.Deposit;
            // 3
            BankAccount account0 = new BankAccount 
            {
                owner = "John Doe",
                moneyInAccount = 10,
                accountType = AccountType.Checking
            };
            Console.WriteLine(account0.ToString());
            // Test 4
            BankAccount account1 = new BankAccount
            {
                owner = "1",
                moneyInAccount = 10,
                accountType = AccountType.Checking
            };
            BankAccount account2 = new BankAccount
            {
                owner = "2",
                moneyInAccount = 20,
                accountType = AccountType.Checking
            };
            Console.WriteLine(SumOfBalances(account1, account2));
            BankAccount[] noAccounts = null;
            Console.WriteLine(SumOfBalances(noAccounts));  // If null, returns 0.
            // 5
            int numAccounts = 10;
            List<BankAccount> accounts = new List<BankAccount>();
            for (int i=0; i<numAccounts; i++)
            {
                BankAccount tempAccount = new BankAccount()
                {
                    owner = $"Owner Number {i + 1}",
                    moneyInAccount = new Random(i).Next(1000),
                    accountType = AccountType.Deposit
                };
                accounts.Add(tempAccount);
            }
            Console.WriteLine("Money in accounts before sorting:");
            foreach (BankAccount bankAccount in accounts) Console.Write($"{bankAccount.moneyInAccount} ");
            Console.Write("\n");
            BankAccount richest = GetRichestAccount(accounts);
            Console.WriteLine("The richest account:");
            Console.WriteLine(richest.ToString());
            BankAccount poorest = GetPoorestAccount(accounts);
            Console.WriteLine("The poorest account:");
            Console.WriteLine(poorest.ToString());
            accounts.Sort();
            Console.WriteLine("Sorted money in accounts:");
            foreach (BankAccount bankAccount in accounts) Console.Write($"{bankAccount.moneyInAccount} ");
            Console.Write("\n");
            // Test 6
            AddMoney(ref account0, 10);
            Console.WriteLine(account0.ToString());
            // Test 7
            account0 += 10;
            Console.WriteLine(account0.ToString());
            // Test 8
            string s1 = (IsPalindrome(12321) ? "is a palindrome" : "is not a palindrome");
            Console.WriteLine("12321 " + s1);
            string s2 = (IsPalindrome(21) ? "is a palindrome" : "is not a palindrome");
            Console.WriteLine("21 " + s2);
            string s3 = (IsPalindrome(3223) ? "is a palindrome" : "is not a palindrome");
            Console.WriteLine("3223 " + s3);
        }
    }
}
