CREATE TABLE IF NOT EXISTS "EVENT" (
    "ID" serial NOT NULL,
    "REGION" varchar NOT NULL,
    "SENSOR" varchar NOT NULL,
    "TIMESTAMP" timestamp(0) NOT NULL,
    "VALUE" varchar NOT NULL,
    CONSTRAINT event_pk PRIMARY KEY ("REGION", "SENSOR", "TIMESTAMP", "VALUE")
);