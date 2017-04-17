using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Reverse.Regression.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml;

namespace SPMeta2.Reverse.Tests.Services
{
    [Serializable]
    public class ReverseCoverageResult
    {
        public ReverseCoverageResult()
        {
            Properties = new List<ReverseCoveragePropertyResult>();
        }

        public DefinitionBase Model { get; set; }
        public string ModelFullClassName { get; set; }
        public string ModelShortClassName { get; set; }

        public List<ReverseCoveragePropertyResult> Properties { get; set; }
    }

    [Serializable]
    public class ReverseCoveragePropertyResult
    {
        public string SrcPropertyValue { get; set; }
        public string SrcPropertyName { get; set; }

        public string DstPropertyValue { get; set; }
        public string DstPropertyName { get; set; }

        public bool IsValid { get; set; }
        public string Message { get; set; }
    }

    public class DefaultCoverageReportService
    {
        public void RegenerateReports(List<ModelValidationResult> results)
        {
            // huge TODO :)
            var reportsFolder = "../../../SPMeta2.Reverse.Tests/_coverage_reports";
            var readMeFolder = "../../../..";

            var uniqueResults = MapUniqueResults(results);

            SaveUniqueResults(uniqueResults, reportsFolder, "_m2.reverse-coverage.xml");
            SaveDefinitionResults(uniqueResults, reportsFolder);

            var finalReportFileName = "_m2.reverse-coverage.html";

            SaveFinalReport(reportsFolder, finalReportFileName);

            PatchReadmeFile(readMeFolder, reportsFolder, finalReportFileName, uniqueResults);
            PatchStatusFile(readMeFolder, reportsFolder, finalReportFileName);
        }

        private List<ReverseCoverageResult> MapUniqueResults(List<ModelValidationResult> validationResults)
        {
            var uniqueResults = new List<ReverseCoverageResult>();

            foreach (var result in validationResults)
            {
                if (!uniqueResults.Any(r => r.Model.GetType() == result.Model.GetType()))
                {
                    var newResult = new ReverseCoverageResult();

                    newResult.Model = result.Model;

                    newResult.ModelFullClassName = result.Model.GetType().FullName;
                    newResult.ModelShortClassName = result.Model.GetType().Name;

                    foreach (var propResult in result.Properties)
                    {
                        var newPropResult = new ReverseCoveragePropertyResult();

                        if (propResult.Src != null)
                        {
                            newPropResult.SrcPropertyName = propResult.Src.Name;
                            newPropResult.SrcPropertyValue = ConvertUtils.ToString(propResult.Src.Value);
                        }

                        if (propResult.Dst != null)
                        {
                            newPropResult.DstPropertyName = propResult.Dst.Name;
                            newPropResult.DstPropertyValue = ConvertUtils.ToString(propResult.Dst.Value);
                        }

                        newPropResult.IsValid = propResult.IsValid;
                        newPropResult.Message = propResult.Message;

                        newResult.Properties.Add(newPropResult);
                    }

                    uniqueResults.Add(newResult);
                }
            }

            uniqueResults = uniqueResults.OrderBy(r => r.Model.GetType().Name)
                                         .ToList();

            return uniqueResults;
        }

        private void SaveUniqueResults(List<ReverseCoverageResult> uniqueResults,
            string reportsFolder, string fileName)
        {
            var types = uniqueResults.Select(r => r.Model.GetType()).ToList();

            types.AddRange(uniqueResults.Select(r => r.Model.GetType()).ToList());

            types.Add(typeof(ModelValidationResult));
            types.Add(typeof(PropertyValidationResult));

            var xml = XmlSerializerUtils.SerializeToString(uniqueResults, types);
            xml = MakeHTMLLookGreat(xml);

            System.IO.File.WriteAllText(reportsFolder + "/" + fileName, xml);
        }

        private void PatchStatusFile(string readMeFolder, string reportsFolder, string reportFileName)
        {
            var reportContent = System.IO.File.ReadAllText(reportsFolder + "/" + reportFileName);
            reportContent = MakeHTMLLookGreat(reportContent);

            // updating readme
            var readMeContent = System.IO.File.ReadAllText(readMeFolder + "/M2.Reverse.Coverage.Status-template.md");

            readMeContent = readMeContent.Replace("[[COVERAGE-REPORT]]", reportContent);

            System.IO.File.WriteAllText(readMeFolder + "/M2.Reverse.Coverage.Status.md", readMeContent);
        }

