using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace SwPrpUtil.Models
{
	/// <summary>
	/// Structure for collect summary information from Solidworks document
	/// </summary>
	internal class SwFileSummaryInfo
	{
		//ctor default
		public SwFileSummaryInfo()
		{
		}

		/// <summary>
		/// Creat Structure from open document
		/// </summary>
		/// <param name="doc"></param>
		public SwFileSummaryInfo(ModelDoc2 doc)
		{
			ReadSummaryInfo(doc);
		}

		/// <summary>
		/// Read summary information from solidworks document
		/// </summary>
		/// <param name="doc">Opende solidworks document</param>
		/// <exception cref="ArgumentException"></exception>
		public void ReadSummaryInfo(ModelDoc2 doc)
		{
			if (doc == null) throw new ArgumentException(nameof(doc));

			SwSumInfoAuthor = doc.SummaryInfo[(int)swSummInfoField_e.swSumInfoAuthor];
			SwSumInfoComment = doc.SummaryInfo[(int)swSummInfoField_e.swSumInfoComment];
			SwSumInfoCreateDate = doc.SummaryInfo[(int)swSummInfoField_e.swSumInfoCreateDate];
			SwSumInfoCreateDate2 = doc.SummaryInfo[(int)swSummInfoField_e.swSumInfoCreateDate2];
			SwSumInfoKeywords = doc.SummaryInfo[(int)swSummInfoField_e.swSumInfoKeywords];
			SwSumInfoSaveDate = doc.SummaryInfo[(int)swSummInfoField_e.swSumInfoSaveDate];
			SwSumInfoSaveDate2 = doc.SummaryInfo[(int)swSummInfoField_e.swSumInfoSaveDate2];
			SwSumInfoSavedBy = doc.SummaryInfo[(int)swSummInfoField_e.swSumInfoSavedBy];
			SwSumInfoSubject = doc.SummaryInfo[(int)swSummInfoField_e.swSumInfoSubject];
			SwSumInfoTitle = doc.SummaryInfo[(int)swSummInfoField_e.swSumInfoTitle];
		}

		public string SwSumInfoAuthor { get; set; }
		public string SwSumInfoComment { get; set; }
		public string SwSumInfoCreateDate { get; set; }
		public string SwSumInfoCreateDate2 { get; set; }
		public string SwSumInfoKeywords { get; set; }
		public string SwSumInfoSaveDate { get; set; }
		public string SwSumInfoSaveDate2 { get; set; }
		public string SwSumInfoSavedBy { get; set; }
		public string SwSumInfoSubject { get; set; }
		public string SwSumInfoTitle { get; set; }
	}
}