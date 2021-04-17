ARG version
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /install

RUN apt-get -y update
RUN apt-get -y install curl
RUN apt-get -y install unzip

RUN curl -LO https://releases.hashicorp.com/terraform/0.14.7/terraform_0.14.7_linux_amd64.zip
RUN unzip terraform_0.14.7_linux_amd64.zip
RUN cp terraform /usr/local/bin/
RUN terraform -v

RUN curl -LO https://get.helm.sh/helm-v3.5.2-linux-amd64.tar.gz
RUN tar -xvf helm-v3.5.2-linux-amd64.tar.gz
RUN cp linux-amd64/helm /usr/local/bin/
RUN helm version

WORKDIR /source

# copy csproj and restore as distinct layers
COPY iasc-app/* ./iasc-app/
COPY iasc.sln .

RUN ls -lrt
RUN dotnet restore iasc-app/iasc-app.csproj
RUN dotnet publish iasc-app/iasc-app.csproj -c release -o /app --no-restore -p:PackageVersion=${version}


##### final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0

RUN apt-get -y update
RUN apt-get -y install curl
RUN apt-get -y install gnupg2

RUN echo "deb [signed-by=/usr/share/keyrings/cloud.google.gpg] http://packages.cloud.google.com/apt cloud-sdk main" | tee -a /etc/apt/sources.list.d/google-cloud-sdk.list && \
 curl https://packages.cloud.google.com/apt/doc/apt-key.gpg | apt-key --keyring /usr/share/keyrings/cloud.google.gpg  add - && \
 apt-get update -y && \
 apt-get install google-cloud-sdk -y

RUN gcloud version
RUN gsutil version

COPY --from=build /usr/local/bin/terraform /usr/local/bin/
COPY --from=build /usr/local/bin/helm /usr/local/bin/

RUN helm version
RUN terraform -v

WORKDIR /app
COPY --from=build /app .
RUN ls -lrt

ENTRYPOINT ["dotnet", "iasc-app.dll"]
