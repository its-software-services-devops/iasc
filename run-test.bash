#!/bin/bash
# Info : https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage?tabs=windows

COVERAGE_PATH=iasc-test/TestResults

rm -rf ${COVERAGE_PATH}

dotnet test --collect:"XPlat Code Coverage" --results-directory:"${COVERAGE_PATH}" 
EXIT_CODE=$?

echo ""
echo ""

if [[ ${EXIT_CODE} -eq 0 ]]
then
    COVERAGE_FILE=$(find . -name 'coverage.cobertura.xml' | head -1)
    
    reportgenerator \
    "-reports:${COVERAGE_FILE}" \
    "-targetdir:coveragereport" \
    -reporttypes:Html \

    echo ""
    echo ""
    echo "#### Please see coverage report at ./coveragereport/index.html"
fi
