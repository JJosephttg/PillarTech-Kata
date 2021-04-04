FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY . .
WORKDIR /app/src

RUN dotnet build "PillarTech-Kata.sln" -c "Release" -o /app/build

ENTRYPOINT ["dotnet", "test", "/app/build/CheckoutOrderTotalTests.dll"]
