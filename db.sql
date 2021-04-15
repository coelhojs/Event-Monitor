CREATE TABLE IF NOT EXISTS "EVENT" (
    "ID" DEFAULT NOT NULL DEFAULT nextval('table_name_id_seq')
    "REGION" varchar NOT NULL,
    "SENSOR" varchar NOT NULL,
    "TIMESTAMP" timestamp(0) NOT NULL,
    "VALUE" varchar NOT NULL,
    CONSTRAINT event_pk PRIMARY KEY ("REGION", "SENSOR", "TIMESTAMP", "VALUE")
);