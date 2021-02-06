# Docker Quick Start

1. Clone this repository.

2. Build the docker image:

```shell
docker build --build-arg BUILD_CONFIGURATION=Build . -t sql2cdm
```

4. Execute the CLI:

```shell
docker run -v $(pwd):/data sql2cdm -i /data/sql/customer.sql -o /data/cdm-generated
```
