using StandardEng.Data.DB;
using StandardEng.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace StandardEng.Web.Common
{
    public class WarrantyExpiryReminder
    {
        public static void GetListOfWarrantyExpiry()
        {
            List<GetWarrantyExpiryCustomerList_Result> listExpitry = new List<GetWarrantyExpiryCustomerList_Result>();
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                listExpitry = context.GetWarrantyExpiryCustomerList().Where(m=>m.IsReadyForAMC == false).ToList();

                if (listExpitry.Count > 0)
                {

                    string bodyTemplate = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Template/WarrantyExpiryReminder.html"));

                    foreach (GetWarrantyExpiryCustomerList_Result obj in listExpitry)
                    {
                        tblCommissioning commisioningObj = new tblCommissioning();
                        bodyTemplate = bodyTemplate.Replace("[@CustomerName]", obj.CustomerName);
                        bodyTemplate = bodyTemplate.Replace("[@MachineModel]", obj.MachineName);
                        bodyTemplate = bodyTemplate.Replace("[@MachineType]", obj.MachineTypeName);
                        bodyTemplate = bodyTemplate.Replace("[@MachineSerialNo]", obj.MachineSerialNo);
                        bodyTemplate = bodyTemplate.Replace("[@ExpiryDate]", obj.WarrantyExpireDate);

                        Task task = new Task(() => EmailHelper.SendAsyncEmail(obj.CustomerEmail, "Machine Warranty Expiry Reminder", bodyTemplate, true));
                        task.Start();

                        commisioningObj = context.tblCommissioning.Where(m => m.CommissioningId == obj.CommissioningId).FirstOrDefault();
                        commisioningObj.IsReadyForAMC = true;

                        context.Entry(commisioningObj).State = EntityState.Modified;
                        context.SaveChanges();
                    }

                }
            }
        }
       
    }
}