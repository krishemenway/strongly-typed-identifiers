CREATE TABLE public.person
(
	person_id uuid NOT NULL DEFAULT uuid_generate_v1(),
	first_name varchar(100) COLLATE pg_catalog."default" NOT NULL,
	last_name varchar(100) COLLATE pg_catalog."default" NOT NULL,
	CONSTRAINT person_pkey PRIMARY KEY (person_id)
) WITH ( OIDS = FALSE ) TABLESPACE pg_default;

ALTER TABLE public.person OWNER to example_dbuser;
GRANT ALL ON TABLE public.person TO example_dbuser;
