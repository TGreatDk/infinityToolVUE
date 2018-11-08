using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Infinity.Models;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infinity.Controllers
{
    [Route("api/pdf")]
    [ApiController]
    public class PDFController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hey!";
        }

        [HttpGet]
        [Route("list")]
        public List<PDF> GetList()
        {
            List<PDF> list = new List<PDF>();
            list.Add(new PDF() { Name = "File" });
            list.Add(new PDF() { Name = "File2" });

            return list;
        }

        [HttpPost]
        [Route("Combined")]
        public HttpResponseMessage Combine(List<PDF> pdfs)
        {
            string ret = "";
            List<byte[]> sourceFiles = new List<byte[]>();
            foreach (PDF pdf in pdfs)
            {
                ret += pdf.Name + ";";
                sourceFiles.Add(System.IO.File.ReadAllBytes(pdf.FullPath));

            }
            using (ByteArrayOutputStream baos = new ByteArrayOutputStream())
            {
                PdfDocument doc = new PdfDocument(new PdfWriter(baos));
                PdfMerger merger = new PdfMerger(doc);

                foreach (var pdf in pdfs)
                {
                    PdfDocument tmpDoc = new PdfDocument(new PdfReader(pdf.FullPath));
                    merger.Merge(tmpDoc, 1, tmpDoc.GetNumberOfPages());
                    tmpDoc.Close();
                }

                merger.Close();
                doc.Close();

                using (MemoryStream ms = new MemoryStream(baos.ToArray()))
                {
                    using (FileStream file = new FileStream("p2.pdf", FileMode.Create))
                        ms.CopyTo(file);
                    var pushStreamContent = new PushStreamContent((outputstream, cotent, context) =>
                    {
                        ms.Position = 0;
                        ms.CopyTo(outputstream);
                        ms.Dispose();
                        outputstream.Close();
                    }, "application/pdf");
                    return new HttpResponseMessage(HttpStatusCode.OK) { Content = pushStreamContent };
                }
            }
        }
    }
}