FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ParsingHTML.csproj", "ParsingHTML/"]
WORKDIR "/src/ParsingHTML"
RUN dotnet restore "ParsingHTML.csproj"
COPY . .
RUN dotnet build "ParsingHTML.csproj" -c Release -o /ParsingHTML/build

FROM build AS publish
RUN dotnet publish "ParsingHTML.csproj" -c Release -o /ParsingHTML/publish

FROM base AS final
WORKDIR /ParsingHTML
COPY --from=publish /ParsingHTML/publish .
ENTRYPOINT ["dotnet", "ParsingHTML.dll"]