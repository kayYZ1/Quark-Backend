# Quark-Backend
Aby uruchomić aplikację należy uruchomić dwa niezależne serwery: 
- aplikację Quark-Backend oraz 
- aplikację Quark-Frontend https://github.com/Project-team-PO/Quark-Frontend
### Konfiguracja
Aby mieć możliwość uruchomienia serwera Quark-Backend lokalnie należy najpierw wykonać kilka kroków:
- należy sklonować powyższe repozytorium: git clone https://github.com/Project-team-PO/Quark-Backend.git
- trzeba ściągnąć system bazy danych Postgresql (najlepiej z klientem bazy danych PgAdmin, który trzeba ściągnąć osobno, żeby mieć interfejs użytkownika)
- w PostgreSQL zrobić bazę danych o nazwie: quark_db, oraz hasłem: 1234
- należy zainstalować narzędzie frameworka Entity Framework używając polecenia w terminalu: dotnet tool install --global dotnet-ef
- [uruchomić terminal w folderze projektu]
- należy zbudować projekt przy użyciu polecenia: dotnet build
- należy użyć polecenia: dotnet ef database update - które utworzy odpowiednie tabele dla utworzonej wcześniej bazy danych quark_db
- poprawne wywołanie polecenia wyżej powinno dodać do bazy danych m.in. kilku przykładowych, fikcyjnych [użytkowników](./DAL/ModelBuilderExtensions.cs) na których dane będzie można się zalogować do aplikacji.

### Uruchamianie Quark-Backend
- aby uruchomić serwer należy użyć polecenia: dotnet run
- po uruchomieniu aplikacji, w konsoli powinien pojawić się lokalny adres pod którym został wystawiony serwer. Nie trzeba uruchamiać tego adresu w przeglądarce - serwer ten jest potrzebny jedynie do tego aby aplikacja działała po stronie klienta.

### Przeznaczenie folderów
- Entities: przechowuje modele które odpowiadają tabelom w bazie danych. Potrzebne dla stworzenia kontekstu EntityFrameworka (DbContext)
- DAL (Data Access Layer): przechowuje kontekst EntityFramework'a (dla połączenia z bazą danych). Odwzorowuje strukturę bazy danych.