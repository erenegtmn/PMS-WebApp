using RDChartSite.BussinessLayer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDChartSite.BussinessLayer.Abstract;
using RDChartSite.BussinessLayer;
using RDChartSite.Entities;
using RDChartSite.Common.Helpers;
using RDChartSite.Entities.Messages;
using Microsoft.SqlServer.Server;
using System.CodeDom.Compiler;
using System.Net.NetworkInformation;
using RDChartSite.Entities.ValueObjects;

namespace RDChartSite.BussinessLayer
{
    public class FormsManager : ManagerBase<OffDayForms>
    {
        UserManager userManager = new UserManager();
        DateTime currentdate = DateTime.Now;
        public BussinessLayerResult<OffDayForms> InsertIntoDb(OffDayFormsViewModel model)
        {
            OffDayForms offDayForm = Find(x => x.StartDate == model.StartDate);
            BussinessLayerResult<OffDayForms> result = new BussinessLayerResult<OffDayForms>();

            if (offDayForm != null)
            {
                result.AddError(ErrorMessageCode.OffDayFormAlreadyExists, "Zaten bu güne ait bir izin talebiniz bulunmaktadır.");
            }
            else
            {
                Guid GeneratedGuid = Guid.NewGuid();
                int dbInsertResult = Insert(new OffDayForms()
                {

                    StartDate = new DateTime(model.StartDate.Year, model.StartDate.Month, model.StartDate.Day),
                    ReturnDate = new DateTime(model.ReturnDate.Year, model.ReturnDate.Month, model.ReturnDate.Day),
                    PType = model.PType,
                    FormGuid = GeneratedGuid,
                    StartTime = model.StartTime,
                    ReturnTime = model.ReturnTime,
                    ProjectCode = model.ProjectCode,
                    Description = model.Description,
                    Reason = model.Reason,
                    PostTime = currentdate,
                    IsApproved = false,
                    UserId = model.UserId
                }) ;
                
                if (dbInsertResult > 0)
                {
                    Users mailtouser = userManager.Find(x => x.UserId == model.MailTo);
                    string subject = "İzin Talep Formu";
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string approvalPageUrl = $"{siteUri}/InSite/FormApprovalPage?FormGuid={GeneratedGuid}";
                    string body = $@"
                    <html>
                    <head>
                        <style>
                            .card {{
                                border: 1px solid #e0e0e0;
                                border-radius: 5px;
                                padding: 10px;
                                background-color: #f9f9f9;
                                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                                max-width: 400px;
                                margin: 0 auto;
                            }}
                            .button {{
                                display: inline-block;
                                padding: 10px 20px;
                                background-color: #007bff;
                                color: #fff;
                                border-radius: 5px;
                                text-decoration: none;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class=""card"">
                            <h2>İzin Talep Formu</h2>
                            <p>Merhaba {mailtouser.Name},</p>
                            <p>Aşağıda belirtilen izin formunun bilgilerini içeren bir talep alındı:</p>
                            <p>
                            Ad Soyad: {model.Name + " " +model.Surname}<br>
                            Başlangıç Tarihi: {model.StartDate}<br>
                            Dönüş Tarihi: {model.ReturnDate}<br>
                            İzin Türü: {model.PType}<br>
                            Başlangıç Saati: {model.StartTime}<br>
                            Dönüş Saati: {model.ReturnTime}<br>
                            Proje Kodu: {model.ProjectCode}<br>
                            Açıklama: {model.Description}<br>
                            İzin Nedeni: {model.Reason}</p>
                            <p>
                                  Talebi onaylamak için aşağıdaki butona tıklayabilirsiniz:<br>
                           <a href='{approvalPageUrl}' target='_blank'>İncele</a>
                        </div>
                    </body>
                    </html>
                     ";
                    MailHelper.SendMail(subject, body, mailtouser.Email);
                }
            }
            return result;
        }
    }
}
