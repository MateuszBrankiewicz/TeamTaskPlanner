# Team Task Planner API Documentation

## 🚀 Complete Task Management System

Aplikacja działa na: **http://localhost:5078**

## 📋 Funkcjonalności

### ✅ **Autoryzacja i Uwierzytelnianie**
- Rejestracja i logowanie użytkowników
- JWT tokeny z refresh tokenami
- Role-based authorization (User, Manager, Admin)
- Automatyczne przypisywanie ról na podstawie emaila

### ✅ **Zarządzanie Projektami**
- Tworzenie, edycja i usuwanie projektów
- Dodawanie członków do projektów z różnymi rolami
- Śledzenie postępu projektów

### ✅ **Zarządzanie Zadaniami**
- Tworzenie zadań z różnymi priorytetami i statusami
- Przypisywanie zadań do użytkowników i projektów
- Śledzenie terminów i statusów
- Pełny system komentarzy z możliwością odpowiadania

### ✅ **System Komentarzy**
- Komentowanie zadań
- Odpowiedzi na komentarze (nested comments)
- Edycja i usuwanie komentarzy
- Kontrola dostępu do komentarzy

## 🔐 Endpointy API

### **Autoryzacja**
```
POST /api/auth/register       - Rejestracja
POST /api/auth/login          - Logowanie  
POST /api/auth/refresh-token  - Odświeżenie tokena
POST /api/auth/logout         - Wylogowanie
```

### **Projekty**
```
GET    /api/project           - Lista projektów użytkownika
POST   /api/project           - Tworzenie projektu
GET    /api/project/{id}      - Szczegóły projektu
PUT    /api/project/{id}      - Edycja projektu
DELETE /api/project/{id}      - Usunięcie projektu

POST   /api/project/{id}/members      - Dodanie członka
GET    /api/project/{id}/members      - Lista członków
DELETE /api/project/{id}/members/{memberId} - Usunięcie członka
```

### **Zadania**
```
GET    /api/task              - Lista zadań użytkownika
POST   /api/task              - Tworzenie zadania
GET    /api/task/{id}         - Szczegóły zadania
PUT    /api/task/{id}         - Edycja zadania
DELETE /api/task/{id}         - Usunięcie zadania

GET    /api/task?status=Todo  - Filtrowanie po statusie
GET    /api/task?projectId=1  - Filtrowanie po projekcie
```

### **Komentarze**
```
GET    /api/task/{id}/comments     - Komentarze zadania
POST   /api/task/{id}/comments     - Dodanie komentarza
PUT    /api/task/comments/{id}     - Edycja komentarza
DELETE /api/task/comments/{id}     - Usunięcie komentarza
```

### **Dashboard i Admin**
```
GET    /api/dashboard              - Dashboard użytkownika
GET    /api/dashboard/tasks        - Zadania użytkownika
GET    /api/dashboard/stats        - Statystyki użytkownika
GET    /api/dashboard/profile      - Profil użytkownika

GET    /api/admin/users           - Lista użytkowników (Admin)
GET    /api/admin/system-stats    - Statystyki systemu (Admin/Manager)
```

## 📊 Statusy i Priorytety

### **Statusy Zadań**
- `0` - Todo
- `1` - InProgress  
- `2` - InReview
- `3` - Done
- `4` - Blocked
- `5` - Cancelled

### **Priorytety**
- `0` - Low
- `1` - Medium
- `2` - High
- `3` - Critical

### **Role Projektów**
- `0` - Member
- `1` - Lead
- `2` - Manager

## 🔒 System Uprawnień

### **Zadania**
- **Twórca**: Pełna kontrola nad zadaniem
- **Przypisana osoba**: Może edytować status i dodawać komentarze
- **Członkowie projektu**: Mogą przeglądać i komentować
- **Liderzy projektu**: Mogą edytować zadania w swoich projektach

### **Projekty**
- **Twórca**: Pełna kontrola nad projektem
- **Manager**: Może edytować projekt i zarządzać członkami
- **Lead**: Może edytować projekt
- **Member**: Może przeglądać i tworzyć zadania

### **Komentarze**
- **Autor**: Może edytować i usuwać swoje komentarze
- **Twórca zadania**: Może usuwać komentarze w swoich zadaniach
- **Liderzy projektu**: Mogą moderować komentarze

## 🎯 Przykłady Użycia

### 1. **Podstawowy Workflow**
```
1. Zarejestruj się: POST /api/auth/register
2. Zaloguj się: POST /api/auth/login  
3. Utwórz projekt: POST /api/project
4. Dodaj członka: POST /api/project/1/members
5. Utwórz zadanie: POST /api/task
6. Skomentuj zadanie: POST /api/task/1/comments
7. Zaktualizuj status: PUT /api/task/1
```

### 2. **Współpraca w Zespole**
```
- Manager tworzy projekt i dodaje członków
- Członkowie tworzą zadania w projekcie
- Zespół komentuje i śledzi postęp
- Lead zarządza priorytetami i statusami
```

### 3. **Zarządzanie Zadaniami**
```
- Filtrowanie zadań po statusie i projekcie
- Śledzenie terminów i priorytetów
- System komentarzy z odpowiedziami
- Automatyczne znaczniki czasowe
```

## 🧪 Testowanie

Użyj plików `.http` do testowania:
- `TaskManagementTests.http` - Testy zarządzania zadaniami
- `ProjectManagementTests.http` - Testy zarządzania projektami
- `DashboardTests.http` - Testy dashboard'u
- `RoleTests.http` - Testy autoryzacji ról

## 🗄️ Baza Danych

System automatycznie tworzy wszystkie potrzebne tabele:
- `Users`, `Roles` - Użytkownicy i role
- `Projects`, `ProjectMembers` - Projekty i członkostwo
- `Tasks` - Zadania z pełną hierarchią
- `TaskComments` - Komentarze z możliwością odpowiedzi
- `TaskAttachments` - Załączniki do zadań (przygotowane na przyszłość)

## 🔧 Technologie

- **Backend**: ASP.NET Core 9.0
- **Database**: PostgreSQL z Entity Framework
- **Authentication**: JWT + Refresh Tokens
- **Authorization**: Role-based + Custom policies
- **API**: RESTful z pełną dokumentacją

## 🎉 System jest gotowy do użycia!

Wszystkie funkcjonalności zostały zaimplementowane i przetestowane. Aplikacja obsługuje:
- ✅ Pełne zarządzanie zadaniami
- ✅ System komentarzy z odpowiedziami  
- ✅ Zarządzanie projektami i zespołami
- ✅ Bezpieczną autoryzację i uwierzytelnianie
- ✅ Kontrolę dostępu na poziomie zadań i projektów
