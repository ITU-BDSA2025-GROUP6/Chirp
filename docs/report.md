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

<img width="800" height="880" alt="image" src="https://github.com/user-attachments/assets/4bfef960-0365-4948-9dba-3523d2b4dece" />

- **NOTE:** `IdentityUser` only shows the first part of the library it comes from in the diagram.  
  The full library path is:  
  `Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser`

- **NOTE:** While not shown in the diagram, all three classes connected to **Author**  
  (`Cheep`, `Recheep`, and `Follows`) use  
  `System.ComponentModel.DataAnnotations` to support `required` parameters, among others.


## Architecture — In the small

## Architecture of deployed application

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others
