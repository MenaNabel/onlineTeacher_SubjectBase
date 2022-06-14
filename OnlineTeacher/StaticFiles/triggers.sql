use OnlineExam;
 --insert trigger
create trigger tr_tblSiteInfo_ForInsertingStudent
on Student
For insert 
as
begin 
	
	update siteinfo
	set StudentsCount = StudentsCount+1;
	
end;

create trigger tr_tblSiteInfo_ForInsertingExam
on Exam
For insert 
as
begin 
	
	update siteinfo
	set ExamsCount = ExamsCount+1;
	

end;
create trigger tr_tblSiteInfo_ForInsertingFile
on Lectures
For insert 
as
begin 
	
	update siteinfo
	set FilesCount = FilesCount+1;


end;
create trigger tr_tblSiteInfo_ForInsertingLecture
on Lectures
For insert 
as
begin 
	
	update siteinfo
	set LecturesCount = LecturesCount+1;

end;
------ / delete  trigger -- 
create trigger tr_tblSiteInfo_FordeletingStudent
on Student
For delete 
as
begin 
	
	update siteinfo
	set StudentsCount = StudentsCount-1;
	
end;
create trigger tr_tblSiteInfo_FordeleteingExam
on Exam
For delete 
as
begin 
	
	update siteinfo
	set ExamsCount = ExamsCount-1;
	

end;
create trigger tr_tblSiteInfo_FordelteingFile
on Lectures
For delete 
as
begin 
	
	update siteinfo
	set FilesCount = FilesCount-1;


end;
create trigger tr_tblSiteInfo_FordeleteingLecture
on Lectures
For delete 
as
begin 
	
	update siteinfo
	set LecturesCount = LecturesCount-1;

end;
-----------------------------Honer List -------------
create trigger tr_tblHonerList_ForRecalcTheBest
on StudentExam
For insert 
as
begin 
	
	
		select  st.LevelID, st.ID  ,  sum(StEX.Degree) Degree , st.Name , image into tbWork
		from Student  st inner join
		StudentExam as StEX
		on   StEX.StudentID = st.ID
		group by  LevelID, ID , name , Image
		Order by Degree desc
		
		;WITH cte AS
		(
		   SELECT *,
				 ROW_NUMBER() OVER (PARTITION BY LevelID ORDER BY Degree DESC) AS rn
		   FROM tbWork
		)
		
		SELECT * into HonerTemp
		FROM cte
		WHERE rn = 1
	
		drop table tbWork;

		delete from HonerLists; 
		
		insert into  HonerLists( StudentID,studentName , StudentPicture , LevelID, TotalExamsDegree )
		select ht.ID , ht.Name, ht.Image,ht.LevelID,ht.Degree  from HonerTemp ht;

		drop table HonerTemp;


	
	
end;
create trigger tr_tblHonerList_ForRecalcTheBestUpdate
on StudentExam
For update 
as
begin 
	
	
		select  st.LevelID, st.ID  ,  sum(StEX.Degree) Degree , st.Name , image into tbWork
		from Student  st inner join
		StudentExam as StEX
		on   StEX.StudentID = st.ID
		group by  LevelID, ID , name , Image
		Order by Degree desc
		
		;WITH cte AS
		(
		   SELECT *,
				 ROW_NUMBER() OVER (PARTITION BY LevelID ORDER BY Degree DESC) AS rn
		   FROM tbWork
		)
		
		SELECT * into HonerTemp
		FROM cte
		WHERE rn = 1
	
		drop table tbWork;

		delete from HonerLists; 
		
		insert into  HonerLists( StudentID,studentName , StudentPicture , LevelID, TotalExamsDegree )
		select ht.ID , ht.Name, ht.Image,ht.LevelID,ht.Degree  from HonerTemp ht;

		drop table HonerTemp;


	
	
end;