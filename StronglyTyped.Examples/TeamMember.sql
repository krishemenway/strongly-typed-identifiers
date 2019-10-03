CREATE TABLE public.team_member
(
	team_member_id BIGSERIAL NOT NULL,
	team_id integer NOT NULL,
	person_id uuid NOT NULL,

	CONSTRAINT team_member_team_id_fkey FOREIGN KEY (team_id) REFERENCES public.team (team_id) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT team_member_person_id_fkey FOREIGN KEY (person_id) REFERENCES public.person (person_id) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION,

	CONSTRAINT team_member_pkey PRIMARY KEY (team_member_id)
) WITH ( OIDS = FALSE ) TABLESPACE pg_default;

ALTER TABLE public.team_member OWNER to example_dbuser;
GRANT ALL ON TABLE public.team_member TO example_dbuser;
