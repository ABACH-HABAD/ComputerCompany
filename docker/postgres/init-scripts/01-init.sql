--
-- PostgreSQL database dump
--

\restrict 4Cl9mh5MFviYISH0toIUjXv9GNdROcp0NozHWoDlO4bP8OIcKCeHFkcnec6hHJV

-- Dumped from database version 18.4 (Debian 18.4-1.pgdg13+1)
-- Dumped by pg_dump version 18.4 (Debian 18.4-1.pgdg13+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

ALTER TABLE IF EXISTS ONLY public."Sessions" DROP CONSTRAINT IF EXISTS "FK_Sessions_Accounts_AccountId";
ALTER TABLE IF EXISTS ONLY public."Reviews" DROP CONSTRAINT IF EXISTS "FK_Reviews_Accounts_SenderId";
ALTER TABLE IF EXISTS ONLY public."Assemblies" DROP CONSTRAINT IF EXISTS "FK_Assemblies_Storages_StorageId";
ALTER TABLE IF EXISTS ONLY public."Assemblies" DROP CONSTRAINT IF EXISTS "FK_Assemblies_PowerUnits_PowerUnitId";
ALTER TABLE IF EXISTS ONLY public."Assemblies" DROP CONSTRAINT IF EXISTS "FK_Assemblies_Motherboards_MotherboardId";
ALTER TABLE IF EXISTS ONLY public."Assemblies" DROP CONSTRAINT IF EXISTS "FK_Assemblies_Memories_MemoryId";
ALTER TABLE IF EXISTS ONLY public."Assemblies" DROP CONSTRAINT IF EXISTS "FK_Assemblies_Gpus_GpuId";
ALTER TABLE IF EXISTS ONLY public."Assemblies" DROP CONSTRAINT IF EXISTS "FK_Assemblies_Frames_FrameId";
ALTER TABLE IF EXISTS ONLY public."Assemblies" DROP CONSTRAINT IF EXISTS "FK_Assemblies_Cpus_CpuId";
ALTER TABLE IF EXISTS ONLY public."Assemblies" DROP CONSTRAINT IF EXISTS "FK_Assemblies_Accounts_AccountId";
ALTER TABLE IF EXISTS ONLY public."Accounts" DROP CONSTRAINT IF EXISTS "FK_Accounts_Roles_RoleId";
DROP INDEX IF EXISTS public."IX_Storages_Name";
DROP INDEX IF EXISTS public."IX_Storages_Model";
DROP INDEX IF EXISTS public."IX_Storages_Id";
DROP INDEX IF EXISTS public."IX_Sessions_Refresh";
DROP INDEX IF EXISTS public."IX_Sessions_Id";
DROP INDEX IF EXISTS public."IX_Sessions_AccountId";
DROP INDEX IF EXISTS public."IX_Roles_Name";
DROP INDEX IF EXISTS public."IX_Roles_Id";
DROP INDEX IF EXISTS public."IX_Reviews_SenderId";
DROP INDEX IF EXISTS public."IX_PowerUnits_Name";
DROP INDEX IF EXISTS public."IX_PowerUnits_Model";
DROP INDEX IF EXISTS public."IX_PowerUnits_Id";
DROP INDEX IF EXISTS public."IX_Motherboards_Name";
DROP INDEX IF EXISTS public."IX_Motherboards_Model";
DROP INDEX IF EXISTS public."IX_Motherboards_Id";
DROP INDEX IF EXISTS public."IX_Memories_Name";
DROP INDEX IF EXISTS public."IX_Memories_Model";
DROP INDEX IF EXISTS public."IX_Memories_Id";
DROP INDEX IF EXISTS public."IX_Gpus_Name";
DROP INDEX IF EXISTS public."IX_Gpus_Model";
DROP INDEX IF EXISTS public."IX_Gpus_Id";
DROP INDEX IF EXISTS public."IX_Frames_Name";
DROP INDEX IF EXISTS public."IX_Frames_Model";
DROP INDEX IF EXISTS public."IX_Frames_Id";
DROP INDEX IF EXISTS public."IX_Cpus_Name";
DROP INDEX IF EXISTS public."IX_Cpus_Model";
DROP INDEX IF EXISTS public."IX_Cpus_Id";
DROP INDEX IF EXISTS public."IX_Assemblies_StorageId";
DROP INDEX IF EXISTS public."IX_Assemblies_PowerUnitId";
DROP INDEX IF EXISTS public."IX_Assemblies_MotherboardId";
DROP INDEX IF EXISTS public."IX_Assemblies_MemoryId";
DROP INDEX IF EXISTS public."IX_Assemblies_Id";
DROP INDEX IF EXISTS public."IX_Assemblies_GpuId";
DROP INDEX IF EXISTS public."IX_Assemblies_FrameId";
DROP INDEX IF EXISTS public."IX_Assemblies_CpuId";
DROP INDEX IF EXISTS public."IX_Assemblies_AccountId";
DROP INDEX IF EXISTS public."IX_Accounts_RoleId";
DROP INDEX IF EXISTS public."IX_Accounts_Login";
DROP INDEX IF EXISTS public."IX_Accounts_Id";
ALTER TABLE IF EXISTS ONLY public."Storages" DROP CONSTRAINT IF EXISTS "PK_Storages";
ALTER TABLE IF EXISTS ONLY public."Sessions" DROP CONSTRAINT IF EXISTS "PK_Sessions";
ALTER TABLE IF EXISTS ONLY public."Roles" DROP CONSTRAINT IF EXISTS "PK_Roles";
ALTER TABLE IF EXISTS ONLY public."Reviews" DROP CONSTRAINT IF EXISTS "PK_Reviews";
ALTER TABLE IF EXISTS ONLY public."PowerUnits" DROP CONSTRAINT IF EXISTS "PK_PowerUnits";
ALTER TABLE IF EXISTS ONLY public."Motherboards" DROP CONSTRAINT IF EXISTS "PK_Motherboards";
ALTER TABLE IF EXISTS ONLY public."Memories" DROP CONSTRAINT IF EXISTS "PK_Memories";
ALTER TABLE IF EXISTS ONLY public."Gpus" DROP CONSTRAINT IF EXISTS "PK_Gpus";
ALTER TABLE IF EXISTS ONLY public."Frames" DROP CONSTRAINT IF EXISTS "PK_Frames";
ALTER TABLE IF EXISTS ONLY public."Cpus" DROP CONSTRAINT IF EXISTS "PK_Cpus";
ALTER TABLE IF EXISTS ONLY public."Assemblies" DROP CONSTRAINT IF EXISTS "PK_Assemblies";
ALTER TABLE IF EXISTS ONLY public."Accounts" DROP CONSTRAINT IF EXISTS "PK_Accounts";
DROP TABLE IF EXISTS public."Storages";
DROP TABLE IF EXISTS public."Sessions";
DROP TABLE IF EXISTS public."Roles";
DROP TABLE IF EXISTS public."Reviews";
DROP TABLE IF EXISTS public."PowerUnits";
DROP TABLE IF EXISTS public."Motherboards";
DROP TABLE IF EXISTS public."Memories";
DROP TABLE IF EXISTS public."Gpus";
DROP TABLE IF EXISTS public."Frames";
DROP TABLE IF EXISTS public."Cpus";
DROP TABLE IF EXISTS public."Assemblies";
DROP TABLE IF EXISTS public."Accounts";
-- *not* dropping schema, since initdb creates it
--
-- Name: public; Type: SCHEMA; Schema: -; Owner: -
--

-- *not* creating schema, since initdb creates it


--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA public IS '';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Accounts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Accounts" (
    "Id" uuid NOT NULL,
    "Login" character varying(100) NOT NULL,
    "Password" character varying(100) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "RoleId" uuid NOT NULL
);


