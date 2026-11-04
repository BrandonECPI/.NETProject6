using MAUI_Data_Access.Models;
using Newtonsoft.Json;

namespace MAUI_Data_Access.DataAccess
{
    /// <summary>
    /// Data access layer: gets and saves people through the REST web API
    /// instead of the local SQLite database.
    /// </summary>
    public class PersonData
    {
        // Web API address (change the port to match your running API)
        private const string ApiUrl = "http://localhost:5046/api/Person";

        // Gets every person from the web API
        public async Task<List<Person>> GetPeopleAsync()
        {
            HttpClient client;

            try
            {
                client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                List<Person> people = new List<Person>();

                var response = await client.GetAsync(ApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        people = JsonConvert.DeserializeObject<List<Person>>(content) ?? new List<Person>();
                    }
                }

                return people;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Sends a new person to the web API to be saved
        public async Task<int> SavePersonAsync(Person person)
        {
            HttpClient client;

            try
            {
                client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");

                // Turn the person into JSON, then into bytes to send
                var content = JsonConvert.SerializeObject(person);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync(ApiUrl, byteContent);
                return response.IsSuccessStatusCode ? 1 : 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}