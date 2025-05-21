# MySQL DB Context
Verbinding op drie plaatsen gedefinieerd. Niet nodig
- DbContext naar connection vragen bij Dapper Query (zie code)
- Startup Project meegegeven aan ef tools

```
stefancourteaux@MBP EventPlanner.Api.Persistence % pwd                      
/Users/stefancourteaux/Source/PG2Prep/ExamenOefening/EventPlanner.Api.Persistence
stefancourteaux@MBP EventPlanner.Api.Persistence % dotnet ef database update --startup-project ../EventPlanner.Api
```

# Async suffix
Het is best practice om alle async methodes met `Async` te suffixen. Mogelijks heb je dit gedaan, maar dan geconstateerd dat `CreatedAtAction` de route niet vond. 

Zie `CreateAsync` in `EvenementController`.

