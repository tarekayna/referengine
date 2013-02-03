--drop database referengine_db_local
--drop database referengine_db_test
create database referengine_db_test as copy of referengine_db
create database referengine_db_local as copy of referengine_db