--
-- Name: Assemblies; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Assemblies" (
    "Id" uuid NOT NULL,
    "UsedMemoryCount" integer NOT NULL,
    "AccountId" uuid NOT NULL,
    "CpuId" uuid NOT NULL,
    "MotherboardId" uuid NOT NULL,
    "MemoryId" uuid NOT NULL,
    "PowerUnitId" uuid NOT NULL,
    "StorageId" uuid NOT NULL,
    "FrameId" uuid NOT NULL,
    "GpuId" uuid NOT NULL
);


--
-- Name: Cpus; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Cpus" (
    "Id" uuid NOT NULL,
    "Model" character varying(100) NOT NULL,
    "Socket" character varying(10) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Price" double precision NOT NULL,
    "Count" integer NOT NULL
);


--
-- Name: Frames; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Frames" (
    "Id" uuid NOT NULL,
    "Model" character varying(100) NOT NULL,
    "FormFactor" character varying(10) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Price" double precision NOT NULL,
    "Count" integer NOT NULL
);


--
-- Name: Gpus; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Gpus" (
    "Id" uuid NOT NULL,
    "ModelCore" character varying(30) NOT NULL,
    "Model" character varying(100) NOT NULL,
    "VideoMemory" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Price" double precision NOT NULL,
    "Count" integer NOT NULL
);


