using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.Tests;

public class WaitingListTests
{
    private readonly LoanManager _manager = TestSession.Create(laptops: 1);
    private readonly Borrower _holder;
    private readonly Loan _holderLoan;

    public WaitingListTests()
    {
        _holder = _manager.Register("Amara");
        _holderLoan = _manager.Borrow(_holder.Id, EquipmentCategory.Laptop).Value!;
    }

    [Fact]
    public void Join_AddsBorrowerWhenCategoryIsFullyBorrowed()
    {
        var waiter = _manager.Register("Kwame");

        var result = _manager.JoinWaitingList(waiter.Id, EquipmentCategory.Laptop);

        Assert.True(result.Succeeded);
        Assert.Equal([waiter], _manager.WaitingLists.GetQueue(EquipmentCategory.Laptop));
    }

    [Fact]
    public void Join_RejectsWhenItemsAreStillAvailable()
    {
        var manager = TestSession.Create(laptops: 2);
        var borrower = manager.Register("Kwame");

        var result = manager.JoinWaitingList(borrower.Id, EquipmentCategory.Laptop);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void Join_RejectsDuplicateEntry()
    {
        var waiter = _manager.Register("Kwame");
        _manager.JoinWaitingList(waiter.Id, EquipmentCategory.Laptop);

        var result = _manager.JoinWaitingList(waiter.Id, EquipmentCategory.Laptop);

        Assert.False(result.Succeeded);
        Assert.Single(_manager.WaitingLists.GetQueue(EquipmentCategory.Laptop));
    }

    [Fact]
    public void Join_RejectsCategoryWithZeroStock()
    {
        var waiter = _manager.Register("Kwame");

        var result = _manager.JoinWaitingList(waiter.Id, EquipmentCategory.Camera360);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void Join_RejectsIneligibleBorrower()
    {
        var result = _manager.JoinWaitingList(_holder.Id, EquipmentCategory.Laptop);

        Assert.False(result.Succeeded);
        Assert.Empty(_manager.WaitingLists.GetQueue(EquipmentCategory.Laptop));
    }

    [Fact]
    public void Cancel_RemovesRequestAndKeepsOthersInOrder()
    {
        var first = _manager.Register("Kwame");
        var second = _manager.Register("Lina");
        var third = _manager.Register("Tendai");
        _manager.JoinWaitingList(first.Id, EquipmentCategory.Laptop);
        _manager.JoinWaitingList(second.Id, EquipmentCategory.Laptop);
        _manager.JoinWaitingList(third.Id, EquipmentCategory.Laptop);

        var result = _manager.CancelWaitingRequest(second.Id, EquipmentCategory.Laptop);

        Assert.True(result.Succeeded);
        Assert.Equal([first, third], _manager.WaitingLists.GetQueue(EquipmentCategory.Laptop));
    }

    [Fact]
    public void Cancel_RejectsBorrowerWhoIsNotWaiting()
    {
        var bystander = _manager.Register("Kwame");

        var result = _manager.CancelWaitingRequest(bystander.Id, EquipmentCategory.Laptop);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void Return_AnnouncesNextWaitingBorrower()
    {
        var waiter = _manager.Register("Kwame");
        _manager.JoinWaitingList(waiter.Id, EquipmentCategory.Laptop);

        var result = _manager.Return(_holderLoan.Id);

        Assert.Contains(waiter.Name, result.Message);
        Assert.Same(waiter, _manager.WaitingLists.Peek(EquipmentCategory.Laptop));
    }

    [Fact]
    public void Borrow_CannotBypassExistingQueue()
    {
        var waiter = _manager.Register("Kwame");
        var latecomer = _manager.Register("Lina");
        _manager.JoinWaitingList(waiter.Id, EquipmentCategory.Laptop);
        _manager.Return(_holderLoan.Id);

        var result = _manager.Borrow(latecomer.Id, EquipmentCategory.Laptop);

        Assert.False(result.Succeeded);
        Assert.Equal(1, _manager.Inventory.AvailableCount(EquipmentCategory.Laptop));
    }

    [Fact]
    public void IssueToNext_ServesQueueInFirstInFirstOutOrder()
    {
        var first = _manager.Register("Kwame");
        var second = _manager.Register("Lina");
        _manager.JoinWaitingList(first.Id, EquipmentCategory.Laptop);
        _manager.JoinWaitingList(second.Id, EquipmentCategory.Laptop);
        _manager.Return(_holderLoan.Id);

        var result = _manager.IssueToNextWaiting(EquipmentCategory.Laptop);

        Assert.True(result.Succeeded);
        Assert.Same(first, result.Value!.Borrower);
        Assert.Equal([second], _manager.WaitingLists.GetQueue(EquipmentCategory.Laptop));
    }

    [Fact]
    public void IssueToNext_RejectsWhenNoItemHasBeenReturned()
    {
        var waiter = _manager.Register("Kwame");
        _manager.JoinWaitingList(waiter.Id, EquipmentCategory.Laptop);

        var result = _manager.IssueToNextWaiting(EquipmentCategory.Laptop);

        Assert.False(result.Succeeded);
        Assert.Single(_manager.WaitingLists.GetQueue(EquipmentCategory.Laptop));
    }

    [Fact]
    public void IssueToNext_RechecksEligibilityBeforeIssuing()
    {
        var manager = TestSession.Create(laptops: 1, headsets: 1, tripods: 1);
        var holder = manager.Register("Amara");
        var waiter = manager.Register("Kwame");
        var laptopLoan = manager.Borrow(holder.Id, EquipmentCategory.Laptop).Value!;
        manager.JoinWaitingList(waiter.Id, EquipmentCategory.Laptop);
        manager.Borrow(waiter.Id, EquipmentCategory.VrHeadset);
        manager.Borrow(waiter.Id, EquipmentCategory.Tripod);
        manager.Return(laptopLoan.Id);

        var result = manager.IssueToNextWaiting(EquipmentCategory.Laptop);

        Assert.False(result.Succeeded);
        Assert.True(manager.WaitingLists.Contains(EquipmentCategory.Laptop, waiter.Id));
    }
}
