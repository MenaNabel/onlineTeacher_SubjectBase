using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.StudentExams.Helper;
using OnlineTeacher.Shared.Enums;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Exams;
using OnlineTeacher.ViewModels.StudentExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data;

namespace OnlineTeacher.Services.StudentExams
{
    public class StudentExamServiceAsync : IStudentExamServiceAsync
    {
        private readonly IRepositoryAsync<StudentExam> _StudentExam;
        
    

     
        public StudentExamServiceAsync(IRepositoryAsync<StudentExam> StudentExam)
        {
            _StudentExam = StudentExam;
        
        }
        public async Task<StudentExam> AddAsync(StudentExam StudentExam)
        {
            try
            {
                
                StudentExam.IsSubmitted = true;
                StudentExam.IsActive = false;
                var studentExam = await _StudentExam.InsertAsync(StudentExam);

                return studentExam.Entity;
            }
            catch (DbUpdateException ex) {

              return await ReExam(StudentExam) is true ? StudentExam : null;
            
            }
        }
        public async Task<IEnumerable<Grades>> GradesProgrees(int studentID) {
            try
            {
                var Exams = await _StudentExam.GetListAsync(SE => SE.StudentID == studentID && SE.ExamType == ExamType.Revision.ToString(), include: s => s.Include(e => e.Exam), orderBy: SE => SE.OrderBy(S => S.Exam.DateAndExamminationExpireTime));
                return Exams.Items.Select(ConvertToGrades);

            }
            catch (Exception e)
            {

                throw e;
            }

        
        }

        /// <summary>
        /// student Exam Again and re calculate degrees and update degree
        /// </summary>
        /// <param name="StudentExam"></param>
        /// <returns>
        ///  if update correctly 
        ///      true
        ///  else
        ///      false  
        /// </returns>
        public async Task<bool> ReExam(StudentExam StudentExam)
        {
            var Result = await Get(StudentExam.StudentID, StudentExam.ExamID);
            if (Result is null)
                return false;
            if (Result.IsActive == false) // if not allowed reopen Exam can't update
                return false;
            Result.Degree = StudentExam.Degree;
            Result.IsPassed = StudentExam.IsPassed;
            // Here
            Result.IsActive = false;
         

            return _StudentExam.Update(Result);
        }
        /// <summary>
        /// student  request  Exam Again
        /// </summary>
        /// <param name="StudentExam"></param>
        /// <returns>
        ///  if update correctly 
        ///      true
        ///  else
        ///      false  
        /// </returns>
        public async Task<bool> Request_ReOpenExam(StudentExam StudentExam)
        {
            var Result = await Get(StudentExam.StudentID, StudentExam.ExamID);
            if (Result is null)
                return false;
            Result.HaveRequest = true;
            Result.Reason = StudentExam.Reason;
         
            return _StudentExam.Update(Result);
        }
        /// <summary>
        /// teacher accept  request  Exam Again
        /// </summary>
        /// <param name="StudentExam"></param>
        /// <returns>
        ///  if update correctly 
        ///      true
        ///  else
        ///      false  
        /// </returns>
        public async Task<bool> AcceptRequest_ReExam(ReopenExamFeedback feedback)
        {
            var Result = await Get(feedback.StudentID, feedback.ExamID);
            if (Result is null)
                return false;
            Result.HaveRequest = false;
            Result.IsActive = feedback.IsAccept;
          
         
            return _StudentExam.Update(Result);
        }
      
        private  async Task<StudentExam> Get(int StudentID, int ExamID) {

          return await _StudentExam.SingleOrDefaultAsync(st => (st.StudentID == StudentID) && (st.ExamID == ExamID));
        }

        public async Task<IEnumerable<ReOpenExamDetailsViewModel>> GetReOpenExamRequests()
        {
           var ReopenExamRequests = await  _StudentExam.GetListAsync(stEx => stEx.HaveRequest == true, include: (StEX => StEX.Include(e => e.Exam).Include(s => s.Student)));
           return  ReopenExamRequests.Items.Select(ConvertToReOpenExamDetails);
        }

        private ReOpenExamDetailsViewModel ConvertToReOpenExamDetails(StudentExam studentExam)
        {
            return new ReOpenExamDetailsViewModel
            {
                ExamID = studentExam.ExamID,
                StudentID = studentExam.StudentID,
                ExamName = studentExam.Exam?.Name,
                StudentName = studentExam.Student?.Name,
                Reasone = studentExam.Reason

            };
        }
        private Grades ConvertToGrades(StudentExam exam) {
            var Grade = new Grades
            {
                ExamName = exam.Exam.Name,
                Degree = exam.Degree
            };
            return Grade;
        }


    }
}
