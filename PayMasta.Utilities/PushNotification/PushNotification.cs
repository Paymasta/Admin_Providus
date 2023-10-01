using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities.PushNotification
{
    public class PushNotification: IPushNotification
    {
        //public FcmPushResponse FireBasePush(PushNotificationModel objPush)
        //{
        //    //"Request".ErrorLog("PushNotificationRepository.cs", "FireBasePush Request", objPush);
        //    FcmPushResponse response = new FcmPushResponse();
        //    try
        //    {
        //        WebRequest tRequest = WebRequest.Create(ConfigurationManager.AppSettings["FCM_HOST_URL"]);
        //        tRequest.Method = "post";
        //        tRequest.ContentType = "application/json";
        //        Byte[] byteArray = Encoding.UTF8.GetBytes(objPush.message);
        //        tRequest.Headers.Add(string.Format("Authorization: key={0}", ConfigurationManager.AppSettings["FCM_APPLICATION_KEY"]));
        //        tRequest.Headers.Add(string.Format("Sender: id={0}", ConfigurationManager.AppSettings["FCM_SERVER_KEY"]));
        //        tRequest.ContentLength = byteArray.Length;
        //        tRequest.ContentType = "application/json";
        //        using (Stream dataStream = tRequest.GetRequestStream())
        //        {
        //            dataStream.Write(byteArray, 0, byteArray.Length);
        //            using (WebResponse tResponse = tRequest.GetResponse())
        //            {
        //                using (Stream dataStreamResponse = tResponse.GetResponseStream())
        //                {
        //                    using (StreamReader tReader = new StreamReader(dataStreamResponse))
        //                    {
        //                        String sResponseFromServer = tReader.ReadToEnd();
        //                        response = JsonConvert.DeserializeObject<FcmPushResponse>(sResponseFromServer);
        //                        // "Response".ErrorLog("PushNotificationRepository.cs", "FireBasePush Response", response);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        var sss = ex.Message;
        //        if (ex.InnerException != null)
        //        {
        //            var ss = ex.InnerException;
        //        }
        //        // ex.Message.ErrorLog("PushNotificationRepository.cs", "FireBasePush EXCEPTION", objPush);                
        //    }
        //    return response;
        //}

        public string SendPush(PushModel model)
        {
            string response = "";
            //Create the web request with fire base API  
            WebRequest tRequest = WebRequest.Create(AppSetting.FCMURL);
            tRequest.Method = "post";
            //serverKey - Key from Firebase cloud messaging server  
            tRequest.Headers.Add(string.Format("Authorization: key={0}", AppSetting.FCM_ServerKey));
            //Sender Id - From firebase project setting  
            tRequest.Headers.Add(string.Format("Sender: id={0}", AppSetting.FCM_SenderId));
            tRequest.ContentType = "application/json";
            var serializer = string.Empty;
            try
            {
                var payload = new
                {
                    to = model.DeviceToken,
                    priority = "high",
                    content_available = true,
                    mutable_content = true,
                    notification = new
                    {
                        datetime = model.DateTime,
                        body = model.Message,
                        title = model.Title,
                        sound = "Dukkan_Tune.wav",
                        badge = model.BadgeCounter,
                    },
                    data = model
                    //data = new
                    //{
                    //    datetime = model.DateTime,
                    //    body = model.Message,
                    //    title = model.Title,
                    //    sound = "sound.caf",
                    //    badge = model.BadgeCounter
                    //},
                };
                //Serialize your object to a Json-String:
                serializer = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

                Byte[] byteArray = Encoding.UTF8.GetBytes(serializer);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    response = sResponseFromServer;
                                }
                        }
                    }
                }
                // await RecordPush(2, serializer, response, notification_id);

            }
            catch (Exception ex)
            {
                //await RecordPush(-2, serializer, ex.Message, notification_id);
            }

            //_logger.LogInformation("iOS Push Response - "+ response);
            return response;
        }
    }
}
