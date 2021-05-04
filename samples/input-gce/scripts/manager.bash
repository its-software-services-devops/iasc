#!/bin/bash

USER=cicd
DOCKER_VERSION=19.03.14

sudo useradd ${USER}
sudo usermod -aG wheel ${USER}

# docker
sudo groupadd docker
sudo usermod -aG docker ${USER}

sudo yum -y install yum-utils
sudo yum-config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo
sudo yum install -y docker-ce-${DOCKER_VERSION} docker-ce-cli-${DOCKER_VERSION}

sudo systemctl start docker
sudo systemctl enable docker
sudo chown root:docker /var/run/docker.sock

# unzip
sudo yum -y install unzip

# terraform
TERRAFORM_VERSION=0.14.6
TERRAFORM_URL=https://releases.hashicorp.com/terraform/${TERRAFORM_VERSION}/terraform_${TERRAFORM_VERSION}_linux_amd64.zip
curl -LO "${TERRAFORM_URL}"
unzip "terraform_${TERRAFORM_VERSION}_linux_amd64.zip"
chmod 555 terraform
sudo mv ./terraform /usr/local/bin/terraform

# kubectl
KUBECTL_VERSION=v1.19.0
KUBECTL_URL=https://storage.googleapis.com/kubernetes-release/release
curl -LO "${KUBECTL_URL}/${KUBECTL_VERSION}/bin/linux/amd64/kubectl"
chmod 555 kubectl
sudo mv ./kubectl /usr/local/bin/kubectl

# helm 
HELM_CLI_VERSION=v3.3.1
HELM_URL=https://get.helm.sh/helm-${HELM_CLI_VERSION}-linux-amd64.tar.gz
curl -LO "${HELM_URL}"
tar -xvf helm-${HELM_CLI_VERSION}-linux-amd64.tar.gz
chmod 555 ./linux-amd64/helm
sudo mv ./linux-amd64/helm /usr/local/bin/helm

# git
sudo yum -y install git
