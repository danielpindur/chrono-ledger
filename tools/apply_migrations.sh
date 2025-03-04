#!/bin/bash

# Script to update the database with the latest EF Core migrations
# Usage: ./apply_migrations.sh

# Run the command to update the database
dotnet ef database update --project ChronoLeger.SchemaSync --startup-project ../ChronoLeger

# Check if the command was successful
if [ $? -eq 0 ]; then
    echo "Database updated successfully."
else
    echo "Error: Failed to update the database."
    exit 1
fi
