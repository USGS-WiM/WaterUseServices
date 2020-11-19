CREATE TABLE "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "CategoryTypes" (
    "ID" serial NOT NULL,
    "Code" text NOT NULL,
    "Description" text,
    "Name" text NOT NULL,
    CONSTRAINT "PK_CategoryTypes" PRIMARY KEY ("ID"),
    CONSTRAINT "AK_CategoryTypes_Code" UNIQUE ("Code")
);

CREATE TABLE "Regions" (
    "ID" serial NOT NULL,
    "Description" text,
    "Name" text NOT NULL,
    "ShortName" text NOT NULL,
    CONSTRAINT "PK_Regions" PRIMARY KEY ("ID"),
    CONSTRAINT "AK_Regions_ShortName" UNIQUE ("ShortName")
);

CREATE TABLE "Roles" (
    "ID" serial NOT NULL,
    "Description" text NOT NULL,
    "Name" text NOT NULL,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("ID"),
    CONSTRAINT "AK_Roles_Name" UNIQUE ("Name")
);

CREATE TABLE "SourceTypes" (
    "ID" serial NOT NULL,
    "Code" text NOT NULL,
    "Description" text,
    "Name" text NOT NULL,
    CONSTRAINT "PK_SourceTypes" PRIMARY KEY ("ID"),
    CONSTRAINT "AK_SourceTypes_Code" UNIQUE ("Code")
);

CREATE TABLE "StatusTypes" (
    "ID" serial NOT NULL,
    "Code" text NOT NULL,
    "Description" text,
    "Name" text,
    CONSTRAINT "PK_StatusTypes" PRIMARY KEY ("ID"),
    CONSTRAINT "AK_StatusTypes_Code" UNIQUE ("Code")
);

CREATE TABLE "UnitTypes" (
    "ID" serial NOT NULL,
    "Abbreviation" text NOT NULL,
    "Name" text,
    CONSTRAINT "PK_UnitTypes" PRIMARY KEY ("ID"),
    CONSTRAINT "AK_UnitTypes_Abbreviation" UNIQUE ("Abbreviation")
);

CREATE TABLE "CategoryCoefficient" (
    "ID" serial NOT NULL,
    "CategoryTypeID" int4 NOT NULL,
    "Comments" text,
    "RegionID" int4 NOT NULL,
    "Value" float8 NOT NULL,
    CONSTRAINT "PK_CategoryCoefficient" PRIMARY KEY ("ID"),
    CONSTRAINT "FK_CategoryCoefficient_CategoryTypes_CategoryTypeID" FOREIGN KEY ("CategoryTypeID") REFERENCES "CategoryTypes" ("ID") ON DELETE CASCADE,
    CONSTRAINT "FK_CategoryCoefficient_Regions_RegionID" FOREIGN KEY ("RegionID") REFERENCES "Regions" ("ID") ON DELETE CASCADE
);

CREATE TABLE "Managers" (
    "ID" serial NOT NULL,
    "Email" text NOT NULL,
    "FirstName" text NOT NULL,
    "LastName" text NOT NULL,
    "OtherInfo" text,
    "Password" text NOT NULL,
    "PrimaryPhone" text,
    "RoleID" int4 NOT NULL,
    "SecondaryPhone" text,
    "Username" text NOT NULL,
    CONSTRAINT "PK_Managers" PRIMARY KEY ("ID"),
    CONSTRAINT "AK_Managers_Username" UNIQUE ("Username"),
    CONSTRAINT "FK_Managers_Roles_RoleID" FOREIGN KEY ("RoleID") REFERENCES "Roles" ("ID") ON DELETE CASCADE
);

CREATE TABLE "Sources" (
    "ID" serial NOT NULL,
    "CategoryTypeID" int4 NOT NULL,
    "FacilityName" text NOT NULL,
    "RegionID" int4 NOT NULL,
    "SourceTypeID" int4 NOT NULL,
    "StationID" text,
    CONSTRAINT "PK_Sources" PRIMARY KEY ("ID"),
    CONSTRAINT "FK_Sources_CategoryTypes_CategoryTypeID" FOREIGN KEY ("CategoryTypeID") REFERENCES "CategoryTypes" ("ID") ON DELETE CASCADE,
    CONSTRAINT "FK_Sources_Regions_RegionID" FOREIGN KEY ("RegionID") REFERENCES "Regions" ("ID") ON DELETE CASCADE,
    CONSTRAINT "FK_Sources_SourceTypes_SourceTypeID" FOREIGN KEY ("SourceTypeID") REFERENCES "SourceTypes" ("ID") ON DELETE CASCADE
);

