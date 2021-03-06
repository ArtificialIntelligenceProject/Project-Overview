﻿using System;

namespace ConsoleApp4
{
    public class Customer
    {

        public string Name { get; set; }
        public bool IsNationalityIsraeli { get; set; }
        public decimal TravelExpense { get; set; }
        public int Daystovisit { get; set; }
        public bool WanttovisitNorthernareas { get; set; }
        public bool CriminalRecord { get; set; }
        public bool MedicalIssues { get; set; }
        public bool WanttovisitSouthernareas { get; set; }
        public bool MedicalIssues2 { get; set; }
        //public Customer(string name, bool isNationalityUK, decimal travelExpense, int citiestovisit, bool wanttovisitNorthernareas, bool criminalRecord)
        //{
        //    Name = name;
        //    IsNationalityUK = isNationalityUK;
        //    TravelExpense = travelExpense; 
        //    Citiestovisit = citiestovisit;
        //    WanttovisitNorthernareas = wanttovisitNorthernareas;
        //    CriminalRecord = criminalRecord;
        //}
    }
    public abstract class Decision
    {
        public abstract void Evaluate(Customer Customer);
    }
    public class DecisionQuery : Decision
    {
        public string Title { get; set; }
        public Decision Positive { get; set; }
        public Decision Negative { get; set; }
        public Func<Customer, bool> Test { get; set; }

        public override void Evaluate(Customer Customer)
        {
            bool result = this.Test(Customer);
            string resultAsString = result ? "yes" : "no";

            Console.WriteLine($"{this.Title}? {resultAsString}");

            if (result) this.Positive.Evaluate(Customer);
            else this.Negative.Evaluate(Customer);
        }
    }
    public class DecisionResult : Decision
    {
        static void Main(string[] args)
        {

            var trunk = MainDecisionTree();

            Console.WriteLine("What is your name?");
            string name = Console.ReadLine();
            Console.WriteLine("Are you Israel National? Y/N");
            bool isIsraeli = Console.ReadLine().Equals("Y") ? true : false;
            Console.WriteLine("Do you have a criminal record? Y/N ");
            bool crimincalRecord = Console.ReadLine().Equals("Y") ? true : false;
            Console.WriteLine("How much are you willing to spend?");
            int travelExpense = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("How many days you can holiday for?");
            int daysToVisit = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Do you want to visit the northern areas? Y/N ");
            bool gotoNorth = Console.ReadLine().Equals("Y") ? true : false;
            Console.WriteLine("Do you have any of the conditions; heart or lung issues, diabetes? Y/N ");
            bool medicalIssues = Console.ReadLine().Equals("Y") ? true : false;
            Console.WriteLine("Do you want to visit the southern deserts? Y/N ");
            bool gotoSouth = Console.ReadLine().Equals("Y") ? true : false;
            Console.WriteLine("Do you have any of the conditions; high or low blood pressure, kidney problems? Y/N ");
            bool medicalIssues2 = Console.ReadLine().Equals("Y") ? true : false;
            var john = new Customer
            {
                //Name = "John Doe",
                Name = name,
                //IsNationality = true,
                IsNationalityIsraeli = isIsraeli,
                //TravelExpense = 200000,
                TravelExpense = travelExpense,
                //Citiestovisit = 4,
                Daystovisit = daysToVisit,
                WanttovisitNorthernareas = gotoNorth,
                //WanttovisitNorthernareas = true,
                //CriminalRecord = false
                CriminalRecord = crimincalRecord,
                WanttovisitSouthernareas = gotoSouth,
                MedicalIssues = medicalIssues,
                MedicalIssues2 = medicalIssues2
            };



            trunk.Evaluate(john);

            Console.WriteLine("Press any key...");
            Console.ReadKey();

        }

        public bool Result { get; set; }
        public override void Evaluate(Customer Customer)
        {
            Console.WriteLine("\r\nRecommend to Visit Pakistan: {0}", Result ? "YES" : "NO");
        }
        private static DecisionQuery MainDecisionTree()
        {
            //Decision 5
            var medicalissuesbranch2 = new DecisionQuery
            {
                Title = " Want to visit Southern deserts",
                Test = (Customer) => Customer.MedicalIssues2,
                Positive = new DecisionResult { Result = false },
                Negative = new DecisionResult { Result = true }
            };

            //Decision 5
            var visitareasbranch2 = new DecisionQuery
            {
                Title = " Want to visit Southern deserts",
                Test = (Customer) => Customer.WanttovisitSouthernareas,
                Positive = medicalissuesbranch2,
                Negative = new DecisionResult { Result = true }
            };

            //Decision 5
            var medicalissuesbranch = new DecisionQuery
            {
                Title = "Do you have any of the conditions; heart or lung issues, diabetes",
                Test = (Customer) => Customer.MedicalIssues,
                Positive = new DecisionResult { Result = false },
                Negative = visitareasbranch2
            };

            //Decision 4
            var visitareasbranch = new DecisionQuery
            {
                Title = " Want to visit Northern areas",
                Test = (Customer) => Customer.WanttovisitNorthernareas,
                Positive = medicalissuesbranch,
                Negative = visitareasbranch2
            };

            //Decision 3
            var daystovisit = new DecisionQuery
            {
                Title = " How many days you can holiday for",
                Test = (Customer) => Customer.Daystovisit > 9,
                Positive = visitareasbranch,
                Negative = new DecisionResult { Result = false }
            };
            //Decision 2
            var amountbranch = new DecisionQuery
            {
                Title = " Your Travel Expense",
                Test = (Customer) => Customer.TravelExpense > 150000,
                Positive = daystovisit,
                Negative = new DecisionResult { Result = false }
            };
            //Decision 1
            var criminalBranch = new DecisionQuery
            {
                Title = " Have a criminal record",
                Test = (Customer) => Customer.CriminalRecord,
                Positive = new DecisionResult { Result = false },
                Negative = amountbranch
            };
            //Decision 0
            var trunk = new DecisionQuery
            {
                Title = " Is nationality Israeli",
                Test = (Customer) => Customer.IsNationalityIsraeli,
                Positive = new DecisionResult { Result = false },
                Negative = criminalBranch
            };

            return trunk;

        }
    }


}
