if not exists (select * from sysobjects where name='person' and xtype='U')
	CREATE TABLE person (
	  id bigint NOT NULL IDENTITY,
	  first_name varchar(80) NOT NULL,
	  last_name varchar(80) NOT NULL,
	  gender varchar(6) NOT NULL,
	  address varchar(100) DEFAULT NULL,
	  PRIMARY KEY (id)
	)
go