CREATE TABLE "RegionManager" (
    "ManagerID" int4 NOT NULL,
    "RegionID" int4 NOT NULL,
    CONSTRAINT "PK_RegionManager" PRIMARY KEY ("ManagerID", "RegionID"),
    CONSTRAINT "FK_RegionManager_Managers_ManagerID" FOREIGN KEY ("ManagerID") REFERENCES "Managers" ("ID") ON DELETE CASCADE,
    CONSTRAINT "FK_RegionManager_Regions_RegionID" FOREIGN KEY ("RegionID") REFERENCES "Regions" ("ID") ON DELETE CASCADE
);

CREATE TABLE "Permits" (
    "ID" serial NOT NULL,
    "EndDate" timestamp NOT NULL,
    "IntakeCapacity" float8 NOT NULL,
    "SourceID" int4 NOT NULL,
    "StartDate" timestamp NOT NULL,
    "StatusTypeID" int4 NOT NULL,
    "UnitTypeID" int4 NOT NULL,
    "WellCapacity" float8 NOT NULL,
    CONSTRAINT "PK_Permits" PRIMARY KEY ("ID"),
    CONSTRAINT "FK_Permits_Sources_SourceID" FOREIGN KEY ("SourceID") REFERENCES "Sources" ("ID") ON DELETE CASCADE,
    CONSTRAINT "FK_Permits_StatusTypes_StatusTypeID" FOREIGN KEY ("StatusTypeID") REFERENCES "StatusTypes" ("ID") ON DELETE CASCADE,
    CONSTRAINT "FK_Permits_UnitTypes_UnitTypeID" FOREIGN KEY ("UnitTypeID") REFERENCES "UnitTypes" ("ID") ON DELETE CASCADE
);

CREATE TABLE "TimeSeries" (
    "ID" serial NOT NULL,
    "Date" timestamp NOT NULL,
    "SourceID" int4 NOT NULL,
    "UnitTypeID" int4 NOT NULL,
    "Value" float8 NOT NULL,
    CONSTRAINT "PK_TimeSeries" PRIMARY KEY ("ID"),
    CONSTRAINT "FK_TimeSeries_Sources_SourceID" FOREIGN KEY ("SourceID") REFERENCES "Sources" ("ID") ON DELETE CASCADE,
    CONSTRAINT "FK_TimeSeries_UnitTypes_UnitTypeID" FOREIGN KEY ("UnitTypeID") REFERENCES "UnitTypes" ("ID") ON DELETE CASCADE
);

CREATE INDEX "IX_CategoryCoefficient_CategoryTypeID" ON "CategoryCoefficient" ("CategoryTypeID");

CREATE INDEX "IX_CategoryCoefficient_RegionID" ON "CategoryCoefficient" ("RegionID");

CREATE INDEX "IX_Managers_RoleID" ON "Managers" ("RoleID");

CREATE INDEX "IX_Permits_SourceID" ON "Permits" ("SourceID");

CREATE INDEX "IX_Permits_StatusTypeID" ON "Permits" ("StatusTypeID");

CREATE INDEX "IX_Permits_UnitTypeID" ON "Permits" ("UnitTypeID");

CREATE INDEX "IX_RegionManager_RegionID" ON "RegionManager" ("RegionID");

CREATE INDEX "IX_Sources_CategoryTypeID" ON "Sources" ("CategoryTypeID");

CREATE INDEX "IX_Sources_RegionID" ON "Sources" ("RegionID");

CREATE INDEX "IX_Sources_SourceTypeID" ON "Sources" ("SourceTypeID");

CREATE INDEX "IX_TimeSeries_SourceID" ON "TimeSeries" ("SourceID");

CREATE INDEX "IX_TimeSeries_UnitTypeID" ON "TimeSeries" ("UnitTypeID");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20170324162120_init', '1.1.1');

