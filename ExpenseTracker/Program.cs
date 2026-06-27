// =====================================================================
//  Program.cs  —  the interactive console UI for MyBudget (Assignment 1).
//  Target framework: .NET 10 (LTS), language C# 14.
//
//  >>> BUILD THE MENU-DRIVEN UI HERE (Modules 1-3). <<<
//
//  Once you have implemented BudgetRules.cs (so the unit tests pass), wire it
//  up to a console interface that meets the assignment brief:
//
//    * Print a banner (try a raw string literal).
//    * Loop a menu until the user exits, using a switch on the choice:
//        1) Add an expense   2) View summary   3) Set monthly budget   4) Exit
//    * Read and VALIDATE input, re-prompting on bad data (decimal.TryParse,
//      BudgetRules.NormalizeCategory, a date parse, non-empty text).
//    * Keep running totals in simple variables (no collections / no classes).
//    * Use BudgetRules.ValidateAmount / ClassifyAmount / BudgetStatus /
//      FormatCurrency for all logic and formatting.
//    * Handle bad input with try / catch / finally and InvalidExpenseException.
//
//  See section 6 of the assignment brief for a sample run to aim for.
// =====================================================================
using ExpenseTracker;
using System.Globalization;
decimal overallTotal = 0m;
decimal totalExpense = 0m;
decimal highestExpense = 0m;
String? highestCategory = null;
string? status = null;


Console.WriteLine("===============================================================");
Console.WriteLine("                  MyBudget  Expense Tracker                     ");
Console.WriteLine("===============================================================");

string? choice = null;
do
{
    Console.WriteLine("");
    Console.WriteLine("1) Add Expense   2) View Summary 3) Set Mothly Budget    4) Exit");
    choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            AddExpense();
            break;
        case "2":
            ViewSummary();
            break;
        case "3":
            MonthlyBudget();
            break;
        case "4":
            ExitMessage();
            break;
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}
while (choice != null && !choice.Equals("4"));

// add expense
void AddExpense() {
    //  Enter the Category
    Console.WriteLine("");
    Console.Write ("Enter Category : [Food/Transport/Utilities/Entertaiment/Other]:  ");
    string? catchoice = Console.ReadLine();
    string ? category = BudgetRules.NormalizeCategory(catchoice);
    while (category == null)
    {
        Console.WriteLine("Enter Category : [Food/Transport/Utilities/Entertaiment/Other] ");
        string? input = Console.ReadLine();
        category = BudgetRules.NormalizeCategory(input);
    }

    //  Enter the Amount
    Console.WriteLine(" ");
    Console.WriteLine("Enter the Amount");
    string? amountChoice = Console.ReadLine();
    while (amountChoice==null && !decimal.TryParse(amountChoice, out decimal inputAmount))
    {
        Console.WriteLine("Enter a valid  Amount");
        amountChoice = Console.ReadLine();
    }

    while (decimal.Parse(amountChoice)<0)
    {
        Console.WriteLine("Enter a Valid Amount: Must be  greater the zero");
        amountChoice = Console.ReadLine();
    }

    decimal amount = decimal.Parse(amountChoice);
    overallTotal += amount;
    totalExpense += 1;
    String amountClass = BudgetRules.ClassifyAmount(amount);

    if(amount> highestExpense) { 
        highestExpense = amount;
        highestCategory = category;
    }

    // Enter Date
    Console.WriteLine("");
    Console.WriteLine("Enter the Date in format yyyy-MM-dd");
    string? inputDate = Console.ReadLine();
    DateTime dateType;

    if (!string.IsNullOrWhiteSpace(inputDate) &&
        DateTime.TryParseExact(
            inputDate,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out dateType))
    {
      
    }
    else
    {
        dateType = DateTime.Today;
    }

    String date = dateType.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    //Enter a Note
    Console.WriteLine(" ");
    Console.Write("Enter a Note(Optional):  ");
    string? note = Console.ReadLine();
    Console.WriteLine("                                ");
    Console.WriteLine($" Description :          {note}");
    Console.WriteLine($" Amount :               {amount}");
    Console.WriteLine($" Recorded:              ${amount} | {category} |{date}");
    Console.WriteLine($" Size Band :             {amountClass}");

}

void ViewSummary()
{
   Console.WriteLine("                                            ");
   Console.WriteLine("============== SUMMARY ====================");    
   Console.WriteLine($"The number of expenses is : {totalExpense}:C2");
   Console.WriteLine($"Total spend :               {overallTotal}:C2");
   Console.WriteLine($"Average :                   {overallTotal/ totalExpense}:C2");
   Console.WriteLine($"Highest Single :            {highestCategory}");
   Console.WriteLine($"Highest Expense:            {highestExpense} :C2");


}

void MonthlyBudget() {
    Console.WriteLine("");
   Console.WriteLine(" Set your Monthly Budget");
    string? inputMonBudget = Console.ReadLine();

    while (!decimal.TryParse(inputMonBudget, out decimal inpoutAmount)) {
        Console.WriteLine(" Please Enter a valid budget mount");
        inputMonBudget = Console.ReadLine();
    }

    decimal monthlyLimit = decimal.Parse(inputMonBudget);
    status = BudgetRules.BudgetStatus( monthlyLimit- overallTotal , monthlyLimit);
    Console.WriteLine($"Monthly Budget:  {monthlyLimit}:C2 ");
   // Console.WriteLine($"Budget set to:   {monthlyLimit- overallTotal}:C2 ");
    Console.WriteLine($"        Budget:  {monthlyLimit}:C2 remaining of {monthlyLimit- overallTotal} -> {status}");
}

void ExitMessage()
{
    Console.WriteLine("============== END ====================");
    Console.WriteLine("        Thank your visite              ");
    Console.WriteLine("=======================================");
}
