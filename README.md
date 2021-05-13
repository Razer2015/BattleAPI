# BattleAPI

## What is it?

Retrieves data from CompanionAPI and returns it in the form it used to be in Battlelog. Currently only player counts are supported.

## Usage

1. Fill in the correct credentials in `docker-compose.yml` -file.
2. Run `docker-compose up`.
3. Open 
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