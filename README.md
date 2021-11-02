# BattleAPI

## What is it?

Retrieves data from CompanionAPI and returns it in the form it used to be in Battlelog. Currently only player counts are supported.

## Usage

1. Fill in the correct credentials in `docker-compose.yml` -file.
2. Run `docker-compose up`.

## Endpoints

### Swagger
http://localhost:5234/swagger/index.html

Works if in development environment or `ENABLE_SWAGGER` environment variable has been set to "true"

### Server
```
http://localhost:5234/api/v1/server/getNumPlayersOnServer/{platform}}/{serverGuid}/{responseType:optional}/

For example:
http://localhost:5234/api/v1/server/getNumPlayersOnServer/pc/4d0151b3-81ff-4268-b4e8-5e60d5bc8765/

http://localhost:5234/api/v1/server/getNumPlayersOnServer/pc/4d0151b3-81ff-4268-b4e8-5e60d5bc8765/0/

http://localhost:5234/api/v1/server/getNumPlayersOnServer/pc/4d0151b3-81ff-4268-b4e8-5e60d5bc8765/1/
```

Or if you know the `gameId`

```
http://localhost:5234/api/v1/server/serverSlots/{gameId}/{responseType:optional}/

For example:
http://localhost:5234/api/v1/server/serverSlots/36028797037504179/

http://localhost:5234/api/v1/server/serverSlots/36028797037504179/0/

http://localhost:5234/api/v1/server/serverSlots/36028797037504179/1/
```

### Persona

#### By name
```
http://localhost:5234/api/v1/persona/personaByName/{soldierName}/{serverGuid:optional}

For example:
http://localhost:5234/api/v1/persona/personaByName/xfileFIN
or
http://localhost:5234/api/v1/persona/personaByName/xfileFIN/4d0151b3-81ff-4268-b4e8-5e60d5bc8765
```

#### By persona
```
http://localhost:5234/api/v1/persona/persona/{eaGuid}/{soldierName}/{serverGuid:optional}

For example:
http://localhost:5234/api/v1/persona/persona/EA_XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX/xfileFIN
or
http://localhost:5234/api/v1/persona/persona/EA_XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX/xfileFIN/4d0151b3-81ff-4268-b4e8-5e60d5bc8765
```
### User

#### By name
```
http://localhost:5234/api/v1/user/getUsersByPersonaNames/?personaNames=xfileFIN&personaNames=T3st1ngM4n
or
http://localhost:5234/api/v1/user/getUsersByPersonaNames/?personaNames=xfileFIN&personaNames=T3st1ngM4n&kind=light
or
http://localhost:5234/api/v1/user/getUsersByPersonaNames/?personaNames=xfileFIN&personaNames=T3st1ngM4n&kind=full
```

#### By personaId
```
http://localhost:5234/api/v1/user/getUsersByPersonaIds/?personaIds=806262072&personaIds=1058743680
or
http://localhost:5234/api/v1/user/getUsersByPersonaIds/?personaIds=806262072&personaIds=1058743680&kind=light
or
http://localhost:5234/api/v1/user/getUsersByPersonaIds/?personaIds=806262072&personaIds=1058743680&kind=full
```

#### By userId
```
http://localhost:5234/api/v1/user/getUsersByIds/?userIds=2832663161901751610&userIds=3307924268968953779
or
http://localhost:5234/api/v1/user/getUsersByIds/?userIds=2832663161901751610&userIds=3307924268968953779&kind=light
or
http://localhost:5234/api/v1/user/getUsersByIds/?userIds=2832663161901751610&userIds=3307924268968953779&kind=full
```
## Usage in stack

### Deploying the stack
```
docker stack deploy -c docker-compose.yml battleapi
```

### Removing the stack
```
docker stack rm battleapi
```

### Updating the stack

#### Pulling an updated image
```
docker pull xfilefin/battleapi:latest
```

#### Updating the stack to use the new image
`--force` required if you have the same tag as the currently running one. E.g. `latest`
```
docker service update --image xfilefin/battleapi:latest battleapi_backend --force
```

## Building

`docker build -t xfilefin/battleapi . --file BattleAPI/Dockerfile`

## Credits

- Philipp Wagner for [TimescaleDB stuff](https://www.bytefish.de/blog/timeseries_databases_3_timescaledb.html)