using AutoMapper;
using OnlineTeacher.DataAccess;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data;

namespace OnlineTeacher.Services.Home
{
    public class SiteInfoServicesAsync : ISiteInfo
    {
        private IRepositoryAsync<SiteInfo> _SiteInfo ;
        private IMapper _Mapper ;
        private readonly OnlineExamContext _context;

        public SiteInfoServicesAsync(IRepositoryAsync<SiteInfo> SiteInfo , IMapper mapper, OnlineExamContext context)
        {
            _SiteInfo = SiteInfo;
            _Mapper = mapper;
            _context = context;
        }
        public async Task<SiteInfoViewModel> getSiteInfo()
        {
            //var SiteInfo =  await _SiteInfo.SingleOrDefaultAsync();

            // return SiteInfo is not null ? _Mapper.Map<SiteInfoViewModel>(SiteInfo) : null ;
            int filesCount, studentsCount, lectureCount, examCount = 0;
            filesCount = _context.Lectures.Where(lec => lec.FileName != null).ToList().Count;
            studentsCount = _context.Student.Select(x => x.ID).ToList().Count;
            lectureCount = _context.Lectures.ToList().Count;
            examCount = _context.Exam.ToList().Count;
            SiteInfoViewModel siteInfo = new SiteInfoViewModel
            {
                FilesCount = filesCount,
                StudentsCount = studentsCount,
                LecturesCount = lectureCount,
                ExamsCount = examCount
            };
            SiteInfo siteInfo2 = new SiteInfo();
            _Mapper.Map(siteInfo, siteInfo2);
            _context.SiteInfo.Remove(_context.SiteInfo.FirstOrDefault());
            await _context.SiteInfo.AddAsync(siteInfo2);
            await _context.SaveChangesAsync();

            return siteInfo != null ? siteInfo : null;
        }
    }
}
