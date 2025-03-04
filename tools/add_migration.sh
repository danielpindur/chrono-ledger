#!/bin/bash

# Script to add a new EF Core migration
# Usage: ./add_migration.sh <MigrationName>

# Check if the migration name is provided
if [ "$#" -ne 1 ]; then
    echo "Error: You must provide a migration name."
    echo "Usage: $0 <MigrationName>"
    exit 1
fi

MIGRATION_NAME=$1

# Run the command to add the migration
dotnet ef migrations add "$MIGRATION_NAME" --project ChronoLeger.SchemaSync --startup-project ../ChronoLeger

# Check if the command was successful
if [ $? -eq 0 ]; then
    echo "Migration '$MIGRATION_NAME' added successfully."
else
    echo "Error: Failed to add migration '$MIGRATION_NAME'."
    exit 1
fi
