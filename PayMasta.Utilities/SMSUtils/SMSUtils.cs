using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PayMasta.Utilities.SMSUtils
{
    public class SMSUtils : ISMSUtils
    {
        private string path = AppSetting.SMS_Path;

        //public async Task<bool> SendSms(SMSModel model)
        //{
        //    try
        //    {
        //        //var request = new
        //        //{
        //        //    username = AppSetting.SMS_UserName,
        //        //    password = AppSetting.SMS_Password,
        //        //    msg = model.Message,
        //        //    texttype = AppSetting.SMS_TextType,
        //        //    numbers = model.CountryCode.Replace("+", string.Empty) + model.PhoneNumber,
        //        //    sender = AppSetting.SMS_Sender
        //        //};

        //        //path = path +"?"+
        //        //    "username=" + request.username +
        //        //    "&password=" + request.password +
        //        //    "&msg=" + request.msg +
        //        //    "&texttype=" + request.texttype +
        //        //    "&numbers=" + request.numbers +
        //        //    "&sender=" + request.sender;

        //        var request = new
        //        {
        //            application = AppSetting.SMS_application,
        //            password = AppSetting.SMS_Password,
        //            content = model.Message,
        //            destination = model.CountryCode.Replace("+", string.Empty) + model.PhoneNumber,
        //            source = AppSetting.SMS_source,
        //            mask = AppSetting.SMS_mask
        //        };

        //        path = path + "?" +
        //            "application=" + request.application +
        //            "&password=" + request.password +
        //            "&content=" + request.content +
        //            "&destination=" + request.destination +
        //            "&source=" + request.source +
        //            "&mask=" + request.mask;

        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(path);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(
        //                new MediaTypeWithQualityHeaderValue("application/json"));
        //            var response = await client.GetAsync(new Uri(path));
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var result = await response.Content.ReadAsStringAsync();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return true;
        //}

        //public async Task<bool> SendSms(SMSModel model)
        //{
        //    // Find your Account SID and Auth Token at twilio.com/console
        //    // and set the environment variables. See http://twil.io/secure
        //    //string accountSid = Environment.GetEnvironmentVariable("ACa815467594285aa18c946ad8734f260d");
        //    //string authToken = Environment.GetEnvironmentVariable("35b1c210528bcb48a9455af111862a34");
        //    try
        //    {
        //        string accountSid = "ACa815467594285aa18c946ad8734f260d";
        //        string authToken = "35b1c210528bcb48a9455af111862a34";

        //        TwilioClient.Init(accountSid, authToken);

        //        var message = MessageResource.Create(
        //            body: model.Message,
        //            from: new Twilio.Types.PhoneNumber("+19034598720"),
        //            to: new Twilio.Types.PhoneNumber(model.CountryCode + model.PhoneNumber)
        //        );
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }


        //}
        public async Task<bool> SendSms(SMSModel model)
        {
            try
            {
                var accountSid = AppSetting.AccountSidTwilio;
                var authToken = AppSetting.AuthTokenTwilio;
                TwilioClient.Init(accountSid, authToken);

                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber(model.CountryCode + model.PhoneNumber));
                messageOptions.MessagingServiceSid = AppSetting.MessagingServiceSid.ToString();
                messageOptions.Body = model.Message;

                var message = await MessageResource.CreateAsync(messageOptions);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
