#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ToDoListApp.Client/ToDoListApp.Client.csproj", "ToDoListApp.Client/"]
COPY ["ToDoListApp.Domain/ToDoListApp.Domain.csproj", "ToDoListApp.Domain/"]
RUN dotnet restore "ToDoListApp.Client/ToDoListApp.Client.csproj"
COPY . .
WORKDIR "/src/ToDoListApp.Client"
RUN dotnet build "ToDoListApp.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDoListApp.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoListApp.Client.dll"]