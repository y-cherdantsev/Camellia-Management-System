﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Camellia_Management_System.FileManage
{
    /// @author Yevgeniy Cherdantsev
    /// @date 07.03.2020 17:22:27
    /// @version 1.0
    /// <summary>
    /// Class that parsing pdf and gets information from them in centralized objects
    /// </summary>
    public class PdfParser
    {
        private readonly string _innerText;



        /// @author Yevgeniy Cherdantsev
        /// @date 10.03.2020 10:38:28
        /// @version 1.0
        /// <summary>
        /// Construcvtor
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <param name="deleteFile">If the object should delete file after parsing</param>
        public PdfParser(string path, bool deleteFile = true)
        {
            var file = new FileInfo(path);
            try
            {
                _innerText = GetTextFromPdf(file);
            }
            catch (Exception)
            {
                // ignored
            }

            if (deleteFile)
                file.Delete();
        }



        /// @author Yevgeniy Cherdantsev
        /// @date 10.03.2020 10:39:27
        /// @version 1.0
        /// <summary>
        /// Gets text from pdf file (pdftohtml.exe util required)
        /// </summary>
        /// <param name="file">File that should be parsed</param>
        /// <returns>string - inner text</returns>
        private static string GetTextFromPdf(FileSystemInfo file)
        {
            var command = $"pdftohtml.exe -i -noframes -nomerge -enc UTF-8 \"{file.FullName}\"";
            System.Diagnostics.Process.Start("cmd.exe", "/C " + command)?.WaitForExit();

            var htmlFile = new FileInfo(file.FullName.Replace(file.Extension, ".html"));
            var text = File.ReadAllText(htmlFile.FullName);
            try
            {
                htmlFile.Delete();
            }
            catch (Exception)
            {
                // ignored
            }

            return text;
        }



        /// @author Yevgeniy Cherdantsev
        /// @date 10.03.2020 10:25:01
        /// @version 1.0
        /// <summary>
        /// Parsing of registration reference and getting of founders from it
        /// </summary>
        /// <returns>IEnumerable - list of founders</returns>
        public IEnumerable<string> GetFounders()
        {
            return FoundersPdfParse.GetFounders(_innerText);
        }
        
        /// @author Yevgeniy Cherdantsev
        /// @version 1.0
        /// <summary>
        /// Parsing of participation ul reference and getting of child companies from it
        /// </summary>
        /// <returns>IEnumerable - list of child companies</returns>
        public IEnumerable<string> GetChildCompanies()
        {
            return ChildCompaniesPdfParse.GetChildCompanies(_innerText);
        }
        
        /// @author Yevgeniy Cherdantsev
        /// @version 1.0
        /// <summary>
        /// Parsing of participation fl reference and getting of companies from it
        /// </summary>
        /// <returns>IEnumerable - list of companies where person is head</returns>
        public IEnumerable<string> GetWherePersonIsHead()
        {
            return WhereIsHeadPdfParse.GetWhereIsHead(_innerText);
        }   
        
        public List<ActivitiesDatePdfParse.DateActivity> GetActivitiesDates()
        {
            return ActivitiesDatePdfParse.GetDatesChanges(_innerText);
        }
        
        public string GetHead()
        {
            return RegisteredDateParse.GetHead(_innerText);
        }
        
        public string GetName()
        {
            return RegisteredDateParse.GetName(_innerText);
        }
        
        public string GetPlace()
        {
            return RegisteredDateParse.GetPlace(_innerText);
        }
        
        public string CountFounders()
        {
            return RegisteredDateParse.CountFounders(_innerText);
        }
        
        
        
    }
}