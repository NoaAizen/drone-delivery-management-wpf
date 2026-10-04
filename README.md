# Drone Delivery Management System

A C# and WPF application for managing a drone-based parcel delivery workflow, designed with a layered architecture that separates presentation, business logic, and data access.

The system manages drones, customers, parcels and charging stations. It includes automated drone simulation, battery and distance calculations, XML-based persistence, and separate manager and customer workflows.

## Highlights

- **Three-layer architecture:** Presentation (PL) → Business (BL) → Data Access (DAL), with each layer used only through an interface.
- **Interfaces and factories:** `IBL` / `IDal` contracts, created through `BlFactory` / `DalFactory`, with singleton implementations.
- **Separate models per layer:** data objects (DO), business objects (BO) and presentation objects (PO), with explicit conversion between them.
- **XML persistence:** built on LINQ to XML and `XmlSerializer`. An interchangeable in-memory DAL is also included.
- **Domain logic:**
  - drone lifecycle and parcel delivery workflow;
  - parcel-to-drone assignment;
  - battery consumption and distance calculations;
  - charging station management.
- **Background drone simulator:** runs on a `BackgroundWorker`, with synchronized access to the shared layers.
- **WPF / XAML UI:** data binding, filtering and grouping, plus separate manager and customer flows.

## Features

### Drone lifecycle and delivery workflow

A drone moves through a full delivery cycle:

1. **Available:** the drone is waiting at a station or at its last delivery point.
2. **Parcel assignment:** the drone is matched with a waiting parcel. Parcels are chosen by priority, then by the weight the drone can carry, then by distance. A parcel is assigned only if the drone has enough battery to reach the sender, the target, and then a charging station.
3. **Pickup:** the drone flies to the sender and collects the parcel.
4. **Delivery:** the drone flies to the target customer and delivers the parcel.
5. **Charging:** the drone is sent to the nearest station with a free charging slot, and released once it's charged.

### Battery and distance calculations

- Distances are calculated from geographic coordinates.
- Power consumption per kilometer depends on whether the drone is empty or carrying a light, medium or heavy parcel.
- The consumption rates and the charging rate are stored in the XML configuration.

### Management

- **Drones:** add drones, update the model, view details, and filter the list by status and weight category.
- **Customers:** add and update customers, and view the parcels each one has sent and received.
- **Parcels:** create, view, filter, group and delete parcels with weight and priority.
- **Charging stations:** add and update stations, track charging slots, and list stations with free slots.

### Manager and customer flows

- **Manager:** full access to drones, customers, parcels and stations.
- **Customer:**
  - register, sign in and reset a password;
  - send parcels and see only their own parcels;
  - confirm pickup and receipt of parcels.
- **Contact:** a contact button in the customer area opens an external Google Form.

### Background drone simulator

The simulator moves a drone through its lifecycle automatically, with no user input. It is started from the drone window and runs on a `BackgroundWorker`. On each cycle it:

- assigns a parcel when the drone is available,
- picks up and then delivers the parcel,
- sends the drone to charge when no parcel can be assigned, and
- charges the battery gradually, then releases the drone.

After each step the UI refreshes through progress callbacks. Changes to the shared business and data layers are guarded with locks. The simulator can be stopped at any time.

## Architecture

```
┌──────────────────────────────┐
│  PL  – WPF / XAML            │  windows, data binding, PO models
└──────────────┬───────────────┘
               │  IBL  (BlFactory)
┌──────────────▼───────────────┐
│  BL  – Business logic        │  validation, delivery workflow,
│                              │  battery/distance, simulator, BO models
└──────────────┬───────────────┘
               │  IDal (DalFactory)
┌──────────────▼───────────────┐
│  DAL – Data access           │  DalXml (XML files)  |  DalObject (in-memory)
│                              │  DO models
└──────────────────────────────┘
```

- **PL** depends only on `IBL`, and **BL** depends only on `IDal`. The implementations are created through factories and exposed as singletons.
- **DalXml** stores data in XML files under `DAL/xml`, using both LINQ to XML (`XElement`) and `XmlSerializer`. This is the implementation the BL uses.
- **DalObject** is an in-memory implementation. It also generates the initial demo data that fills the XML files.

## Tech Stack

C# · .NET 5 · WPF · XAML · LINQ · LINQ to XML · XML serialization · `BackgroundWorker` / threading

## Repository Structure

| Folder | Description |
| --- | --- |
| `PL/` | WPF presentation layer: windows, presentation objects (`PO/`) and image assets (`image/`). |
| `BL/` | Business layer: `IBL` and factory (`BlApi/`), business objects (`BO/`), core logic (`BL.cs`) and the simulator (`Simulator.cs`). |
| `DAL/` | Data access layer: `IDal` and factory (`DalApi/`), data objects (`DO/`), in-memory (`DalObject/`) and XML (`DalXml/`) implementations, and the XML data files (`xml/`). |
| `ConsoleUI/` | Development and testing utility for exercising the DAL directly from the console. |
| `ConsoleUI_BL/` | Development and testing utility for exercising the BL directly from the console. |

## Getting Started

**Requirements:**
- Windows
- .NET 5 SDK
- Visual Studio 2019 (16.8 or later) or Visual Studio 2022, with the .NET desktop development workload

**Steps:**
1. Open `DroneDelivery.sln`.
2. Set **PL** as the startup project.
3. Build and run in the default **Debug** configuration.

**Demo accounts.** Each time the application starts, it regenerates its demo data with fictional users:

| Role | Username | Password |
| --- | --- | --- |
| Manager | `manager` | `manager123` |
| Customer | `alice`, `bob`, `carol`, `david`, `emma`, `frank`, `grace`, `henry`, `irene`, `jack` (IDs 0–9) | same as the username |

Customer sign-in also asks for the customer ID.

## Technical Notes

- **Framework:** targets .NET 5, which is now out of support. This cleaned version of the repository has not been rebuilt yet, because the .NET SDK was not installed on the machine used to prepare it.
- **Data paths:** the XML data files are found through a path relative to the default build output folder (`PL/bin/Debug/net5.0-windows/`). Run the app from that location.
- **Demo data:** it is regenerated on every start, so changes are not kept between runs.
- **Authentication:** for demonstration only. The manager account is hard-coded, and passwords are stored in plain text.
- **Comments:** most code comments are written in Hebrew.

## Credits

Developed collaboratively by **Noa Aizen** and **Oriya Aharoni**.
