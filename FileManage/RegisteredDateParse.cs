﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Text;

namespace Camellia_Management_System.FileManage
{
    public class RegisteredDateParse : PdfParse
    {
        public static string GetHead(string innerText)
        {
            var result = "";
            innerText = MinimizeReferenceText(innerText);
            if (innerText.IndexOf("<b>Руководитель:</b>") == -1)
                return "Неизвестно";
            innerText = innerText.Substring(innerText.IndexOf("<b>Руководитель:</b>") + 20,
                innerText.Length - innerText.IndexOf("<b>Руководитель:</b>") - 20);
            var elements = innerText.Substring(0, innerText.IndexOf("<b>")).Replace("\r", " ").Replace("\n", " ")
                .Replace(".", " ")
                .Split(' ');
            foreach (var element in elements)
            {
                if (!element.Equals("И") &&
                    !element.Equals("А") &&
                    !element.Equals("О") &&
                    !element.Equals("ООО") &&
                    !element.Equals("ТОО") &&
                    !element.Equals("АО") &&
                    !element.Equals("КОО") &&
                    element.All(x => "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯӘІҢҒҮҰҚӨҺƏ".Contains(x)))
                    result += element.Trim() + " ";
            }
            // result = Regex.Replace(result, "[ ]+", "");
            while (result.IndexOf("  ") != -1)
                result = result.Replace("  ", " ");
            return result.Trim();
        }

        public static string GetName(string innerText)
        {
            innerText = MinimizeReferenceText(innerText);
            if (innerText.IndexOf("<b>Наименование:</b>") == -1)
                return "Неизвестно";
            innerText = innerText.Substring(innerText.IndexOf("<b>Наименование:</b>") + 20,
                innerText.Length - innerText.IndexOf("<b>Наименование:</b>") - 20);
            var result = innerText.Substring(0, innerText.IndexOf("<b>")).Replace("\r", " ").Replace("\n", " ").Trim();
            return result;
        }

        public static string GetPlace(string innerText)
        {
            innerText = MinimizeReferenceText(innerText);
            if (innerText.IndexOf("<b>Местонахождение:</b>") == -1)
                return "Неизвестно";
            innerText = innerText.Substring(innerText.IndexOf("<b>Местонахождение:</b>") + 23,
                innerText.Length - innerText.IndexOf("<b>Местонахождение:</b>") - 23);
            var result = innerText.Substring(0, innerText.IndexOf("Электрондық")).Replace("\r", "").Replace("\n", " ")
                .Trim();
            result = result.Replace("ақпараттық-анықтамалық қызметі\"", "");
            result = result.Replace("Касательно получения государственных услуг\"", "");
            result = result.Trim();
            return result;
        }

        public static string CountFounders(string innerText)
        {
            innerText = MinimizeReferenceText(innerText);
            if (innerText.IndexOf("<b>Количество участников (членов):</b>") == -1)
                return "Неизвестно";
            innerText = innerText.Substring(innerText.IndexOf("<b>Количество участников (членов):</b>") + 38,
                innerText.Length - innerText.IndexOf("<b>Количество участников (членов):</b>") - 38);
            var result = innerText.Substring(0, innerText.IndexOf("<b>")).Replace("\r", " ").Replace("\n", " ").Trim();
            return result;
        }

        public static string GetOccupation(string innerText)
        {
            innerText = MinimizeReferenceText(innerText);
            if (innerText.IndexOf("<b>Виды деятельности:</b>") == -1)
                return "Неизвестно";
            innerText = innerText.Substring(innerText.IndexOf("<b>Виды деятельности:</b>") + 25,
                innerText.Length - innerText.IndexOf("<b>Виды деятельности:</b>") - 25);
            var result = innerText.Substring(0, innerText.IndexOf("<b>")).Replace("\r", " ").Replace("\n", " ").Trim();
            return result;
        }
    }
}