--
-- Name: Memories; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Memories" (
    "Id" uuid NOT NULL,
    "Model" character varying(100) NOT NULL,
    "Size" integer NOT NULL,
    "Type" character varying(5) NOT NULL,
    "Frequency" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Price" double precision NOT NULL,
    "Count" integer NOT NULL
);


--
-- Name: Motherboards; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Motherboards" (
    "Id" uuid NOT NULL,
    "Model" character varying(100) NOT NULL,
    "Socket" character varying(10) NOT NULL,
    "Chipset" character varying(10) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Price" double precision NOT NULL,
    "Count" integer NOT NULL
);


--
-- Name: PowerUnits; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."PowerUnits" (
    "Id" uuid NOT NULL,
    "Model" character varying(100) NOT NULL,
    "FormFactor" character varying(10) NOT NULL,
    "Certification" character varying(20) NOT NULL,
    "Power" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Price" double precision NOT NULL,
    "Count" integer NOT NULL
);


--
-- Name: Reviews; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Reviews" (
    "Id" uuid NOT NULL,
    "Message" text NOT NULL,
    "Stars" integer NOT NULL,
    "SenderId" uuid NOT NULL
);


--
-- Name: Roles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Roles" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL
);


--
-- Name: Sessions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Sessions" (
    "Id" uuid NOT NULL,
    "Refresh" text NOT NULL,
    "Ip" text NOT NULL,
    "AccountId" uuid NOT NULL
);


--
-- Name: Storages; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Storages" (
    "Id" uuid NOT NULL,
    "Model" character varying(100) NOT NULL,
    "Type" character varying(10) NOT NULL,
    "FormFactor" character varying(10) NOT NULL,
    "Size" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Price" double precision NOT NULL,
    "Count" integer NOT NULL
);


--
-- Data for Name: Accounts; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Accounts" VALUES ('019f794d-c816-7b8a-b020-dc69f38f702d', 'admin@computercompany.com', 'nIx6CvUCnXy1JHq7UV7z4burruX8gv/alVpj4bYbqwo=', 'Админ', '4a93cc32-358b-4184-98b9-af3b12a55e52');


--
-- Data for Name: Assemblies; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: Cpus; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: Frames; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: Gpus; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: Memories; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: Motherboards; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: PowerUnits; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: Reviews; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: Roles; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Roles" VALUES ('4cc2534f-c52a-46ef-85cc-eedcd3bc5246', 'User');
INSERT INTO public."Roles" VALUES ('3b36f69f-c37e-40ab-a70f-0ecde3b0dc35', 'Employee');
INSERT INTO public."Roles" VALUES ('a81d5582-ab0c-42c3-9c23-70d774a22d22', 'Cashier');
INSERT INTO public."Roles" VALUES ('80fa76b3-bdd1-4428-9b87-9fddf752dbe8', 'Manager');
INSERT INTO public."Roles" VALUES ('4a93cc32-358b-4184-98b9-af3b12a55e52', 'Admin');


--
-- Data for Name: Sessions; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: Storages; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Name: Accounts PK_Accounts; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Accounts"
    ADD CONSTRAINT "PK_Accounts" PRIMARY KEY ("Id");


--
-- Name: Assemblies PK_Assemblies; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Assemblies"
    ADD CONSTRAINT "PK_Assemblies" PRIMARY KEY ("Id");


--
-- Name: Cpus PK_Cpus; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Cpus"
    ADD CONSTRAINT "PK_Cpus" PRIMARY KEY ("Id");


--
-- Name: Frames PK_Frames; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Frames"
    ADD CONSTRAINT "PK_Frames" PRIMARY KEY ("Id");


--
-- Name: Gpus PK_Gpus; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Gpus"
    ADD CONSTRAINT "PK_Gpus" PRIMARY KEY ("Id");


--
-- Name: Memories PK_Memories; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Memories"
    ADD CONSTRAINT "PK_Memories" PRIMARY KEY ("Id");


