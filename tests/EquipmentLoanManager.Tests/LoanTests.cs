using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Tests;

public class LoanTests
{
    /// <summary>Checks borrowing assigns an item and updates availability.</summary>
    [Fact]
    public void Borrow_AssignsAvailableItemAndUpdatesAvailability()
    {
        var manager = TestSession.Create(laptops: 2);
        var borrower = manager.Register("Amara");

        var result = manager.Borrow(borrower.Id, EquipmentCategory.Laptop);

        Assert.True(result.Succeeded);
        Assert.Same(borrower, result.Value!.Borrower);
        Assert.False(result.Value.Item.IsAvailable);
        Assert.Equal(1, manager.Inventory.AvailableCount(EquipmentCategory.Laptop));
    }

    /// <summary>Checks every loan gets a unique ID.</summary>
    [Fact]
    public void Borrow_GivesEachLoanAUniqueId()
    {
        var manager = TestSession.Create(laptops: 2);
        var first = manager.Borrow(manager.Register("Amara").Id, EquipmentCategory.Laptop).Value!;
        var second = manager.Borrow(manager.Register("Kwame").Id, EquipmentCategory.Laptop).Value!;

        Assert.NotEqual(first.Id, second.Id);
        Assert.NotEqual(first.Item.Id, second.Item.Id);
    }

    /// <summary>Checks a third active loan is refused.</summary>
    [Fact]
    public void Borrow_RejectsThirdActiveLoan()
    {
        var manager = TestSession.Create(laptops: 1, headsets: 1, tripods: 1);
        var borrower = manager.Register("Amara");
        manager.Borrow(borrower.Id, EquipmentCategory.Laptop);
        manager.Borrow(borrower.Id, EquipmentCategory.VrHeadset);

        var result = manager.Borrow(borrower.Id, EquipmentCategory.Tripod);

        Assert.False(result.Succeeded);
        Assert.Equal(2, manager.Loans.ActiveFor(borrower.Id).Count);
    }

    /// <summary>Checks a second item from the same category is refused.</summary>
    [Fact]
    public void Borrow_RejectsSecondItemFromSameCategory()
    {
        var manager = TestSession.Create(laptops: 2);
        var borrower = manager.Register("Amara");
        manager.Borrow(borrower.Id, EquipmentCategory.Laptop);

        var result = manager.Borrow(borrower.Id, EquipmentCategory.Laptop);

        Assert.False(result.Succeeded);
        Assert.Equal(1, manager.Inventory.AvailableCount(EquipmentCategory.Laptop));
    }

    /// <summary>Checks an unknown borrower cannot borrow.</summary>
    [Fact]
    public void Borrow_RejectsUnknownBorrower()
    {
        var manager = TestSession.Create(laptops: 1);

        var result = manager.Borrow("BR-999", EquipmentCategory.Laptop);

        Assert.False(result.Succeeded);
        Assert.Empty(manager.Loans.Active);
    }

    /// <summary>Checks borrowing fails when no item is available.</summary>
    [Fact]
    public void Borrow_RejectsWhenAllItemsAreOnLoan()
    {
        var manager = TestSession.Create(laptops: 1);
        manager.Borrow(manager.Register("Amara").Id, EquipmentCategory.Laptop);

        var result = manager.Borrow(manager.Register("Kwame").Id, EquipmentCategory.Laptop);

        Assert.False(result.Succeeded);
        Assert.Single(manager.Loans.Active);
    }

    /// <summary>Checks returning completes the loan and frees the item.</summary>
    [Fact]
    public void Return_CompletesLoanAndFreesItem()
    {
        var manager = TestSession.Create(laptops: 1);
        var loan = manager.Borrow(manager.Register("Amara").Id, EquipmentCategory.Laptop).Value!;

        var result = manager.Return(loan.Id);

        Assert.True(result.Succeeded);
        Assert.Equal(LoanStatus.Completed, loan.Status);
        Assert.True(loan.Item.IsAvailable);
        Assert.Contains(loan, manager.Loans.Completed);
        Assert.Empty(manager.Loans.Active);
    }

    /// <summary>Checks a loan cannot be returned twice.</summary>
    [Fact]
    public void Return_RejectsDuplicateReturn()
    {
        var manager = TestSession.Create(laptops: 1);
        var loan = manager.Borrow(manager.Register("Amara").Id, EquipmentCategory.Laptop).Value!;
        manager.Return(loan.Id);

        var result = manager.Return(loan.Id);

        Assert.False(result.Succeeded);
        Assert.Single(manager.Loans.Completed);
    }

    /// <summary>Checks an unknown loan cannot be returned.</summary>
    [Fact]
    public void Return_RejectsUnknownLoan()
    {
        var manager = TestSession.Create(laptops: 1);

        var result = manager.Return("LN-404");

        Assert.False(result.Succeeded);
    }

    /// <summary>Checks a borrower leaves the active list after returning everything.</summary>
    [Fact]
    public void ActiveBorrowers_DropsBorrowerOnceAllLoansAreReturned()
    {
        var manager = TestSession.Create(laptops: 1);
        var borrower = manager.Register("Amara");
        var loan = manager.Borrow(borrower.Id, EquipmentCategory.Laptop).Value!;

        manager.Return(loan.Id);

        Assert.DoesNotContain(borrower, manager.ActiveBorrowers());
        Assert.Same(borrower, manager.Loans.Completed.Single().Borrower);
    }
}
