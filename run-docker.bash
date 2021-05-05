#/bin/bash

sudo docker run \
-e IASC_TMP_DIR=/wip \
-e IASC_VCS_MODE=git \
-e IASC_VCS_URL=https://github.com/its-software-services-devops/infra-devops.git \
-e IASC_VCS_REF=develop \
-e IASC_VCS_FOLDER= \
-it gcr.io/its-artifact-commons/iasc:develop-ef644a0 \
init
