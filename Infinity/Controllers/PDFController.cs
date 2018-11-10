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
        public async Task<IActionResult> Combine(List<PDF> pdfs)
        {
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

                MemoryStream ms = new MemoryStream(baos.ToArray());

                //using (FileStream fs = new FileStream("p.pdf", FileMode.Create))
                //    ms.CopyTo(fs);

                //var pushStreamContent = new PushStreamContent((outputstream, cotent, context) =>
                //{
                //    baos.Position = 0;
                //    baos.CopyTo(outputstream);
                //    baos.Dispose();
                //    outputstream.Close();
                //}, "application/pdf");
                    


                return new FileStreamResult(new FileStream("p.pdf", FileMode.Open), "application/pdf");
                //return File(ms, "application/pdf");
                //return new ContentResult() { Content = s, ContentType = "text/plan", StatusCode = 200 };

            }
        }
    }
}