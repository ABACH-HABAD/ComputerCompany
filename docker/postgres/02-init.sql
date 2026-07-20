
INSERT INTO "Roles" ("Id","Name") VALUES
	 ('4cc2534f-c52a-46ef-85cc-eedcd3bc5246'::uuid,'User'),
	 ('3b36f69f-c37e-40ab-a70f-0ecde3b0dc35'::uuid,'Employee'),
	 ('a81d5582-ab0c-42c3-9c23-70d774a22d22'::uuid,'Cashier'),
	 ('80fa76b3-bdd1-4428-9b87-9fddf752dbe8'::uuid,'Manager'),
	 ('4a93cc32-358b-4184-98b9-af3b12a55e52'::uuid,'Admin');

INSERT INTO "Accounts" ("Id","Login","Password","Name","RoleId") VALUES
	 ('019f794d-c816-7b8a-b020-dc69f38f702d'::uuid,'admin@computercompany.com','nIx6CvUCnXy1JHq7UV7z4burruX8gv/alVpj4bYbqwo=','Админ','4a93cc32-358b-4184-98b9-af3b12a55e52'::uuid);
