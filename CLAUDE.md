# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Commands

### Running the application
```bash
dotnet run                    # Runs both server (port 5000) and client (port 8080) in watch mode
```

### Testing
```bash
dotnet run -- WatchRunTests   # Run tests in watch mode with browser UI
dotnet run -- RunTestsHeadless # Run tests headlessly
```

### Building and deployment
```bash
dotnet run -- Bundle          # Create production build
dotnet run -- Azure           # Deploy to Azure
```

### Code formatting
```bash
dotnet run -- Format          # Format F# code using Fantomas
```

## Architecture Overview

This is a **SAFE Stack** application using F# throughout:

### Technology Stack
- **Client**: Fable (F# to JS), Elmish (MVU architecture), React, TailwindCSS
- **Server**: Saturn web framework on .NET 8.0
- **Shared**: Type-safe contracts and domain models shared between client and server
- **Build**: FAKE build script (Build.fs), Vite for client bundling

### Project Structure
```
src/
├── Client/   # Fable/React SPA with Elmish MVU pattern
├── Server/   # Saturn API server
└── Shared/   # Shared types and contracts (ITodosApi interface)

tests/
├── Client/   # Mocha tests (port 8081)
├── Server/   # Expecto tests
└── Shared/   # Shared logic tests
```

### Key Architectural Patterns

1. **Elmish MVU Pattern**: The client uses Model-View-Update with:
   - `Model` type containing application state
   - `Msg` discriminated union for all possible messages
   - `update` function for state transitions
   - `view` function for React rendering

2. **Type-Safe API**: The `ITodosApi` interface in Shared defines the contract between client and server, ensuring compile-time type safety across the full stack.

3. **RemoteData Pattern**: Client handles async operations with explicit loading/error states.

4. **FAKE Build Automation**: All build tasks are defined in Build.fs using the FAKE DSL.

### Development Workflow

When modifying the application:
1. Shared types go in `src/Shared/Shared.fs`
2. Server implementation in `src/Server/Server.fs`
3. Client logic in `src/Client/Index.fs`
4. Styles use TailwindCSS utility classes

The application auto-reloads on file changes when running `dotnet run`.

### Testing Approach

- Server tests use Expecto framework with `testList` and `testCase`
- Client tests use Mocha with Fable bindings
- Run tests in watch mode during development for immediate feedback