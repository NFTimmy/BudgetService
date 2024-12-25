namespace BudgetService;

public class BudgetService
{
   private readonly IBudgetRepo _budgetRepo;

   public BudgetService(IBudgetRepo budgetRepo)
   {
      _budgetRepo = budgetRepo;
   }

   public decimal Query(DateTime start, DateTime end)
   {
      decimal result = 0;

      var datesInRange = GetDatesInRange(start,end);
      
      foreach (var date in datesInRange)
      {
         result += GetBudgetByDate(date);
      }

      return result;
   }

   private decimal GetBudgetByDate(string date)
   {
      var budgets = _budgetRepo.GetAll();
      var budgetOfYearMonth = budgets.Single(x => string.Equals(x.YearMonth, date.Substring(0,6))).Amount;
      var daysOfYearMonth = DateTime.DaysInMonth(
         int.Parse(date.Substring(0, 4)),
         int.Parse(date.Substring(4, 2)));

      return budgetOfYearMonth / daysOfYearMonth;

   }

   static List<string> GetDatesInRange(DateTime startDate, DateTime endDate)
   {
      List<string> dates = new List<string>();

      for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
      {
         dates.Add(date.ToString("yyyyMMdd")); // 将日期格式化为"20240130"这种格式
      }

      return dates;
   }
}