--
-- Name: Motherboards PK_Motherboards; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Motherboards"
    ADD CONSTRAINT "PK_Motherboards" PRIMARY KEY ("Id");


--
-- Name: PowerUnits PK_PowerUnits; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."PowerUnits"
    ADD CONSTRAINT "PK_PowerUnits" PRIMARY KEY ("Id");


--
-- Name: Reviews PK_Reviews; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Reviews"
    ADD CONSTRAINT "PK_Reviews" PRIMARY KEY ("Id");


--
-- Name: Roles PK_Roles; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Roles"
    ADD CONSTRAINT "PK_Roles" PRIMARY KEY ("Id");


--
-- Name: Sessions PK_Sessions; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Sessions"
    ADD CONSTRAINT "PK_Sessions" PRIMARY KEY ("Id");


--
-- Name: Storages PK_Storages; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Storages"
    ADD CONSTRAINT "PK_Storages" PRIMARY KEY ("Id");


--
-- Name: IX_Accounts_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Accounts_Id" ON public."Accounts" USING btree ("Id");


--
-- Name: IX_Accounts_Login; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Accounts_Login" ON public."Accounts" USING btree ("Login");


--
-- Name: IX_Accounts_RoleId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Accounts_RoleId" ON public."Accounts" USING btree ("RoleId");


--
-- Name: IX_Assemblies_AccountId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Assemblies_AccountId" ON public."Assemblies" USING btree ("AccountId");


--
-- Name: IX_Assemblies_CpuId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Assemblies_CpuId" ON public."Assemblies" USING btree ("CpuId");


--
-- Name: IX_Assemblies_FrameId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Assemblies_FrameId" ON public."Assemblies" USING btree ("FrameId");


--
-- Name: IX_Assemblies_GpuId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Assemblies_GpuId" ON public."Assemblies" USING btree ("GpuId");


--
-- Name: IX_Assemblies_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Assemblies_Id" ON public."Assemblies" USING btree ("Id");


--
-- Name: IX_Assemblies_MemoryId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Assemblies_MemoryId" ON public."Assemblies" USING btree ("MemoryId");


--
-- Name: IX_Assemblies_MotherboardId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Assemblies_MotherboardId" ON public."Assemblies" USING btree ("MotherboardId");


--
-- Name: IX_Assemblies_PowerUnitId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Assemblies_PowerUnitId" ON public."Assemblies" USING btree ("PowerUnitId");


--
-- Name: IX_Assemblies_StorageId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Assemblies_StorageId" ON public."Assemblies" USING btree ("StorageId");


--
-- Name: IX_Cpus_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Cpus_Id" ON public."Cpus" USING btree ("Id");


--
-- Name: IX_Cpus_Model; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Cpus_Model" ON public."Cpus" USING btree ("Model");


--
-- Name: IX_Cpus_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Cpus_Name" ON public."Cpus" USING btree ("Name");


--
-- Name: IX_Frames_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Frames_Id" ON public."Frames" USING btree ("Id");


--
-- Name: IX_Frames_Model; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Frames_Model" ON public."Frames" USING btree ("Model");


--
-- Name: IX_Frames_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Frames_Name" ON public."Frames" USING btree ("Name");


--
-- Name: IX_Gpus_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Gpus_Id" ON public."Gpus" USING btree ("Id");


--
-- Name: IX_Gpus_Model; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Gpus_Model" ON public."Gpus" USING btree ("Model");


--
-- Name: IX_Gpus_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Gpus_Name" ON public."Gpus" USING btree ("Name");


--
-- Name: IX_Memories_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Memories_Id" ON public."Memories" USING btree ("Id");


--
-- Name: IX_Memories_Model; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Memories_Model" ON public."Memories" USING btree ("Model");


--
-- Name: IX_Memories_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Memories_Name" ON public."Memories" USING btree ("Name");


--
-- Name: IX_Motherboards_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Motherboards_Id" ON public."Motherboards" USING btree ("Id");


--
-- Name: IX_Motherboards_Model; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Motherboards_Model" ON public."Motherboards" USING btree ("Model");


--
-- Name: IX_Motherboards_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Motherboards_Name" ON public."Motherboards" USING btree ("Name");


--
-- Name: IX_PowerUnits_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_PowerUnits_Id" ON public."PowerUnits" USING btree ("Id");


