Resto

Resto е ASP.NET Core приложение за управление на ресторант.  
Позволява на администратори да управляват меню, категории, маси и потребители, а потребителите да разглеждат менюто и да резервират маса.

--Основни функции:
- CRUD за меню и категории (търсене, филтриране, сортиране)
- Управление на маси и резервации
- Поръчки с количество и инструкции
- Качване на снимки до 5 MB
- Роли: администратор и потребител

--Технологии:.NET 9, ASP.NET Core MVC, Entity Framework Core, PostgreSQL, BCrypt, Bootstrap.

--Стартиране:
1. `git clone https://github.com/SUdokU23/ASP.NET-CRUD-Restaurant-aplication.git`
2. Създай `.env` файл с `SQL_CONN=...`
3. `dotnet restore`
4. `dotnet ef database update`
5. `dotnet run`

--Лиценз:MIT  
--Автор: SUdokU23
