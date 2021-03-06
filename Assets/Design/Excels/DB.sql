drop table DefaultScore;
create table DefaultScore(id integer PRIMARY KEY NOT NULL,level,rank,npcid,score);
insert into DefaultScore values (1,1,1,0,5000);
insert into DefaultScore values (2,1,2,1,3000);
insert into DefaultScore values (3,1,3,2,2000);
insert into DefaultScore values (4,2,1,0,6000);
insert into DefaultScore values (5,2,2,1,3000);
insert into DefaultScore values (6,2,3,2,2000);
insert into DefaultScore values (7,3,1,1,5500);
insert into DefaultScore values (8,3,2,0,3500);
insert into DefaultScore values (9,3,3,2,2500);
insert into DefaultScore values (10,4,1,2,7700);
insert into DefaultScore values (11,4,2,0,6600);
insert into DefaultScore values (12,4,3,1,5500);
insert into DefaultScore values (13,5,1,2,6200);
insert into DefaultScore values (14,5,2,0,5200);
insert into DefaultScore values (15,5,3,1,4700);drop table LevelData;
create table LevelData(id integer PRIMARY KEY NOT NULL,star1,star2,star3,max,target);
insert into LevelData values (1,400,700,980,1200,'""');
insert into LevelData values (2,500,800,1000,1200,'""');
insert into LevelData values (3,800,1200,1500,1700,'""');
insert into LevelData values (4,1400,1800,2100,2300,'""');
insert into LevelData values (5,1400,1800,2100,2300,'""');drop table PlayerData;
create table PlayerData(id integer PRIMARY KEY NOT NULL,gold,life,level);
insert into PlayerData values (1,0,0,0);drop table ScoreRecord;
create table ScoreRecord(id integer PRIMARY KEY NOT NULL,rank,score,star);
drop table TalkBorder;
create table TalkBorder(id integer PRIMARY KEY NOT NULL,chapterid,figure_type,content,isLeft);
insert into TalkBorder values (1,1,0,'content1111',1);
insert into TalkBorder values (2,1,1,'content1112',1);
insert into TalkBorder values (3,1,0,'content1113',0);
insert into TalkBorder values (4,2,1,'content1114',0);
insert into TalkBorder values (5,2,0,'chapter2',1);