# Лабораторная работа №2

Работа выполнена на языке C# на платформе .NET 5.

## Запуск приложения, используя Docker

Для запуска контейнера необходимо наличие Docker.

- Выполняем сборку контейнера: `docker build -t lab2 .`
- Выполняем запуск контейнера: `docker run --rm -p 8080:80 --name lab2 lab2`

Приложение будет доступно по адресу: [http://localhost:8080](http://localhost:8080)

### Примечание

Для корректной работы на другом порту, отличном от 8080, необходимо изменить параметр `AppPort` в файле `appsettings.json`

## Запуск приложения, используя dotnet runtime

Для запуска из исходного кода необходимо наличие .NET SDK v5.0.100 или выше. Загрузить можно [тут](https://dotnet.microsoft.com/download/dotnet/5.0).

В файле `appsettings.json`, лежащем в корне проекта необходимо указать параметры `ApplicationId` и `SecretKey`, соответствующие идентификатору и защищенному ключу своего приложения. Или можно оставить значения по умолчанию.

Затем командой `dotnet run`, выполняемой в корне проекта, запускаем веб-приложение.

Приложение будет слушать входящие соединения по адресу: [http://localhost:5000](http://localhost:5000).
