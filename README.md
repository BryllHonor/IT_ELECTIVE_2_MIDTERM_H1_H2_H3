# The Last Panel — Comic Shop POS

A Point of Sale (POS) web application built with **ASP.NET Core MVC** for a neighborhood
comic book shop. This is an MVP: no database, no Entity Framework — all data
(products, the active cart, and completed transactions) lives in memory and resets
when the application restarts.

## Store Theme

**The Last Panel** — comics, graphic novels, collectibles, trading cards, and supplies.
Tagline: *"Every Story Has an Ending. Yours Starts Here."*

The catalog is seeded with 10 products across four categories (Graphic Novels,
Collectibles, Trading Cards, Supplies), including one item seeded at zero stock
to demonstrate the "Out of Stock" UI state.

## Architecture

- **Entities** (`Models/Entities`) — `Product`, `CartItem`, `ShoppingCart`, `Transaction`.
  Pure domain objects; never bound directly to a form.
- **DTOs** (`Models/DTOs`) — `AddToCartDTO`, `UpdateCartDTO`, `CheckoutFormDTO`.
  The only objects Views/Controllers bind to; carry Data Annotations for
  server-side validation.
- **Repositories** (`Repositories`) — in-memory, DI-Singleton-scoped stores
  (`ProductRepository`, `CartRepository`, `TransactionRepository`) that hold
  the "static" data for the life of the running app.
- **Services** (`Services`) — business logic layer (stock validation, cart math,
  checkout orchestration) sitting between Controllers and Repositories.
- **Controllers** — thin; bind DTOs, call services, translate results into
  redirects or `ModelState` errors.

## Running the Application

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download)

```bash
cd ComicShopPOS
dotnet restore
dotnet run
```

The console output will show the local URL (typically `https://localhost:5001` or
`http://localhost:5000`). Open it in a browser — you'll land on the Home dashboard.

## Using the App

1. **Browse Comics** — view the catalog, enter a quantity, and add items to the cart.
   Out-of-stock items are greyed out and can't be added.
2. **Cart** — review line items, update quantities, remove items, or proceed to checkout.
   Quantity changes are re-validated against current stock.
3. **Checkout** — enter the customer's name (required) and email (optional), then confirm
   payment. This creates a `Transaction`, deducts stock, and clears the cart.
4. **Sales History** — browse all completed transactions and drill into details for any sale.

## Git Workflow

This repository uses three branches:

- `main` — production-ready code.
- `feature/shopping-cart` — US-01 through US-04 (product browsing, cart, quantity
  management, item removal).
- `feature/checkout` — US-05 and US-06 (checkout/payment, transaction history).

## Constraints Honored

- ASP.NET Core MVC only — no Entity Framework, no SQL database.
- All business logic (stock checks, totals, checkout) runs server-side in
  Services/Controllers — no client-side JS for calculations.
- Entities are never used as input models; all form submissions bind to DTOs.
- Server-side validation via Data Annotations, surfaced with
  `asp-validation-for` / `asp-validation-summary` tag helpers.
