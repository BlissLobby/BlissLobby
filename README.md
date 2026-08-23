# BlissLobby
Visitor Management System for office spaces. Can also be used for residential spaces

# Actors
* SuperAdmin
* ClusterAdmin
* Resident / Employee
* Visitor
* Lobby Personnel

# Functional requirements
* User/actor authentication
* Pre-approval of visits by residents
* Visitor check-in based on pre-approval
* Visitor check-in based on instant resident approval
* Visitor check-in based on lobby personnel approval
* Visitor check-out by lobby personnel
* Reports for visitor activity analysis

# Non-functional requirements
* Self-hostable on-premise and on cloud
* No security vulnerabilities in the source code, frameworks, platforms used
* Encrypt data at rest and in-transit
* Source code based on Clean architecture and SOLID principles
* No sensitive data like api keys, passwords to be checked into source control
* Easy to use
* Encourage two-factor authentication
* Permissions/Privileges provided to User/Actor roles should follow principle of least privilege
* Support OAuth 2.0 providers, Active Directory for authentication
* Auditability and reconciliation capabilities. Logs should contain user identifiers and device identifiers wherever possible.
* Data Security
  * Notify resident if his visitor information is viewed by other application users
  * Option to purge visit data after 180 days
  * Show only resident name to other users to avoid exposure of personal information
  * Option to store only visitor face image/biometric data hash to avoid storing PII
  * Option for application admin (data security officer) to remove visitor's personal information and visit information
  * Option to take vistor consent (via physical signature or email) to store PII data and visit data
  * GDPR compliance
* User management auditability
  * Log all user management actions (create, update, delete)
  * Move deleted users to an archive table and scrub sensitive information if not required
  * Each user edit log should contain fields that were changed, old value, new value, who changed it and when
* Observability (dashboards and alarms) for application health and performance
* Support horizontal scalability (provide options for distributed cache)
* Support multi-tenancy after the application is matured for single tenant setup

# Interfaces
* Email
  * Application activity notifications
  * Visitor approvals notifications
  * User account management notifications
* Web application
  * Dashboards
  * Notifications
  * Approvals
  * User management
  * Reports viewing and management
  * Visitor invite CRUD
  * Visitor check-in resident approval
  * Visitor check-in via lobby personnel
  * Visitor check-in via pre-approved invite
  * Visitor check-out by lobby personnel
 
# UI screens

## User management
* Users Create, View, Update (password, email, username, phone, disable), Delete pages - actors: SuperAdmin, ClusterAdmin
* Cluster Admin should be able to view and manage users only in his cluster. SuperAdmin should be able to view and manage all users.

## Buildings management
* Buildings CRUD - actors: SuperAdmin
* Clusters CRUD - actors: SuperAdmin
* Add/remove Building from Cluster - actors: Superadmin
* Add/remove Clusters to users with ClusterAdmin roles - actors: Superadmin
* Add/remove residents to building - actors: SuperAdmin, ClusterAdmin

## User self service
* User forgot password page - actor: AppUser
* User login page - actor: AppUser
* Email update page - actor: AppUser
* Password change page - actor: AppUser
* Mobile number update page - actor: AppUser

## Invite management
* Invite generation page - actors: Resident, SuperAdmin, ClusterAdmin
* Invite listing page - actors: Resident, SuperAdmin, ClusterAdmin
* Invite editing page - actors: Resident
* Invite cancellation page - actors: Resident
* Invite checked-in success display page - actors: Resident, SuperAdmin, ClusterAdmin

## Visit Check-in / Check-out
* Instant visit check-in approval by resident page - actors: Resident
* Instant visit check-in approval by lobby personnel page - actors: LobbyPersonnel
* Invite based visit check-in page - actors: LobbyPersonnel
* Visitor check-out page - actors: LobbyPersonnel

## Reports
* My Visitors page- actor: AppUser
* Resident Visitors page - actor: SuperAdmin, ClusterAdmin
* All Visitors page - actor: SuperAdmin, ClusterAdmin
 
