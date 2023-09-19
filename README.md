# Azure Functions Event Hubs to Cosmos DB

## Overview

This repository provides examples of [Azure Functions](https://learn.microsoft.com/azure/azure-functions/functions-overview) which process events from [Event Hubs](https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-about) and store them in [Cosmos DB](https://learn.microsoft.com/en-us/azure/cosmos-db/introduction).

Each implementation provides 2 functions:

### BatchEventsHubToCosmosDB

`BatchEventsHubToCosmosDB` processes events in batches and stores them in Cosmos DB in one go.
It consumes events from the Consumer Group identified by `EVENT_HUB_BATCH_CONSUMER_GROUP` and stores processed events in the Cosmos DB container identified by `COSMOSDB_BATCH_EVENTS_CONTAINER`.


### IndividualEventsHubToCosmosDB

`IndividualEventsHubToCosmosDB` processes events individually, 1 execution per event.
It consumes events from the Consumer Group identified by `EVENT_HUB_INDIVIDUAL_CONSUMER_GROUP` and stores processed events in the Cosmos DB container identified by `COSMOSDB_INDIVIDUAL_EVENTS_CONTAINER`.


## Configuration

| App setting                          | Description                                                |
|--------------------------------------|------------------------------------------------------------|
| EVENT_HUB_CONNECTION_STRING          | Event Hubs connection string                               |
| EVENT_HUB_NAME                       | Name of the hub                                            |
| EVENT_HUB_BATCH_CONSUMER_GROUP       | Name of the consumer group for batch processing            |
| EVENT_HUB_INDIVIDUAL_CONSUMER_GROUP  | Name of the consumer group for individual event processing |
| COSMOSDB_CONNECTION_STRING           | Cosmos DB connection string                                |
| COSMOSDB_DATABASE                    | Cosmos DB database name                                    |
| COSMOSDB_BATCH_EVENTS_CONTAINER      | Name of the container for batch-processed events           |
| COSMOSDB_INDIVIDUAL_EVENTS_CONTAINER | Name of the container for individually-processed events    |


## Deployment

You can use [Azure Functions Core tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=linux%2Cisolated-process%2Cnode-v4%2Cpython-v2%2Chttp-trigger%2Ccontainer-apps&pivots=programming-language-csharp#publish) to deploy functions to Azure.

```sh
# Replace $FUNCTION_APP_NAME with the name of the Function App
func azure functionapp publish $FUNCTION_APP_NAME
```

## Testing

You can use the [Data generator or Events Hub](https://learn.microsoft.com/en-us/azure/event-hubs/send-and-receive-events-using-data-generator) to generate new events from the `Yellow Taxi` data set and test the functions in this project.

