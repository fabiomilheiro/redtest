# Introduction

The clients posts 2 probabilities (0-1) and one type of calculation to which the app responds with a result.

## Request example:

```
curl -X POST \
  https://localhost:5001/calculations \
  -H 'cache-control: no-cache' \
  -H 'content-type: application/json' \
  -d '{
	"a": 0.5,
	"b": 0.6,
	"calculationType": 1
}'
```

## Bad request response example:

```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "|a15c00e3-4504610a782b63c9.",
    "errors": {
        "CalculationType": [
            "The CalculationType specified is not valid.\r\nValid values: 0 (Combined), 1 (Either)"
        ]
    }
}
```

## Successful response example:

```json
{
    "value": 0.8
}
```

## Technical implementation summary

### ICalculation

Abstraction to that calculations must implement.

By forcing the need to implement an interface, we allow for more calculations to be introduced without the need to change other working parts of the program.

### CalculationFactory

Abstract factory which takes a calculation type enum and returns the appropriate calculation implementation.

If the type supplied is not valid, then throws an exception.

This is done so it becomes more obvious to the developer in case there's a bug (better than a null reference exception further down the line).

However, it's a case that is not expected to happen as models are validated before the controller executes.

### CalculationService

Orchestrator that calls validation, the calculation factory, the repository and returns a domain object with the result of the calculation.

### CalculationAuditRepository

The repository used to save calculation data. Current implementation is files.

### Models

The models exposed to the client.

### Unit tests

I used TDD approach to develop each of the parts of the app described above.

### Service tests

I created another project to also test the service as a component in order to prove that all parts of the app integrate well with each other.

I took care not to test individual class features that are already tests in the class tests.

How I implemented this was using the `WebApplicationFactory` to create an app and client for each test.

One thing to note is that for such component tests we should isolate the app from external dependencies which, in our case, is the file system.

The replacement of this dependency happens in the func passed to `IWebHostBuilder.ConfigureTestServices` extension method.