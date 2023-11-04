### Co działa:
- połączenie z bazą danych
- dodawanie rekordów do bazy danych przy obsłudze PUT requesta. Kontroler (UsersController) był zrobiony prowizorycznie żeby sprawdzić czy w ogóle działa połączenie z bazą danych.

### Problemy:
- póki co backend nie uwzględnia w ogóle Quark-Frotnendu.

### Przeznaczenie folderów
- Entities: przechowuje modele które odpowiadają tabelom w bazie danych. Potrzebne dla stworzenia kontekstu EntityFrameworka (DbContext)
- DAL (Data Access Layer): przechowuje kontekst EntityFramework'a (dla połączenia z bazą danych). Odwzorowuje strukturę bazy danych.