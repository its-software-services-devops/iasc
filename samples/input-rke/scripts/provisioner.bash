#!/bin/bash

USER=cicd
DOCKER_VERSION=19.03.14

sudo useradd ${USER}
sudo usermod -aG wheel ${USER}

sudo groupadd docker
sudo usermod -aG docker ${USER}

sudo yum -y install yum-utils
sudo yum-config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo
sudo yum install -y docker-ce-${DOCKER_VERSION} docker-ce-cli-${DOCKER_VERSION}

sudo systemctl start docker
sudo systemctl enable docker
sudo chown root:docker /var/run/docker.sock
