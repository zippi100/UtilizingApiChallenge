using Coding_Challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Coding_Challenge
{

    // Main Program
    class Program
    {

        /// <summary>
        /// Gloabl Variables For Main
        /// </summary>
        /// 
        static HttpClient client = new HttpClient();

        // DealerResponse variables for path extention and dealerResponse Get response storage
        static string dealerExtention;
        static DealerResponse dealerResponse;

        // Vehicle Response data
        static VehicleIdsResponse vehicleIdsResponse;
        

        // Answer storage for each GET
        static DealerAnswer dealerAnswer = null;
        static Answer answer = new Answer();
        static string answerExtention;
        static AnswerResponse answerResponse;

        // Stores all Dealrs wiith Dealer Answers
        static Dictionary<int, DealerAnswer> dealers = new Dictionary<int, DealerAnswer>();
        
        // DataSetId storage
        static DataSetId dataSetId;

        // Task used for parallel programming
        static Task[] vehicalAnswerTask;
        static Task[] dealerAnswerTask;

        static int counter  = 0;
        static int counter2 = 0;

        /// <summary>
        /// Main
        ///</summary>
        ///
        static async Task Main(string[] args)
        {
            // Update port # and initialize base path source
            client.BaseAddress = new Uri("http://CompanyNameHere");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // DataSetId variables for path extention and dataSetId storage
            string baseExtention = @"/api/datasetId";
            
            // DataSetId dataSetId;
            dataSetId = await GetDataSetIdAsync(baseExtention);

            // VehicleId variables for path extention and vehicleId Get response storage
            string vehicleIdExtention = $"/api/{dataSetId.DatasetId}/vehicles";
            vehicleIdsResponse = await GetVehicleIdsAsync(vehicleIdExtention);

            // Task Arrays - used to have conncurrent task GET calls
            vehicalAnswerTask = new Task[vehicleIdsResponse.VehicleIds.Count];
            dealerAnswerTask  = new Task[vehicleIdsResponse.VehicleIds.Count];


            // For each vehicleId in vehicleIdResponse GET
            foreach (int v in vehicleIdsResponse.VehicleIds)
            {
                vehicalAnswerTask[counter] = Task.Run(() => TaskMethodVehicles(v));
                ++counter;
            }

           
            Task.WaitAll(vehicalAnswerTask);

            // POST answer
            answerExtention = $"/api/{dataSetId.DatasetId}/answer";
            answerResponse = await PostAnswerAsync(answerExtention, answer);
            Console.WriteLine("Success:{0}  Message:{1}  TotalMilliseconds:{2}",  answerResponse.Success, answerResponse.Message, answerResponse.TotalMilliseconds);
        }

        // Thread/Task used to GET Vehicles
        static async Task TaskMethodVehicles(int v)
        {
            // VehicleReponse variables for path extention and vehicleResponse Get response storage
            string vehiclesExtention = $"/api/{dataSetId.DatasetId}/vehicles/{v}";
            VehicleResponse vehicleResponse = await GetVehiclesAsync(vehiclesExtention);
            VehicleAnswer vehicleAnswer = new VehicleAnswer(vehicleResponse.VehicleId, vehicleResponse.Year,
                                             vehicleResponse.Make, vehicleResponse.Model);

            dealerAnswerTask[counter2] = Task.Run(() => TaskMethodVehicleToDealers(vehicleAnswer, vehicleResponse));
            dealerAnswerTask[counter2].Wait();
            ++counter2;
        }


        // Thread/Task used to GET dealers
        static async Task TaskMethodVehicleToDealers(VehicleAnswer vehicleAns, VehicleResponse vehicleResponse)
        {
            int dealerId = vehicleResponse.DealerId;
           
            dealerExtention = $"/api/{dataSetId.DatasetId}/dealers/{dealerId}";
            dealerResponse = await GetDealerAsync(dealerExtention);

            if (!dealers.ContainsKey(dealerResponse.DealerId))
            {
                dealerAnswer = new DealerAnswer(dealerResponse.DealerId, dealerResponse.Name);
                dealers[dealerResponse.DealerId] = dealerAnswer;
                answer.Dealers.Add(dealers[dealerResponse.DealerId]);

            }

            dealers[dealerResponse.DealerId].Vehicles.Add(vehicleAns);

        }

        // Function - POST Answer to server. return Reponse from POST
        static async Task<AnswerResponse> PostAnswerAsync(string path, Answer answer)
        {
            AnswerResponse answerResponse = new AnswerResponse();
            HttpResponseMessage response = new HttpResponseMessage();

            // HTTP POST  
            response = await client.PostAsJsonAsync(path, answer).ConfigureAwait(false);

            // Verification  
            if (response.IsSuccessStatusCode)
            {
                // Reading Response.  
                string result = response.Content.ReadAsStringAsync().Result;
                answerResponse = JsonConvert.DeserializeObject<AnswerResponse>(result);
            }
         
            return answerResponse; 
        }


        // Function - GET DealerResponse from server. return Reponse from GET
        static async Task<DealerResponse> GetDealerAsync(string path)
        {
            DealerResponse dealerResponse = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                dealerResponse = await response.Content.ReadAsAsync<DealerResponse>();
            }
            return dealerResponse;
        }

        // Function - GET VehicleResponse from server. return Reponse from GET
        static async Task<VehicleResponse> GetVehiclesAsync(string path)
        {
            VehicleResponse vehicleResponse = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                vehicleResponse = await response.Content.ReadAsAsync<VehicleResponse>();
            }
            return vehicleResponse;
        }

        // Function - GET DataSetId from server. return Reponse from GET
        static async Task<DataSetId> GetDataSetIdAsync(string path)
        {
            DataSetId dataSetId = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                dataSetId = await response.Content.ReadAsAsync<DataSetId>();
            }
            return dataSetId;
        }

        // Function - GET VehicleIdResponse from server. return Reponse from GET
        static async Task<VehicleIdsResponse> GetVehicleIdsAsync(string path)
        {
            VehicleIdsResponse vehicleIdsResponse = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                vehicleIdsResponse = await response.Content.ReadAsAsync<VehicleIdsResponse>();
            }
            return vehicleIdsResponse;
        }


    }

    // Original Code Commented out with Success in 47 seconds

    //foreach (int v in vehicleIdsResponse.VehicleIds)
    //{

    //    vehiclesExtention = $"/api/{dataSetId.DatasetId}/vehicles/{v}";
    //    vehicleResponse = await GetVehiclesAsync(vehiclesExtention);
    //    vehicleAnswer = new VehicleAnswer(vehicleResponse.VehicleId, vehicleResponse.Year,
    //                                     vehicleResponse.Make, vehicleResponse.Model);

    //    dealerExtention = $"/api/{dataSetId.DatasetId}/dealers/{vehicleResponse.DealerId}";
    //    dealerResponse = await GetDealerAsync(dealerExtention);

    //    if (!dealers.ContainsKey(dealerResponse.DealerId))
    //    {
    //        dealerAnswer = new DealerAnswer(dealerResponse.DealerId, dealerResponse.Name);
    //        dealers[dealerResponse.DealerId] = dealerAnswer;
    //        answer.Dealers.Add(dealers[dealerResponse.DealerId]);

    //    }

    //    dealers[dealerResponse.DealerId].Vehicles.Add(vehicleAnswer);


    //}

}
