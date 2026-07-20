#!/usr/bin/env bash
set -e

echo "========================================="
echo "🔄 Starting database restore..."
echo "========================================="

if [ ! -f /docker-entrypoint-initdb.d/01-init.backup ]; then
    echo "⚠️ Backup file not found!"
    exit 0
fi

pg_restore -U "$POSTGRES_USER" -d "$POSTGRES_DB" --verbose --single-transaction /docker-entrypoint-initdb.d/01-init.backup

echo "✅ Database restored successfully!"
echo "========================================="