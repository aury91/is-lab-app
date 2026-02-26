# ---- Stage 1: Build ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY IsLabApp.csproj .
RUN dotnet restore

# Копируем остальные файлы и публикуем приложение
COPY . .
RUN dotnet publish -c Release -o /app/publish

# ---- Stage 2: Runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Копируем результат сборки из первого этапа
COPY --from=build /app/publish .

# Указываем, какой порт будет слушать приложение внутри контейнера
EXPOSE 8080

# Запускаем приложение
ENTRYPOINT ["dotnet", "IsLabApp.dll"]
