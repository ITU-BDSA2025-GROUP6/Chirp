---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2025 Group `6`
author:
- "Andreas <anys@itu.dk>"
- "Maja <mcls@itu.dk>"
- "Sebastian <sebseb10@gmail.com>"
- "Ludvig <ludvig.elias.kirkeby@gmail.com>"
- "Anna <anna.kirstine.hoff@gmail.com>"
- "Nina <cenh@itu.dk>"
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain model

Chirp has the following key entities:

    - Author: Enables user management through extending ASP.NETs IdentityUser
    - Cheep: Represents posts by authors using timestamps and text.
    - Follows: Tracks following, and being followed by, other authors.
    - Recheep: Allows reposting specific cheeps by other authors on our own timeline.
    
Below is an UML class diagram of our domain model: 

<img width="800" height="880" alt="image" src="images/DomainModel.png" />

- **NOTE:** `IdentityUser` only shows the first part of the library it comes from in the diagram.  
  The full library path is:  
  `Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser`

- **NOTE:** While not shown in the diagram, all three classes connected to **Author**  
  (`Cheep`, `Recheep`, and `Follows`) use  
  `System.ComponentModel.DataAnnotations` to support `required` parameters, among others.


## Architecture — In the small
The Onion Architecture of Chirp is seen in the below UML. 

    - UI Layer: Outer layer seen by the Client. Includes Pages, Identity Core scaffolded Pages, Startup code through Program.cs.
    - Service Layer: Data flow between our repository, the UI layer and the Client.
    - Repository Layer: Functionality, DTOs, Interfaces and database retrieval methods supported by Entity Core.
    - Domain Layer: Contains our domain entities only.

<img width="800" height="880" alt="image" src="images/Onion Architecture.png" />

## Architecture of deployed application
The Deployed Application Architecture can be seen below.


#### Remote Architecture
Client interacts with an Azure Database through an HTTP (Converted to HTTPS) request. Hosted offshore. 

<img width="800" height="880" alt="image" src="images/Deployed Application Azure Architecture.png" />

#### Local Architecture 
Client interacts directly with a local database. Does not require internet.

<img width="400" height="480" alt="image" src="images/Deployed Application Locally Hosted Architecture.png" />

## User activities
We have two types of users: `Authorized `and `Unauthorized`.

**Access Restriction**

An `Unauthorized User` may...

    - Login and Register 
    - View Public and Private timelines
    - View multiple pages of content

An `Authorized User` may...

    - Logout
    - View Public and Private timelines
    - View own Timeline, with own posted Cheeps
    - View multiple pages of content
    - Access an "About me" page, with account information and account deletion ("Forget me")
    - Set a Profile Picture in the "About me" page
    - Post new Cheeps
    - Recheep other Authors' Cheeps

<img width="417" height="981" alt="image" src="images/Unauthorized User Activity Diagram.jpg" />


<img width="1130" height="1691" alt="image" src="images/Authorized User Activity.jpg" />

## Sequence of functionality/calls trough _Chirp!_
Below is a Diagram of an example functionality call of an Unauthorized user accessing the site, causing us to display all cheeps, which are grabbed from the database.

<img src="images/Functionality Calls.png" alt="image" style="max-width: 150%; height=auto;"/>

# Process

## Build, test, release, and deployment
We have created three workflows for the different tasks:
- **CI:** Automatically builds and tests the application on every change, and publishes a versioned release artifact when a release tag is pushed.
- **CD:** Build and deploy to Azure Web App.
- **Executables:** Builds, tests, and publishes single-file executables for multiple operating systems when a GitHub release is created.
- **Auto Label:** Automatically labels issues based on keywords.

The first diagram is over our Continuous Integration workflow, which ensures build- and test correctness. In addition it publishes release webapp artifact on tags, with both title and release notes.

<img src="images/CiDiagram.png" alt="CI workflow diagram" style="max-width:80%; height:auto;">

The second diagram illustrates the Continuous Deployment workflow, which consists of a build- and deploy job. This workflow is triggered on 'push' to the main branch.
The workflow ensures that the ASP.NET application is build, published and deployed to Azure App Service.

<img src="images/CdDiagram.png" alt="CD workflow diagram" style="max-width:100%; height:auto;">

The fourth diagram is created over the Auto Label workflow. It shows how labels are automatically assigned to newly opened or edited issues based on predefined keywords, such as "bug", "layout", etc. 
<img src="images/AutoLabel.png" alt="Auto Label diagram" style="max-width:100%; height:auto;">


## Team work
This is our project board at hand-in. The following tasks remain unresolved:
INSÆT BILLEDE AF PROJECT BOARD OG LISTE AF UNRESOLVED TASKS + FORKLARING

The structure around which we have organised our work, can be described with the following sequence diagram.

<img src="images/image.png" alt="TeamworkSequenceUML" style="max-width:100%; height:auto;">

As illustrated in the picture, our process had the following main components:

- **Creating an issue:** During the first two steps of the sequence, the project manager creates a new issue. This issue is based either on the requirements presented the given week or on bugs found during development. After creating the issue, including a description and success requirements, a developer can be assigned to it.
- **Code development:** The issue is then picked up by a developer from the team. In the next steps of the sequence, the developer creates a new designated branch for the issue, develops the feature, pushes the code to the GitHub repository, and finaly requests review of the implemented code.
- **Code review:** GitHub then notifies the team about the pull request, and a reviewer reviews the code. The review is based on the success criteria defined when the issue was created. The reviwer summits the review through GitHub, which then notifies the developer whether the pull request apporved or changes are requested. If changes are requested, the developer updates the code, pushes the changes and sends a new review request thorugh the GitGub repository. If the pull request is approved, the sequence continues to the merge step.
- **Closing the issue:** Once the pull request is merged, the branch is deleted and the issue is closed. The process starts again, with the creation of a new issue.
## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License
During this project, we have learned about the importance of licensing and the ethics of open source developement. This has added new understanding and new perspectives to the reality surrounding the Chirp! application. For licensing, we haven chosen the MIT License. This choice is based on the simplicity of the license, as well as the its broad use and familiarity. Having this lisence means that others are free to use, modify and distribute our application, as long as the original license and copyright notice are included.

## LLMs, ChatGPT, CoPilot, and others
