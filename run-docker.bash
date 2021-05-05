#/bin/bash

sudo docker run \
-e IASC_VCS_MODE=git \
-e IASC_SRC_DIR=/wip \
-e IASC_WIP_DIR=/wip \
-e IASC_TMP_DIR=/tmp \
-e IASC_VCS_URL=https://github.com/its-software-services-devops/infra-devops.git \
-e IASC_VCS_REF=develop \
-e IASC_VCS_FOLDER=gce-rke-manager \
-it gcr.io/its-artifact-commons/iasc:develop-5ae809a \
init
