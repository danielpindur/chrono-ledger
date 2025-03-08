#!/bin/bash

# Script to update the database with the latest EF Core migrations
# Usage: ./apply_migrations.sh

# Run the command to update the database
dotnet ef database update --project src/ChronoLedger.SchemaSync/ChronoLedger.SchemaSync.csproj --startup-project src/ChronoLedger.SchemaSync/ChronoLedger.SchemaSync.csproj --context ChronoLedger.SchemaSync.ChronoLedgerDbContext --verbose

# Check if the command was successful
if [ $? -eq 0 ]; then
    echo "Database updated successfully."
else
    echo "Error: Failed to update the database."
    exit 1
fi
