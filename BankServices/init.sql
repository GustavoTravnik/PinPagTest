CREATE TABLE IF NOT EXISTS public."ClientAccount"
(
    "Id" uuid NOT NULL DEFAULT gen_random_uuid(),
    "Name" character varying NOT NULL,
    "Document" character varying(14) NOT NULL UNIQUE,
    "Amount" numeric(15, 2) NULL DEFAULT 0,
    CONSTRAINT "ClientAccount_PK" PRIMARY KEY ("Id"),
    CONSTRAINT "ClientAccount_NonNegativeAmount" CHECK ("Amount" >= 0)
);

ALTER TABLE IF EXISTS public."ClientAccount"
    OWNER to postgres;

COMMENT ON TABLE public."ClientAccount"
    IS 'Default client account database, containing only the main fields.';

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'transactiontypes') THEN
        CREATE TYPE TransactionTypes AS ENUM ('in', 'out');
    END IF;
END $$;

CREATE TABLE IF NOT EXISTS public."BankTransactions"
(
    "Id" uuid NOT NULL DEFAULT gen_random_uuid(),
	"ClientAccountId" uuid NOT NULL ,
    "TransactionType" TransactionTypes NOT NULL,
    "OccurredAt" TIMESTAMPTZ NULL DEFAULT NOW(),
    "Amount" numeric(15, 2) NULL DEFAULT 0,
    CONSTRAINT "BankTransactions_PK" PRIMARY KEY ("Id"),
	CONSTRAINT "BankTransactions_ClientAccount_FK" FOREIGN KEY ("ClientAccountId") REFERENCES public."ClientAccount" ("Id")
);