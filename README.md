# Информационная система "Компьютерная компания"  
  
## Технологический стек
* **Язык:** C#
* **Фреймворк:** .NET 9, ASP.NET Core Web API, WPF
* **База данных:** PostgreSQL, Entity Framework Core
* **Аутентификация:** JWT
  
## Функции  
* Регистрация и авторизация пользователей
* Пользователь может купить комплектующие для ПК или заказать сборку
* Администратор может просматривать и редактировать данные пользователей и комплектующих

## Как запустить:
1. Скачайте репозиторий
2. Убедитесь, что у вас установлен ***<u>Docker</u>*** (если нет, то скачать его можно тут -> https://www.docker.com/products/docker-desktop/)
3. Соберите проект через ***<u>PowerShell</u>*** команду ```docker-compose up -d``` в папке скачанного репозитория
4. Скачайте и разархивируйте клиентские приложения в https://github.com/ABACH-HABAD/ComputerCompany/releases/tag/Release (или соберите их самостоятельно с помощью ***Visual Studio*** или выполнив команду ```dotnet run``` в папке ```src\ComputerCompany.Presentation```)
5. Приложение готово к использованию
  
## Скриншоты:  
  
Окно авторизации:  
<img width="606" height="458" alt="image" src="https://github.com/user-attachments/assets/b05cd3bb-8b7a-4b61-b0ff-06ef676728fa" />


Страница администратора с таблицей процессоров:  
<img width="1396" height="901" alt="image" src="https://github.com/user-attachments/assets/e31358e2-b279-4698-89fb-844259fc4041" />

  
Страница корзины покупок пользователя :  
<img width="1403" height="901" alt="image" src="https://github.com/user-attachments/assets/1b414ccd-020e-4966-93a5-09a0a288ce23" />

