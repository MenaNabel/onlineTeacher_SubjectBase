using Microsoft.EntityFrameworkCore;
using OnlineTeacher.Services.Exams.Helper;
using OnlineTeacher.ViewModels.Exams;
using OnlineTeacher.ViewModels.Questions;
using OnlineTeacher.DataAccess.Context;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using OnlineTeacher.ViewModels.StudentExam;
using Threenine.Data.Paging;
using OnlineTeacher.DataAccess;

namespace OnlineTeacher.Services.Exams
{

    public partial class ExamServicesAsync : IExamingServicesAsync
    {
        private double SingleQuestionDegree = 0;
        public async Task<ExamStatusViewModel> GetExamForCurrentStudent(int Id)
        {
          
            // Get Current User ID 
            int studentID = _User.GetStudentID();
            if (studentID == 0)
                return null;
            // Get Exam Include If Current Student Subscribe in Subjsct or Not  and relation with student
            var ExamSubscribed = await SingleOrDefult(Id, Ex => 
            Ex.Include(su=>su.StudentExams
            .Where(SE=>((SE.StudentID== studentID) && (SE.ExamID== Id) )))
            .Include(E => E.Lecture)
            .ThenInclude(L => L.Subject)
            .ThenInclude(S => S.Subscriptions.Where(s => s.StudentID == studentID))
            .Include(EQ => EQ.ExamQuestion)
            .ThenInclude(Q => Q.Question));
            if (ExamSubscribed is null) return null; 
            // if current Student Not Subecribe return null else return Exam 
            if (ExamSubscribed?.Lecture.Subject.Subscriptions.Count() < 1) 
                return null;
            if (ExamSubscribed?.StudentExams.Count() > 0) { // if alredy exam before 
                if (ExamSubscribed.StudentExams.FirstOrDefault()?.IsActive == false) 
                {
                    // can't Exam Again
                    return null; 
                }else // can Exam Again 
                return ExamSubscribed is null ? null : ConvertToExamViewModel(ExamSubscribed);
            }
            return ExamSubscribed is null ? null : ConvertToExamViewModel(ExamSubscribed);

        }
        public async Task<ExamCorrectionViewModel> ExamCorrection(ExamCorrectionViewModel examCorrection)
        {
            int StudentID = _User.GetStudentID();
            if (StudentID == 0)
                throw new UnauthorizedAccessException();
            var ExamWithLecture = _Exams.SingleOrDefaultAsync(ex => ex.ID == examCorrection.ExamID, include: ex => ex.Include(e => e.Lecture));
            StudentExam StudentExam = CorrectAncCalc(examCorrection, StudentID);
           
            ExamWithLecture.Wait();
            StudentExam.ExamType = ExamWithLecture.Result.EaxamType;
          
            return await _studentExam.AddAsync(StudentExam) is not null ? examCorrection : null;
        }

        public async Task<ExamCorrectionViewModel> Re_ExamCorrection(ExamCorrectionViewModel examCorrection)
        {
            int StudentID = _User.GetStudentID();
            if (StudentID == 0)
                throw new UnauthorizedAccessException();
       
            StudentExam StudentExam = CorrectAncCalc(examCorrection, StudentID);
            
            return await _studentExam.ReExam(StudentExam) is true ? examCorrection : null;
        }
        public async Task<bool> ReOpenExam(ReOpenExamViewModel  reOpenExamView)
        {
            int StudentID = _User.GetStudentID();
            if (StudentID == 0)
                throw new UnauthorizedAccessException();
            StudentExam StudentExam = new StudentExam
            {
                StudentID = StudentID,
                ExamID = reOpenExamView.ExamID,
                Reason = reOpenExamView.Reason
            };
            return await _studentExam.Request_ReOpenExam(StudentExam);
        }


        private StudentExam CorrectAncCalc(ExamCorrectionViewModel examCorrection, int StudentID)
        {
            double PassedDegree = ((2 * examCorrection.TotalDegree)/3);

            this.SingleQuestionDegree =  examCorrection.TotalDegree / examCorrection.Questions.Count();

            var Degree = examCorrection.Questions.Select(CorrectQuestion).Sum(Q => Q.Degree);
            var IsPassed = (Degree >= PassedDegree);
            var StudentExam = new StudentExam
            {
                Degree = Degree,
                IsPassed = IsPassed,
                ExamID = examCorrection.ExamID,
                StudentID = StudentID
            };
            return StudentExam;
        }



        private QuestionDegree CorrectQuestion(QuestionCorrectionViewModel question)
        {
            var questionDegree = new QuestionDegree { ID = question.ID };
            if (question.ActualAnswer == question.CorrectAnswer)
            {
                questionDegree.Degree = SingleQuestionDegree;
            }
            else
            {
                questionDegree.Degree = 0;
            }
            return questionDegree;
        }

        public async Task<IEnumerable<Grades>> GradesProgress(int studentID) {

          return await  _studentExam.GradesProgrees(studentID);
        
        }

        public async Task<IEnumerable<LectureExamDetailViewModel>> GetExamsForCurrentStudent()
        {
            int studentID = _User.GetStudentID();
            if (studentID == 0)
                return null;
           
          var Report=  await _Exams.GetListAsync(include: e => e.Include(s => s.StudentExams.Where(es=>es.StudentID == studentID)).ThenInclude(s => s.Student).Include(E=>E.Lecture));
            return Report.Items.Select(ConvertToLectureExamDetailViewModel).Where(s=>s is not null);
        }

        private LectureExamDetailViewModel ConvertToLectureExamDetailViewModel(Exam Exam)
        {
            if (Exam.StudentExams == null || Exam.StudentExams.Count() ==0)
                return null;
         if (Exam.StudentExams.FirstOrDefault()?.Student == null)
                return null;
            if (Exam.Lecture == null)
                return null;
            LectureExamDetailViewModel detailViewModel = new LectureExamDetailViewModel();
            detailViewModel.Degree = Exam.Degree;
            detailViewModel.ExamName = Exam.Name;
            detailViewModel.SubmitTime = Exam.StudentExams.FirstOrDefault().SubmitTime;
            detailViewModel.LectureName = Exam.Lecture.Name;
            return detailViewModel;
        }
    }
}