# Technology stack 
* Dotnet web application as backend system
* HTTP as application communication protocol
* PostgreSQL for database in production environment 
* SQLite for database in testing environment 
* Entity Framework for ORM
* Identity framework for user authentication, authorization, session management, cookies, account management
* Cookies for session persistence in browser
* Blazor SSR for server side web page rendering
* Blazor Web assembly for in browser UI interactivity
* Controllers for API endpoints
* Mediatr for application commands and queries handling
* Fluent validation for request validation
* XUnit for application testing
* Docker for application packaging
* GitHub actions for CI CD

# Entities and relationships

## AuditableEntity
* Description: Base class for all entities in the system that require auditing information. Contains common fields for tracking creation and modification details.
### Fields
* Id (Guid, PK)
* CreatedAt (DateTimeOffset)
* CreatedBy (Guid, FK to Users table)
* LastModifiedAt (DateTimeOffset)
* LastModifiedBy (Guid, FK to Users table)

## User
* Table: Users
* Description: Represents a user of the application, which can be a SuperAdmin, ClusterAdmin, Resident, or LobbyPersonnel. Contains authentication and contact information, as well as role and association with clusters and buildings.
* Relationships:
  * A User can be associated with zero or one Cluster (ClusterId)
  * A User can be associated with zero or one Building (BuildingId)
* A User can have one Role (SuperAdmin, ClusterAdmin, Resident, LobbyPersonnel)
* User entity will be extended from IdentityUser<Guid> class provided by ASP.NET Identity framework to leverage built-in authentication and authorization features. This will also provide additional fields like SecurityStamp, ConcurrencyStamp, etc. for enhanced security and concurrency handling.

### Fields
* Id (Guid, PK)
* Username (string, unique)
* Email (string, unique)
* PasswordHash (string)
* PhoneNumber (string)
* IsDisabled (bool)
* DisplayName (string)
* Role (enum: SuperAdmin, ClusterAdmin, Resident, LobbyPersonnel)
* BuildingId (Guid, FK to Buildings table, nullable)

## Building
* Table: Buildings
* Description: Represents a building (physical premises) within the system. A building can be part of a cluster and can have multiple residents and lobby personnel associated with it.
* Relationships:
  * A Building can belong to zero or one Cluster (ClusterId)
  * A Building can have multiple Users (residents and lobby personnel) associated with it

### Fields
* Id (Guid, PK)
* Name (string, unique)
* Address (string)
* ClusterId (Guid, FK to Clusters table, nullable)

## Cluster
* Table: Clusters
* Description: Represents a cluster (group of buildings) within the system. A cluster can have multiple buildings and multiple users (ClusterAdmins) associated with it.
* Relationships:
  * A Cluster can have multiple Buildings associated with it
  * A Cluster can have multiple Users (ClusterAdmins) associated with it
### Fields
* Id (Guid, PK)
* Name (string, unique)
* Description (string)

## ClusterAdmin
* Table: ClusterAdmins
* Description: Represents the association between a ClusterAdmin user and a Cluster. This entity is used to manage many-to-many relationships between ClusterAdmins and Clusters, as a ClusterAdmin can manage multiple Clusters and a Cluster can have multiple ClusterAdmins.
* Relationships:
  * A ClusterAdmin can be associated with one Cluster (ClusterId)
  * A ClusterAdmin can be associated with one User (UserId) who has the ClusterAdmin role
### Fields
* Id (Guid, PK)
* UserId (Guid, FK to Users table)
* ClusterId (Guid, FK to Clusters table)

## Invite
* Table: Invites
* Description: Represents an invitation for a visitor to visit a resident. Contains details about the visitor, the resident they are visiting, the scheduled visit time, and the status of the invite.
* Relationships:
  * An Invite is associated with one Resident (ResidentId)
  * An Invite can have one Visit (VisitId) if the visitor has checked in
