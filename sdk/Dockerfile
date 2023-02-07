FROM mcr.microsoft.com/dotnet/core/sdk:3.1

RUN mkdir -p /usr/src
WORKDIR /usr/src

COPY Finbourne.Workflow.Sdk.Extensions.sln /usr/src/
COPY Finbourne.Workflow.Sdk.Extensions/Finbourne.Workflow.Sdk.Extensions.csproj /usr/src/Finbourne.Workflow.Sdk.Extensions/
COPY Finbourne.Workflow.Sdk.Extensions.Tests/Finbourne.Workflow.Sdk.Extensions.Tests.csproj /usr/src/Finbourne.Workflow.Sdk.Extensions.Tests/
COPY Finbourne.Workflow.Sdk.Extensions.IntegrationTests/Finbourne.Workflow.Sdk.Extensions.IntegrationTests.csproj /usr/src/Finbourne.Workflow.Sdk.Extensions.IntegrationTests/
RUN dotnet restore Finbourne.Workflow.Sdk.Extensions

ENV FBN_WORKFLOW_API_URL ${FBN_WORKFLOW_API_URL}

# ENTRYPOINT [ "/bin/bash" ]
ENTRYPOINT dotnet test --configuration Release --verbosity normal Finbourne.Workflow.Sdk.Extensions

