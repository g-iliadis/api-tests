# API Test Suite

Automated API testing framework using C# & .NET 8 


### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or Rider

### Setup
```bash
# Clone repository
git clone 

# Restore packages
dotnet restore

# Build project
dotnet build

# Run tests
dotnet test
```

## Test Scenarios

The framework tests the following API endpoints:

### Users API (`/api/users`)
- ✅ **Happy Path**: Retrieve users with valid page parameter
- ✅ **Pagination**: Verify different pages return different data
- ✅ **Edge Cases**: Handle invalid page parameters


## Technology Stack

- **.NET 8** - Target framework
- **Reqnroll** - BDD framework with Gherkin
- **NUnit** - Test runner
- **RestSharp** - HTTP client for API calls

## Writing Tests

### 1. Add Gherkin Scenarios
Create `.feature` files in the `Features/` folder:

### 2. Implement Step Definitions
Add corresponding C# methods in `StepDefinitions/`:


## Running Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific feature
dotnet test --filter "FullyQualifiedName~Users"
```

## CI/CD Integration

### GitHub Actions - Check Workflows tests.yml
```yaml
- name: API Tests
```
