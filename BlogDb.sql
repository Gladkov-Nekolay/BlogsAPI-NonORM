CREATE TABLE public."Blog" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Name" text NOT NULL,
    "Text" text,
    "AuthorId" uuid NOT NULL
);

CREATE TABLE public."BlogTag" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Name" text
);

CREATE TABLE public."BlogVsTag" (
    "BlogId" uuid NOT NULL,
    "BlogTagId" uuid CONSTRAINT "BlogVsTag_BlogTag_not_null" NOT NULL
);

CREATE TABLE public."Comment" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Content" text NOT NULL,
    "BlogId" uuid NOT NULL,
    "AuthorId" uuid NOT NULL
);

CREATE TABLE public."User" (
    "Name" text,
    "Password" text NOT NULL,
    "Login" text NOT NULL,
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL
);

ALTER TABLE ONLY public."BlogTag"
    ADD CONSTRAINT "BlogTag_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."BlogVsTag"
    ADD CONSTRAINT "BlogVsTag_pkey" PRIMARY KEY ("BlogId", "BlogTagId");

ALTER TABLE ONLY public."Blog"
    ADD CONSTRAINT "Blog_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Comment"
    ADD CONSTRAINT "Comment_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."BlogVsTag"
    ADD CONSTRAINT "UniquePairBlogVsTags" UNIQUE ("BlogId", "BlogTagId");

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("Id");

CREATE INDEX "BlogNameIndex" ON public."Blog" USING btree ("Name") WITH (deduplicate_items='false');

CREATE INDEX "BlogTagNameIndex" ON public."BlogTag" USING btree ("Name") WITH (deduplicate_items='false');

CREATE UNIQUE INDEX "LoginIndex" ON public."User" USING btree ("Login") WITH (deduplicate_items='true');

ALTER TABLE ONLY public."Comment"
    ADD CONSTRAINT "AuthorId" FOREIGN KEY ("AuthorId") REFERENCES public."User"("Id") NOT VALID;

ALTER TABLE ONLY public."Blog"
    ADD CONSTRAINT "AuthorId" FOREIGN KEY ("AuthorId") REFERENCES public."User"("Id") NOT VALID;

ALTER TABLE ONLY public."Comment"
    ADD CONSTRAINT "BlogId" FOREIGN KEY ("BlogId") REFERENCES public."Blog"("Id") NOT VALID;

ALTER TABLE ONLY public."BlogVsTag"
    ADD CONSTRAINT "BlogId" FOREIGN KEY ("BlogId") REFERENCES public."Blog"("Id") NOT VALID;

ALTER TABLE ONLY public."BlogVsTag"
    ADD CONSTRAINT "TagId" FOREIGN KEY ("BlogTagId") REFERENCES public."BlogTag"("Id") NOT VALID;

