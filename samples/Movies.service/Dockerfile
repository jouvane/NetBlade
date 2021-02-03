FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

EXPOSE 8080
EXPOSE 80
EXPOSE 443

#Copias de arquivos para a imagem
COPY . .
WORKDIR "Movies.API/"

ARG version_variable=0.1.1

#Configuracoes para download de dependencias
RUN echo "ipv6" >> /etc/modules
RUN apt-get update
RUN apt install -y gss-ntlmssp

#Build
RUN dotnet build "Movies.API.csproj" -c Release -p:Version=$version_variable -o /app/build

FROM build AS publish
RUN dotnet publish "Movies.API.csproj" -c Release -o /app/publish
RUN echo { \"DATE_RELEASE\": \"$(date "+%F-%H-%M-%S")\" } > /app/publish/env.json

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

#Configuracoes de Timezone
RUN apk add --no-cache icu-libs tzdata
RUN cp /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime
RUN echo "E. South America Standard Time" >  /etc/timezone

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movies.API.dll"]
