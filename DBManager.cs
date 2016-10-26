using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace DataService
{
    public static class DBManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string dbPath = HttpContext.Current.Server.MapPath("~/App_Data/production.accdb");
        public static bool MergeRecords(string inputString)
        { 
            bool success = true;
            foreach(string propertyValues in inputString.Split(','))
            {
               string[] propertyValue = propertyValues.Split(':');
               log.Info(String.Format("MergeRecords:: Inserting:: propertyId:{0} , propertyValue:{1}", propertyValue[0], propertyValue[1]));

                string insertCommand = "INSERT INTO PropertyValues ([propertyId],[propertyValue]) VALUES (?,?) ";

                using (OleDbConnection connection = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False", dbPath)))
                    {
                        using (OleDbCommand command = new OleDbCommand(insertCommand,connection))
                            {
                                try
                                    {

                                        command.Parameters.Add("propertyId", OleDbType.Numeric).Value = Convert.ToInt32(propertyValue[0]);
                                        command.Parameters.Add("propertyValue", OleDbType.VarChar).Value = propertyValue[1];

                                        log.Info("MergeRecords:: opening connection");
                                        connection.Open();
                                        log.Info("MergeRecords:: Success: connection opened!");

                                        int rowsAffected = command.ExecuteNonQuery();
                                        if (rowsAffected != 1)
                                            success = false;
                                    }
                                    catch (Exception exception)
                                    {
                                        log.Error("MergeRecords:: exception occurred:", exception);
                                    }
                            }
                    }
            }
            return success;
        }
        public static DataSet GetDataSet()
        {

            DataSet dataset = new DataSet("applicationDataSet");

            // Create Table Properties 
            DataTable properties = new DataTable("Properties");

            DataColumn propertyId = new DataColumn("propertyId", typeof(Int32));
            properties.Columns.Add(propertyId);

            DataColumn parentCategory = new DataColumn("parentCategory", typeof(string));
            properties.Columns.Add(parentCategory);

            DataColumn category = new DataColumn("category", typeof(string));
            properties.Columns.Add(category);

            DataColumn propertyName = new DataColumn("propertyName", typeof(string));
            properties.Columns.Add(propertyName);

            dataset.Tables.Add(properties);

            // Create Table PropertyValues

            DataTable propertyValues = new DataTable("PropertyValues");

            DataColumn propertyId_1 = new DataColumn("propertyId", typeof(Int32));
            propertyValues.Columns.Add(propertyId_1);

            DataColumn propertyValue = new DataColumn("propertyValue", typeof(string));
            propertyValues.Columns.Add(propertyValue);

            DataColumn rowStatus = new DataColumn("rowStatus", typeof(Int32));
            rowStatus.DefaultValue = 0;
            propertyValues.Columns.Add(rowStatus);

            dataset.Tables.Add(propertyValues);

            using (OleDbConnection connection = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False", dbPath)))
            {
                OleDbDataAdapter propertiesAdapter = new OleDbDataAdapter("SELECT * FROM Properties", connection);
                OleDbDataAdapter propertyValuesAdapter = new OleDbDataAdapter("SELECT * FROM PropertyValues", connection);
                try
                {
                    log.Info("GetDataSet:: opening connection");
                    connection.Open();
                    log.Info("GetDataSet:: Success: connection opened!");

                    propertiesAdapter.Fill(dataset, "Properties");
                    propertyValuesAdapter.Fill(dataset, "PropertyValues");

                    log.Info("GetDataSet:: adapters are filled");

                    DataRelation relation = dataset.Relations.Add("fk_relation", dataset.Tables["Properties"].Columns["propertyId"], dataset.Tables["PropertyValues"].Columns["propertyId"]);

                    log.Info("GetDataSet:: foreign key relation created!");
                }
                catch (Exception exception)
                {
                    log.Fatal("GetDataSet:: ", exception);
                }
            }
            
            return dataset;
        }

        public static DataTable GetTemplateDataTable()
        {

            // Create Table Properties 
            DataTable templates = new DataTable("Templates");

            DataColumn templateId = new DataColumn("templateId", typeof(Int32));
            templates.Columns.Add(templateId);

            DataColumn parentCategory = new DataColumn("parentCategory", typeof(string));
            templates.Columns.Add(parentCategory);

            DataColumn parentCategoryDisplay = new DataColumn("parentCategoryDisplay", typeof(string));
            templates.Columns.Add(parentCategoryDisplay);

            DataColumn category = new DataColumn("category", typeof(string));
            templates.Columns.Add(category);

            DataColumn categoryDisplay = new DataColumn("categoryDisplay", typeof(string));
            templates.Columns.Add(categoryDisplay);


            using (OleDbConnection connection = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False", dbPath)))
            {
                OleDbDataAdapter templatesAdapter = new OleDbDataAdapter("SELECT * FROM Templates", connection);
                try
                {
                    log.Info("GetTemplateDataSet:: opening connection");
                    connection.Open();
                    log.Info("GetTemplateDataSet:: Success: connection opened!");

                    templatesAdapter.Fill(templates);
                    
                    log.Info("GetTemplateDataSet:: adapter is filled");

                }
                catch (Exception exception)
                {
                    log.Fatal("GetTemplateDataSet:: ", exception);
                }
            }

            return templates;
        }

    }
}