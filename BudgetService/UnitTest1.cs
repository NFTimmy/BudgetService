using NSubstitute;

namespace BudgetService;

public class Tests
{
    private IBudgetRepo _budgetRepo;

    [SetUp]
    public void Setup()
    {
         _budgetRepo = Substitute.For<IBudgetRepo>();
    }

    [Test]
    public void QueryWholeMonth()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 31);
        
        var mockBudgets = new List<Budget>
        {
            new() { YearMonth = "202401", Amount = 3100 },
        };

        _budgetRepo.GetAll().Returns(mockBudgets);
        
        var budgetService = new BudgetService(_budgetRepo);
        var budget = budgetService.Query(start, end);
        Assert.That(3100, Is.EqualTo(budget));
    }

    [Test]
    public void QueryMonthPartialDays()
    {
        var start = new DateTime(2024, 1, 8);
        var end = new DateTime(2024, 1, 10);

        var mockBudgets = new List<Budget>
        {
            new() { YearMonth = "202401", Amount = 310 },
        };

        _budgetRepo.GetAll().Returns(mockBudgets);
        
        var budgetService = new BudgetService(_budgetRepo);
        var budget = budgetService.Query(start, end);
        Assert.That(30, Is.EqualTo(budget));
    }

    [Test]
    public void QueryMultipleWholeMonths()
    {
        var start = new DateTime(2024, 10, 1);
        var end = new DateTime(2024, 11, 30);

        var mockBudgets = new List<Budget>
        {
            new() { YearMonth = "202410", Amount = 3100 },
            new() { YearMonth = "202411", Amount = 3000 },
        };

        _budgetRepo.GetAll().Returns(mockBudgets);
        
        var budgetService = new BudgetService(_budgetRepo);
        var budget = budgetService.Query(start, end);
        Assert.That(6100, Is.EqualTo(budget));
    }

    [Test]
    public void QueryMultipleMonthsPartialDays()
    {
        var start = new DateTime(2024, 7, 31);
        var end = new DateTime(2024, 8, 1);

        var mockBudgets = new List<Budget>
        {
            new() { YearMonth = "202407", Amount = 3100 },
            new() { YearMonth = "202408", Amount = 6200 },
        };

        _budgetRepo.GetAll().Returns(mockBudgets);
        
        var budgetService = new BudgetService(_budgetRepo);
        var budget = budgetService.Query(start, end);
        Assert.That(300, Is.EqualTo(budget));
    }

    [Test]
    public void QueryInvalidDuration()
    {
        var start = new DateTime(2024, 12, 31);
        var end = new DateTime(2024, 12, 1);

        var mockBudgets = new List<Budget>
        {
            new() { YearMonth = "202412", Amount = 3100 },
        };

        _budgetRepo.GetAll().Returns(mockBudgets);
        
        var budgetService = new BudgetService(_budgetRepo);
        var budget = budgetService.Query(start, end);
        Assert.That(0, Is.EqualTo(budget));
    }
}