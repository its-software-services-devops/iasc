ARG version
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /install

RUN apt-get -y update
RUN apt-get -y install curl
RUN apt-get -y install unzip

RUN curl -LO https://releases.hashicorp.com/terraform/1.0.5/terraform_1.0.5_linux_amd64.zip
RUN unzip terraform_1.0.5_linux_amd64.zip
RUN cp terraform /usr/local/bin/
RUN terraform -v

RUN curl -LO https://get.helm.sh/helm-v3.11.1-linux-amd64.tar.gz
RUN tar -xvf helm-v3.11.1-linux-amd64.tar.gz
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
RUN apt-get -y install git

RUN echo "deb [signed-by=/usr/share/keyrings/cloud.google.gpg] http://packages.cloud.google.com/apt cloud-sdk main" | tee -a /etc/apt/sources.list.d/google-cloud-sdk.list && \
 curl https://packages.cloud.google.com/apt/doc/apt-key.gpg | apt-key --keyring /usr/share/keyrings/cloud.google.gpg  add - && \
 apt-get update -y && \
 apt-get install google-cloud-sdk -y

RUN gcloud version
RUN gsutil version && which gsutil
RUN git --version

COPY --from=build /usr/local/bin/terraform /usr/local/bin/
COPY --from=build /usr/local/bin/helm /usr/local/bin/

RUN helm version
RUN terraform -v

WORKDIR /app
COPY --from=build /app .
RUN ls -lrt
RUN dotnet iasc-app.dll info

RUN mkdir -p /wip/input
RUN mkdir -p /wip/output

ENV IASC_SRC_DIR=/wip/input
ENV IASC_WIP_DIR=/wip/output
ENV IASC_TMP_DIR=/tmp
ENV IASC_VCS_MODE=local
ENV IASC_GSUTIL_PATH=/usr/bin/gsutil

ENTRYPOINT ["dotnet", "iasc-app.dll"]
