using Firebase.Auth;
using Firebase.Storage;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PayMasta.Service.ThirdParty
{
    public class ThirdPartyService : IThirdParty
    {

        public async Task<string> UploadFiles(string image, string fileName)
        {
            string data = string.Empty;
            string url = string.Empty;
            string Bucket = AppSetting.Bucket;
            var stream = File.Open(image, FileMode.Open);
            var auth = new FirebaseAuthProvider(new FirebaseConfig(AppSetting.FireBaseAPIKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AppSetting.FirebaseUserName, AppSetting.FirebasePassword);

            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
                .Child("profileUploads")
                // .Child("test")
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);
            try
            {
                url = await task;//.TargetUrl.ToString();
            }
            catch (HttpRequestException e)
            {

            }
            return url;
        }
        public async Task<string> PostBankTransaction(string req, string url)
        {
            string resBody = "";
            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    var content = new StringContent(req, Encoding.UTF8, "application/json");
                    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSetting.NinSecretKey);
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    resBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(resBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                return resBody;
            }
        }
        public async Task<string> FundTransaction(string req, string url, string token)
        {
            string resBody = "";
            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    var content = new StringContent(req, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.PostAsync(url, content);
                   // response.EnsureSuccessStatusCode();
                    resBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(resBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                return resBody;
            }
        }
        public async Task<string> CreateProvidusVirtualAccount(string req, string url)
        {
            string resBody = "";
            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    var content = new StringContent(req, Encoding.UTF8, "application/json");
                    // client.DefaultRequestHeaders.Add("Client-Id", AppSetting.ProvidusClientId);// = new AuthenticationHeaderValue("Client-Id", "dGVzdF9Qcm92aWR1cw==");
                    //client.DefaultRequestHeaders.Add("X-Auth-Signature", AppSetting.ProvidusXAuthSignature);//client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-Auth-Signature", "BE09BEE831CF262226B426E39BD1092AF84DC63076D4174FAC78A2261F9A3D6E59744983B8326B69CDF2963FE314DFC89635CFA37A40596508DD6EAAB09402C7");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    // response.EnsureSuccessStatusCode();
                    resBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(resBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                return resBody;
            }
        }

        public async Task<string> GetVirtualAccount(string token, string url)
        {
            string resBody = "";
            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    //  var content = new StringContent(req, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(url);
                    // response.EnsureSuccessStatusCode();
                    resBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(resBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                return resBody;
            }
        }
    }
}