--
-- Name: IX_PowerUnits_Model; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_PowerUnits_Model" ON public."PowerUnits" USING btree ("Model");


--
-- Name: IX_PowerUnits_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_PowerUnits_Name" ON public."PowerUnits" USING btree ("Name");


--
-- Name: IX_Reviews_SenderId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Reviews_SenderId" ON public."Reviews" USING btree ("SenderId");


--
-- Name: IX_Roles_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Roles_Id" ON public."Roles" USING btree ("Id");


--
-- Name: IX_Roles_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Roles_Name" ON public."Roles" USING btree ("Name");


--
-- Name: IX_Sessions_AccountId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Sessions_AccountId" ON public."Sessions" USING btree ("AccountId");


--
-- Name: IX_Sessions_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Sessions_Id" ON public."Sessions" USING btree ("Id");


--
-- Name: IX_Sessions_Refresh; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Sessions_Refresh" ON public."Sessions" USING btree ("Refresh");


--
-- Name: IX_Storages_Id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Storages_Id" ON public."Storages" USING btree ("Id");


--
-- Name: IX_Storages_Model; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Storages_Model" ON public."Storages" USING btree ("Model");


--
-- Name: IX_Storages_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Storages_Name" ON public."Storages" USING btree ("Name");


--
-- Name: Accounts FK_Accounts_Roles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Accounts"
    ADD CONSTRAINT "FK_Accounts_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."Roles"("Id") ON DELETE CASCADE;


--
-- Name: Assemblies FK_Assemblies_Accounts_AccountId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Assemblies"
    ADD CONSTRAINT "FK_Assemblies_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES public."Accounts"("Id") ON DELETE CASCADE;


--
-- Name: Assemblies FK_Assemblies_Cpus_CpuId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Assemblies"
    ADD CONSTRAINT "FK_Assemblies_Cpus_CpuId" FOREIGN KEY ("CpuId") REFERENCES public."Cpus"("Id") ON DELETE CASCADE;


--
-- Name: Assemblies FK_Assemblies_Frames_FrameId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Assemblies"
    ADD CONSTRAINT "FK_Assemblies_Frames_FrameId" FOREIGN KEY ("FrameId") REFERENCES public."Frames"("Id") ON DELETE CASCADE;


--
-- Name: Assemblies FK_Assemblies_Gpus_GpuId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Assemblies"
    ADD CONSTRAINT "FK_Assemblies_Gpus_GpuId" FOREIGN KEY ("GpuId") REFERENCES public."Gpus"("Id") ON DELETE CASCADE;


--
-- Name: Assemblies FK_Assemblies_Memories_MemoryId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Assemblies"
    ADD CONSTRAINT "FK_Assemblies_Memories_MemoryId" FOREIGN KEY ("MemoryId") REFERENCES public."Memories"("Id") ON DELETE CASCADE;


--
-- Name: Assemblies FK_Assemblies_Motherboards_MotherboardId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Assemblies"
    ADD CONSTRAINT "FK_Assemblies_Motherboards_MotherboardId" FOREIGN KEY ("MotherboardId") REFERENCES public."Motherboards"("Id") ON DELETE CASCADE;


--
-- Name: Assemblies FK_Assemblies_PowerUnits_PowerUnitId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Assemblies"
    ADD CONSTRAINT "FK_Assemblies_PowerUnits_PowerUnitId" FOREIGN KEY ("PowerUnitId") REFERENCES public."PowerUnits"("Id") ON DELETE CASCADE;


--
-- Name: Assemblies FK_Assemblies_Storages_StorageId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Assemblies"
    ADD CONSTRAINT "FK_Assemblies_Storages_StorageId" FOREIGN KEY ("StorageId") REFERENCES public."Storages"("Id") ON DELETE CASCADE;


--
-- Name: Reviews FK_Reviews_Accounts_SenderId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Reviews"
    ADD CONSTRAINT "FK_Reviews_Accounts_SenderId" FOREIGN KEY ("SenderId") REFERENCES public."Accounts"("Id") ON DELETE CASCADE;


--
-- Name: Sessions FK_Sessions_Accounts_AccountId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Sessions"
    ADD CONSTRAINT "FK_Sessions_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES public."Accounts"("Id") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

\unrestrict 4Cl9mh5MFviYISH0toIUjXv9GNdROcp0NozHWoDlO4bP8OIcKCeHFkcnec6hHJV

