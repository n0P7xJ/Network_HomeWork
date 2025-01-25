using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NewPoshtaProjectCore_1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NovaPoshtaProject.Constant;
using NovaPoshtaProject.Data;
using NovaPoshtaProjectCore_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace NovaPoshtaProjectCore_1.Service
{
    public class ServiceNovaPoshta
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly NovaPoshtaDbContext _context;

        public ServiceNovaPoshta()
        {
            _httpClient = new HttpClient();
            _url = "https://api.novaposhta.ua/v2.0/json/";
            _context = new NovaPoshtaDbContext();
            _context.Database.Migrate();
        }
        public List<Area> GetListAreas()
        {
            return _context.Areas.ToList();
        }
        public async Task SeedAreas()
        {
            if (!_context.Areas.Any())
            {
                var modelRequest = new AreaPostModel
                {
                    ApiKey = Api.ApiNovaPoshta,
                    ModelName = "Address",
                    CalledMethod = "getAreas",
                    MethodProperties = new MethodProperties() { }
                };

                var result = await GetApiResponseAsync<AreaResponse>(modelRequest);

                if (result != null && result.Data != null && result.Success)
                {
                    foreach (var item in result.Data)
                    {
                        var entity = new Area
                        {
                            Ref = item.Ref,
                            AreaName = item.AreasCenter,
                            AreaDescription = item.Description,
                        };
                        _context.Areas.Add(entity);
                    }
                    _context.SaveChanges();
                }
            }
        }
        public async Task SeedCity()
        {
            if (!_context.Cities.Any())
            {
                var modelRequest = new CityPostModel
                {
                    ApiKey = Api.ApiNovaPoshta,
                    ModelName = "Address",
                    CalledMethod = "getCities",
                    MethodProperties = new CityMethodProperties()
                };

                int page = 1;
                bool hasMoreData = true;

                while (hasMoreData)
                {
                    // Додаємо поточну сторінку до запиту
                    modelRequest.MethodProperties.Page = Convert.ToString(page);

                    // Отримуємо відповідь для поточної сторінки
                    var result = await GetApiResponseAsync<CityResponse>(modelRequest);

                    if (result != null && result.Data != null && result.Success)
                    {
                        foreach (var item in result.Data)
                        {
                            var area = _context.Areas.FirstOrDefault(a => a.Ref == item.Area);

                            if (area != null)
                            {
                                var entity = new City
                                {
                                    Ref = item.Ref,
                                    Description = item.Description,
                                    AreaRef = item.Area
                                };

                                _context.Cities.Add(entity);
                                Console.WriteLine($"Added city: {item.Description}");
                            }
                        }

                        // Якщо дані на поточній сторінці менші за максимальний розмір, то це остання сторінка
                        hasMoreData = result.Data.Count == result.PageSize;

                        // Перехід до наступної сторінки
                        page++;
                    }
                    else
                    {
                        Console.WriteLine("Failed to fetch or parse cities data.");
                        hasMoreData = false; // Виходимо з циклу у разі помилки
                    }
                }
                // Зберігаємо всі зміни в базі після обробки всіх сторінок
                _context.SaveChanges();
            }
        }
        public async Task SeedWarehouses()
        {
            if (!_context.Warehouses.Any())
            {
                    var requestModel = new WarehousePostModel
                    {
                        ApiKey = Api.ApiNovaPoshta,
                        ModelName = "Address",
                        CalledMethod = "getWarehouses",
                    };

                    var result = await GetApiResponseAsync<WarehouseResponse>(requestModel);

                    if (result != null && result.Data != null && result.Success)
                    {
                        foreach (var item in result.Data)
                        {
                            var entity = new Warehouse
                            {
                                Ref = item.Ref,
                                Description = item.Description,
                                CityRef = item.CityRef,
                                SettlementType = item.SettlementType,
                                Address = item.Address
                            };

                            _context.Warehouses.Add(entity);
                            Console.WriteLine($"Added warehouse: {item.Description}");
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        Console.WriteLine("Failed to fetch or parse warehouses and postmats data.");
                    }
                }
            }
        
        private async Task<T?> GetApiResponseAsync<T>(object model)
        {
            string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResp = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(jsonResp);
                }
            }
            catch (Exception){}

            return default;
        }
    }
}
