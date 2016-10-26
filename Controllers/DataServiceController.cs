using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace DataService.Controllers
{
    public class DataServiceController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [AcceptVerbs("POST")]
        public bool MergePropertyValues([FromBody]string id)
        {
            log.Info("MergeRecords:: Start inserting data");
            bool success = DBManager.MergeRecords(id);
            log.Info("MergeRecords:: End inserting data");
            return success;
        }

        [AcceptVerbs("GET")]
        public DataSet GetPropertiesData()
        {
            log.Info("GetData:: Start fetching data");

            DataSet dataset = DBManager.GetDataSet();

            log.Info("GetData:: End fetching data");

            return dataset;
        }

        [AcceptVerbs("GET")]
        public Object GetTemplateFile(string id)
        {
            log.Info("GetTemplateFile:: Start getting Template File with id = " + id );
            object fileObject = null;
            try
            {
                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Templates/" + id);
                string fileText = System.IO.File.ReadAllText(filePath);

                fileObject = JsonConvert.DeserializeObject(fileText);
                log.Info("Template:: Template File Fetch Ends id = " + id);
            }
            catch (Exception exception)
            {
                log.Error("Template::" + exception);
            }

            return fileObject;

        }

        public DataTable GetTemplateData()
        {
            log.Info("GetTemplatDataSet:: Start fetching data");

            DataTable templates = DBManager.GetTemplateDataTable();

            log.Info("GetTemplatDataSet:: End fetching data");

            return templates;
        }

    }
}
