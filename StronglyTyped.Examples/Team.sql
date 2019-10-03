CREATE TABLE public.team
(
	team_id SERIAL,
	name varchar(100) COLLATE pg_catalog."default" NOT NULL,
	CONSTRAINT team_pkey PRIMARY KEY (team_id)
) WITH ( OIDS = FALSE ) TABLESPACE pg_default;

ALTER TABLE public.team OWNER to example_dbuser;
GRANT ALL ON TABLE public.team TO example_dbuser;
