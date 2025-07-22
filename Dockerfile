FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HallOfFameAPI/HallOfFameAPI.csproj", "HallOfFameAPI/"]
RUN dotnet restore "HallOfFameAPI/HallOfFameAPI.csproj"

COPY . .
WORKDIR "/src/HallOfFameAPI"
RUN dotnet build "HallOfFameAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HallOfFameAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "HallOfFameAPI.dll"]