        private void PatchReadmeFile(string readMeFolder, string reportsFolder, string reportFileName,
            List<ReverseCoverageResult> results)
        {
            var reportContent = System.IO.File.ReadAllText(reportsFolder + "/" + reportFileName);
            reportContent = MakeHTMLLookGreat(reportContent);

            // updating readme
            var readMeContent = System.IO.File.ReadAllText(readMeFolder + "/README-TEMPLATE.md");

            var defNames = Directory.GetFiles(reportsFolder, "*.def-coverage.html")
                .Select(f => Path.GetFileName(f))
                .Select(f => f.Split('.')[1])
                .OrderBy(f => f);

            reportContent = string.Empty;

            foreach (var defName in defNames)
                reportContent += String.Format("* {0}{1}", defName, Environment.NewLine);

            readMeContent = readMeContent.Replace("[[COVERAGE-DEFINITIONS]]", reportContent);

            System.IO.File.WriteAllText(readMeFolder + "/README.md", readMeContent);
        }

        private void SaveFinalReport(string reportsFolder, string reportFileName)
        {
            // generae the full report
            var reportContent = string.Empty;
            reportContent += "<div class='m-reverse-report-cnt'>";

            var files = Directory.GetFiles(reportsFolder, "*.def-coverage.html")
                                 .OrderBy(f => f);

            foreach (var file in files)
            {
                reportContent += System.IO.File.ReadAllText(file);
            }

            reportContent += "</div>";

            reportContent = MakeHTMLLookGreat(reportContent);

            System.IO.File.WriteAllText(reportsFolder + "/" + reportFileName, reportContent);
        }

        private string MakeHTMLLookGreat(string htmlContent)
        {
            // http://stackoverflow.com/questions/1123718/format-xml-string-to-print-friendly-xml-string

            var result = string.Empty;

            using (var mStream = new MemoryStream())
            {
                using (var writer = new XmlTextWriter(mStream, Encoding.Unicode))
                {
                    var document = new XmlDocument();

                    document.LoadXml(htmlContent);

                    writer.Formatting = Formatting.Indented;

                    document.WriteContentTo(writer);

                    writer.Flush();
                    mStream.Flush();

                    mStream.Position = 0;

                    using (StreamReader reader = new StreamReader(mStream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }
        private void SaveDefinitionResults(List<ReverseCoverageResult> uniqueResults, string reportsFolder)
        {
            // save all reports for caching purposes
            foreach (var result in uniqueResults.OrderBy(s => s.ModelShortClassName))
            {
                var definitionName = result.ModelShortClassName;
                var fileName = string.Format("_m2.{0}.def-coverage.html", definitionName);

                var reportContent = GenerateDefinitionCoverateTable(result);

                reportContent = MakeHTMLLookGreat(reportContent);

                System.IO.File.WriteAllText(reportsFolder + "/" + fileName, reportContent);
            }
        }

        private string GenerateDefinitionCoverateTable(ReverseCoverageResult result)
        {
            var report = string.Empty;

            report += "<div>";

            report += string.Format("<h4>{0}</h4>", result.ModelShortClassName);

            report += "<table>";

            report += "<thead>";
            report += "<td>Property</td>";
            report += "<td>Support</td>";
            report += "<td>Comments</td>";
            report += "</thead>";

            report += "<tbody>";

            foreach (var propResult in result.Properties.OrderBy(p =>
            {
                var localPropName = p.SrcPropertyName;

                if (localPropName.Contains("."))
                    localPropName = localPropName.Split('.')[1];

                return localPropName;
            }))
            {
                var propName = propResult.SrcPropertyName;

                // method calls, such as 's.Scope.ToString()	'
                if (propName.Contains("."))
                    propName = propName.Split('.')[1];

                report += "<tr>";
                report += string.Format("<td>{0}</td>", propName);

                if (propResult.Message == SkipMessages.NotImplemented)
                {
                    report += string.Format("<td>{0}</td>", false);
                    report += string.Format("<td>{0}</td>", propResult.Message);
                }
                else
                {
                    report += string.Format("<td>{0}</td>", propResult.IsValid);
                    report += string.Format("<td>{0}</td>", propResult.Message);
                }

                report += "</tr>";
            }

            report += "</tbody>";

            report += "</table>";

            report += "</div>";

            return report;
        }
    }
}
