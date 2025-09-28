drop table if exists user;
create table user (
  user_id integer primary key autoincrement,
  username string not null,
  email string not null
);

drop table if exists message;
create table message (
  message_id integer primary key autoincrement,
  author_id integer not null,
  text string not null,
  pub_date integer
);

select *
from message;

select message.*, user.* from message, user where message.author_id = user_id

select username, m.*
from message m 
    inner join user u 
        on u.user_id = m.author_id
        order by m.pub_date desc
