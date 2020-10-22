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
    class Answers
    {
        // 1
        public enum AccountType { Checking, Deposit }

        // 2
        public struct BankAccount : IComparable<BankAccount>
        { 
            public string owner; public int moneyInAccount; public AccountType accType; 
            // Made comparable to easily sort and find minimum/maximum.
            public int CompareTo(BankAccount other)
            {
                return moneyInAccount.CompareTo(other.moneyInAccount) > 0 ? 1 : -1;
            }
            // 7
            public static BankAccount operator +(BankAccount acc, int money)
            {
                BankAccount newAcc = new BankAccount
                {
                    owner = acc.owner,
                    moneyInAccount = acc.moneyInAccount + money,
                    accType = acc.accType
                };
                return newAcc;
            }

        }

        // Helping function to print BankAccount elements, a common action.
        static void PrintAccount(BankAccount acc)
        {
            Console.WriteLine("Owner: {0}\nMoney In Account: {1}\nAccount Type: {2}",
                acc.owner, acc.moneyInAccount, acc.accType);
        }

        // 4
        static int SumOfBalances(params BankAccount[] accounts)
        {
            int sum = 0;
            foreach (BankAccount acc in accounts)
            {
                sum += acc.moneyInAccount;
            }
            return sum;
        }

        // 5
        static void GetRichestAccount(List<BankAccount> accounts)
        {
            BankAccount richest = accounts.Max();
            Console.WriteLine("The richest account:");
            PrintAccount(richest);
        }
        static void GetPoorestAccount(List<BankAccount> accounts)
        {
            BankAccount richest = accounts.Min();
            Console.WriteLine("The poorest account:");
            PrintAccount(richest);
        }

        // 6
        static void AddMoney(ref BankAccount acc, int money) { acc.moneyInAccount += money; }

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
            AccountType depo = AccountType.Deposit;
            // 3
            BankAccount account = new BankAccount 
            {
                owner = "John Doe",
                moneyInAccount = 10,
                accType = AccountType.Checking
            };
            PrintAccount(account);
            // Test 4
            BankAccount acc1 = new BankAccount
            {
                owner = "1",
                moneyInAccount = 10,
                accType = AccountType.Checking
            };
            BankAccount acc2 = new BankAccount
            {
                owner = "2",
                moneyInAccount = 20,
                accType = AccountType.Checking
            };
            Console.WriteLine(SumOfBalances(acc1, acc2));
            // 5
            int numAccounts = 10;
            List<BankAccount> accounts = new List<BankAccount>();
            for (int i=0; i<numAccounts; i++)
            {
                BankAccount tempAccount = new BankAccount();
                tempAccount.owner = $"Owner Number {i + 1}";
                Random r1 = new Random(i);
                tempAccount.moneyInAccount = r1.Next(1000);
                tempAccount.accType = AccountType.Deposit;
                accounts.Add(tempAccount);
            }
            Console.WriteLine("Money in accounts before sorting:");
            foreach (BankAccount bac in accounts) Console.Write($"{bac.moneyInAccount} ");
            Console.Write("\n");
            GetRichestAccount(accounts);
            GetPoorestAccount(accounts);
            accounts.Sort();
            Console.WriteLine("Sorted money in accounts:");
            foreach (BankAccount bac in accounts) Console.Write($"{bac.moneyInAccount} ");
            Console.Write("\n");
            // Test 6
            AddMoney(ref account, 10);
            PrintAccount(account);
            // Test 7
            account += 10;
            PrintAccount(account);
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
