﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;
CREATE TABLE users (
    user_id uuid NOT NULL DEFAULT (uuid_generate_v4()),
    external_user_id character varying(50) NOT NULL,
    created_date_time timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    CONSTRAINT pk_users PRIMARY KEY (user_id)
);

CREATE TABLE journal_batches (
    journal_batch_id bigint GENERATED BY DEFAULT AS IDENTITY,
    created_date_time timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    created_by_user_id uuid NOT NULL,
    journals_json json NOT NULL,
    CONSTRAINT pk_journal_batches PRIMARY KEY (journal_batch_id),
    CONSTRAINT fk_journal_batches_users_created_by_user_id FOREIGN KEY (created_by_user_id) REFERENCES users (user_id) ON DELETE RESTRICT
);

CREATE UNIQUE INDEX ix_users_external_user_id ON users (external_user_id);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20250311194156_JournalBatch', '9.0.2');

COMMIT;

