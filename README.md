# Guizziz Web App - dokumentacja

## Wprowadzenie
Projekt Guizziz to aplikacja internetowa stworzona w technologii ASP.NET Core MVC, z użyciem SQL Server której celem jest zarządzanie quizami. Admini mogą tworzyć, edytować oraz usuwać quizy, jak i również całą ich zawartość, dotyczącą poszczególnych pytań i odpowiedzi, mają wgląd w wyniki wszystkich użytkowników, a także w panel zarządzania użytkownikami. Następnie stworzone quizy mogą być rozwiązywane przez poszczególnych użytkowników, którzy mają możliwość sprawdzenia historii swoich podejść do poszczególnych quizów. Dostępna jest rejestracja i logowanie użytkowników, a także pełne zarządzanie stworzonym kontem. Jest również dostęp do REST API.
## Instalacja
1. Sklonuj repozytorium na swój lokalny komputer:
   ```sh
   git clone [https://github.com/FrosteNx/GuizzizWebApp.git]
2. Otwórz repozytorium w wybranym IDE (Visual Studio, Visual Studio Code).
3. Następnie do terminala wpisz następujące polecenia, aby przejść do docelowego folderu:
   ```sh
   cd GuizzWebApp
4. Dodaj nową migrację bazy danych:
   ```sh
   dotnet ef migrations add [nazwa] --context QuizDbContext
5. Zaktualizuj bazę danych:
   ```sh
   dotnet ef database update --context QuizDbContext
6. Uruchom aplikację:
   ```sh
   dotnet run
     
## Funkcjonalności
### Użytkownicy
- Rejestracja nowych użytkowników
- Logowanie użytkowników
- Wylogowywanie użytkowników
### Quizy
- Tworzenie nowych quizów
- Edytowanie istniejących quizów
- Usuwanie quizów
- Pełne zarządzanie sczegółami poszczególnych quizów tj. zarządzanie pytaniami i odpowiedziami do poszczególnych pytań
### Results
- Pełny wgląd w historię przesłanych rozwiązań przez wszystkich użytkowników dla admina
- Wgląd w historię podjętych prób przez danego użytkownika
### Admin Panel
- Zarządzanie użytkownikami
### REST API
- Dotyczące Quizów, Questions, Answers, QuizResults i UserAnswers.

## Przykład użycia 
Podczas pierwszego uruchomienia aplikacji tworzy się domyślny użytkownik administratora. Dane logowania:
- E-mail: *admin@example.com*
- Password: *Admin@1234*
Admin ma możliwość dodawania użytkowników, rejestracji nowych kont. Pełnego zarządzania tworzeniem nowych quizów w zakładce "Quizzes", wglądu do historii udziału wszystkich użytkowników w poszczególnych quizach w zakładce "Results", a także zarządzania użytkownikami w zakładce "Admin Panel.
Po zalogowaniu przejdź do zakładki "Quizzes". Gdzie możesz wziąć udział w dowolnym z dostępnych quizów. Wypełnij quiz, a zobaczysz swój wynik, przejdź do zakładki "Results", żeby zobaczyć historię swoich wyników.
