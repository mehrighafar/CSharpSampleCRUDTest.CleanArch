
# Clean Architecture Sample Project

This repository contains a sample project demonstrating **Clean Architecture** principles in a .NET application. It incorporates various modern development tools and practices, including OpenTelemetry, Jaeger, Prometheus, Loki, MediatR, Fluent Validation, Domain-driven design (DDD), and Test-driven design (TDD). The project also includes unit tests, integration tests, and functional tests, all running in a fully containerized environment using Docker.

## Project Structure

The project follows a layered architecture with a focus on separation of concerns and scalability:

### **Source Code (`src` Directory)**
- **`Use Case` Layer**: Contains core business logic, MediatR handlers, and Fluent Validation validators.
- **`Core` Layer**: Implements domain models and business rules following Domain-Driven Design principles.
- **`Infrastructure` Layer**: Handles external dependencies such as database access and third-party integrations.
- **`Web` Layer**: Provides the REST API endpoints and OpenTelemetry instrumentation for request tracing and logging.

### **Tests (`tests` Directory)**
- **Unit Tests**: Test the smallest units of code in isolation.
- **Integration Tests**: Verify the interactions between multiple components (e.g., database access).
- **Functional Tests**: Simulate real-world usage scenarios by testing the application end-to-end.

## Features

### **Clean Architecture Principles**
- Strict separation of concerns across layers.
- Dependency inversion to keep the core domain independent of external frameworks.
- Well-defined boundaries between layers.

### **Observability**
- **OpenTelemetry**: Integrated for distributed tracing and metrics collection.
- **Jaeger**: Displays tracing information for debugging and performance analysis.
- **Prometheus**: Collects application metrics.
- **Loki**: Stores and queries logs.

### **Validation**
- **FluentValidation**: Centralized and extensible validation framework for request and business rule validation.

### **Messaging and CQRS**
- **MediatR**: Implements the Command-Query Responsibility Segregation (CQRS) pattern for managing commands and queries.

### **Dockerized Environment**
- Fully containerized setup using Docker Compose.
- Includes all necessary services for the application: MongoDB, OpenTelemetry Collector, Jaeger, Prometheus, Loki, and Grafana.

### **Additional Highlights**
- **.NET 9**: Built with the latest .NET version for cutting-edge features.
- **`api.http`**: Provides API testing scripts for quick validation.
- **TestContainers**: Simplifies running integration tests and functional tests with containers.

## Getting Started

### Prerequisites
- [.NET 9](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)

### Running the Application
1. Clone the repository:
   ```bash
   git clone <repo-url>
   cd <repo-directory>
   ```
2. Create a `.env` file in the `src/CSharpSampleCRUDTest.CleanArch.Web`  directory with the following content:
   ```env
   MONGODB_CONNECTION_STRING=mongodb://mongodb:27017
   MONGODB_DATABASE_NAME=SampleDb
   MONGODB_COLLECTION_NAME=Customer

   COLLECTOR=http://otel-collector:4317
   LOKI=http://loki:3100
   ```
3. Start the application and its dependencies using Docker Compose:
   ```bash
   docker-compose up --build
   ```
4. Access the application:
   - **Jaeger UI**: [http://localhost:16686](http://localhost:16686)
   - **Prometheus**: [http://localhost:9090](http://localhost:9090)
   - **Grafana**: [http://localhost:3000](http://localhost:3000) (Default credentials: admin/admin)

### Running Tests
1. Navigate to the `tests` directory.
2. Create a `.env` file in the `tests/CSharpSampleCRUDTest.CleanArch.FunctionalTests`  directory with the following content:
   ```env
   MONGODB_CONNECTION_STRING=mongodb://localhost:27018 
   MONGODB_DATABASE_NAME=SampleTestDb
   MONGODB_COLLECTION_NAME=CustomerEntity
   API_BASE_ADDRESS=http://localhost:57679

   COLLECTOR=http://otel-collector:4317
   LOKI=http://loki:3100
   ```
3. Run the unit, integration, or functional tests:
   ```bash
   dotnet test
   ```

## Configuration

### **OpenTelemetry Configuration**
The OpenTelemetry Collector (`otel-collector`) is configured using `otel-collector-config.yaml`. Update this file to customize the receivers, processors, and exporters.

### **Prometheus Configuration**
Prometheus is configured using `prometheus.yml`. Add or modify scrape jobs as needed to monitor additional services.

### **Loki Configuration**
Loki is configured via `loki-config.yaml`. Update storage and schema settings as required.

## Key Dependencies
- **OpenTelemetry**: For tracing and metrics.
- **Jaeger**: Distributed tracing visualization.
- **Prometheus**: Metrics collection.
- **Loki**: Centralized logging.
- **MongoDB**: NoSQL database for application data.
- **MediatR**: Implements CQRS pattern.
- **FluentValidation**: Validates inputs and business rules.
- **TestContainers**: Simplifies containerized integration tests and functional tests.

## Contribution
Contributions are welcome! Please fork the repository and submit a pull request.

---
Feel free to reach out with any questions or feedback. Happy coding!
