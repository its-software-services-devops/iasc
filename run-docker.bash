#/bin/bash

VERSION=develop-f1db0ed 

sudo docker run \
-v $(pwd)/samples/wip/output:/wip/output \
-v ${HOME}/.config/gcloud:/root/.config/gcloud \
-e IASC_VCS_MODE=git \
-e IASC_VCS_URL=https://github.com/its-software-services-devops/infra-devops.git \
-e IASC_VCS_REF=develop \
-e IASC_VCS_FOLDER=gce-rke-manager \
-it gcr.io/its-artifact-commons/iasc:${VERSION} \
init