### Fields
* Id (Guid, PK)
* VisitorName (string)
* VisitorEmail (string)
* VisitorPhoneNumber (string)
* ResidentId (Guid, FK to Users table where Role is Resident)
* ScheduledVisitTime (DateTimeOffset)
* Status (enum: Pending, Approved, Rejected, Cancelled)
* VisitId (Guid, FK to Visits table, nullable)

## Visit
* Table: Visits
* Description: Represents a visit by a visitor to a resident. Contains details about the visitor, the resident they are visiting, the check-in and check-out times, and the status of the visit.
* Relationships:
  * A Visit is associated with one Resident (ResidentId)
  * A Visit can be associated with one Invite (InviteId) if the visit was based on a pre-approved invite
  * A Visit can be checked in by one LobbyPersonnel (CheckedInBy) and checked out by one LobbyPersonnel (CheckedOutBy)
  * A Visit can have multiple Notifications associated with it
### Fields
* Id (Guid, PK)
* VisitorName (string)
* VisitorEmail (string)
* VisitorPhoneNumber (string)
* ResidentId (Guid, FK to Users table where Role is Resident)
* CheckInTime (DateTimeOffset, nullable)
* CheckOutTime (DateTimeOffset, nullable)
* Status (enum: Pending, CheckedIn, CheckedOut, Cancelled)
* InviteId (Guid, FK to Invites table, nullable)
* CheckedInBy (Guid, FK to Users table where Role is LobbyPersonnel, nullable)
* CheckedOutBy (Guid, FK to Users table where Role is LobbyPersonnel, nullable)
* Notifications (collection of Notification entities)
* Note: VisitorName, VisitorEmail, and VisitorPhoneNumber fields are stored in the Visit entity to capture the visitor's information at the time of the visit, even if the invite was based on a pre-approved invite. This allows for accurate record-keeping and reporting of visits, as well as providing necessary information for notifications and audits.

## Notification
* Table: Notifications
* Description: Represents a notification related to a visit. Contains details about the type of notification, the recipient user, the associated visit, and whether the notification has been read.
* Relationships:
  * A Notification is associated with one Visit (VisitId)
  * A Notification is associated with one User (RecipientId) who receives the notification
### Fields
* Id (Guid, PK)
* VisitId (Guid, FK to Visits table)
* RecipientId (Guid, FK to Users table)
* Type (enum: VisitCheckIn, VisitCheckOut, InviteApproval, InviteRejection, etc.)
* Message (string)
* IsRead (bool)
* CreatedAt (DateTimeOffset)
* Note: The Notification entity is designed to store notifications related to visits, such as visit check-ins, check-outs, invite approvals, and rejections. The RecipientId field allows for efficient querying of notifications for a specific user, while the VisitId field allows for tracking notifications related to a specific visit. The IsRead field can be used to filter unread notifications for users, and the CreatedAt field can be used for sorting and retention purposes. A retention plan can be implemented to automatically delete notifications after a certain period (e.g., 365 days) to manage storage and comply with data retention policies.

# TODOs
* Create logout screen
* Ensure Roles are created during db initialization, also fix the db initialization strategy to avoid data loss during development
* Define UI screens
* Define project scope exclusions to avoid scope creep
* Define application processes as user stories
* Define how to store notifications for efficient querying and also define retention plan
* Mention usage of blazor datatables to render paginated lists

## References
* blazor ssr forms - https://youtu.be/SwmDg4Rmq3U?si=IMH0l6Wcpk5otgza
* fluent validation in blazor forms with blazilla - https://docs.fluentvalidation.net/en/latest/blazor.html , https://blazor.loresoft.com/
* CSRF mitigation with Antiforgery token in blazor EditForm component - https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/?view=aspnetcore-10.0#antiforgery-support
* XSS mitigated by default in blazor - https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/?view=aspnetcore-10.0#antiforgery-support
* Radzen blazor starter - https://blazor.radzen.com/get-started , https://github.com/radzenhq/radzen-blazor/blob/master/RadzenBlazorDemos.Host/Program.cs
* Pagination with EF Core - https://codewithmukesh.com/blog/pagination-sorting-searching-aspnet-core-webapi/
