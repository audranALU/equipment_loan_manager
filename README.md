# Equipment Loan Manager

A WPF desktop app that simulates borrowing and returning campus equipment. A tester configures the stock, adds sample borrowers, lends and returns items, and manages waiting lists. Everything is kept in memory for the current session.

## Requirements

- Windows 10 or 11
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Running the app

```bash
git clone <repository-url>
cd equipment_loan_manager
dotnet run --project src/EquipmentLoanManager.App
```

Or open `EquipmentLoanManager.slnx` in Visual Studio 2022 (17.14 or later) or Rider, set **EquipmentLoanManager.App** as the startup project and press F5.

## Running the tests

```bash
dotnet test
```

## How it works

### 1. Configure equipment

On startup, enter how many items of each category are in stock:

| Category    | Maximum |
|-------------|---------|
| 360 Camera  | 5       |
| Laptop      | 8       |
| VR Headset  | 10      |
| Microphone  | 6       |
| Tripod      | 12      |

Only whole numbers between 0 and the maximum are accepted. Invalid entries are highlighted so you can correct them. Each item gets a unique ID such as `LAP-01` or `CAM-03`.

### 2. Main window

The header shows live totals and a banner with the result of the last action: green for confirmations, red for validation errors.

| Tab | What you can do |
|-----|-----------------|
| **Inventory** | View configured, available and borrowed quantities per category, plus every item and who holds it. |
| **Borrowers** | Add a borrower with a name and an optional ID (one is generated if left blank). Remove a borrower who has no active loans; their waiting-list requests are cancelled too. |
| **Loans** | Pick a borrower and a category to borrow an item. Select an active loan to return it. Completed loans stay in the history. |
| **Waiting lists** | Join a category's queue when every item is on loan, cancel a request, and issue a returned item to the borrower at the front of the queue. |

### Borrowing rules

- A borrower can hold at most two items at a time, and only one per category.
- Only borrowers currently added to the app can borrow or join a queue.
- An item can never be on loan to two borrowers at once.
- Returning a loan twice, or returning an unknown loan, is rejected.
- Waiting lists are first in, first out. If anyone is waiting for a category, new borrow requests for it are refused, so nobody can skip the queue.
- Eligibility is checked when joining a queue and again before an item is issued.
- Categories with zero stock cannot be borrowed or queued for.

## Project structure

```
src/
  EquipmentLoanManager.Core/      Business logic, no UI dependencies
    Configuration/                Category limits and quantity validation
    Models/                       Equipment items, borrowers, loans
    Results/                      Operation results with tester-facing messages
    Services/                     Inventory, registries, borrowing policy, LoanManager
  EquipmentLoanManager.App/       WPF front end (MVVM)
    Infrastructure/               ObservableObject and RelayCommand
    ViewModels/                   One view model per screen and tab
    Views/                        XAML windows and tab views
    Themes/                       Colours and control styles
tests/
  EquipmentLoanManager.Tests/     xUnit tests for the core library
```

The GUI talks only to `LoanManager` and the interfaces it exposes. All rules live in the Core project, so they are tested without the UI.
