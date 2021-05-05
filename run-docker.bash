#/bin/bash

VERSION=develop-901beaf 

sudo docker run \
-v $(pwd)/samples/wip/output:/wip/output \
-e IASC_VCS_MODE=git \
-e IASC_VCS_URL=https://github.com/its-software-services-devops/infra-devops.git \
-e IASC_VCS_REF=develop \
-e IASC_VCS_FOLDER=gce-rke-manager \
-it gcr.io/its-artifact-commons/iasc:${VERSION} \
init
