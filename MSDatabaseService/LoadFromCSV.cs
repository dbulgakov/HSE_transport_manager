using MSDatabaseService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService
{
    class LoadFromCSV
    {
        public List<DormitoryData> LoadDormitoryData(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(filename))
            {
                var dormitories = new List<DormitoryData>();
                using (var sr = new StreamReader(stream))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var items = line.Split(';');
                        dormitories.Add(
                            new DormitoryData
                            {
                                Name = items[0].Split(',').ToList(),
                                Region = items[1],
                                City = items[2],
                                Latitude = decimal.Parse(items[3]),
                                Longitude = decimal.Parse(items[4]),
                                Address = items[5],
                                SubwayStation = items[6].Split(',').ToList(),
                                LocalTrainStation = items[7],
                                ChechDubkiBus = bool.Parse(items[8]),
                                From = items[9].Split(',').ToList(),
                                To = items[10].Split(',').ToList()
                            });
                    }
                }
                return dormitories;
            }
        }

        public List<HSEBuildingData> LoadHSEBuildingData(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(filename))
            {
                var buildings = new List<HSEBuildingData>();
                using (var sr = new StreamReader(stream))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var items = line.Split(';');
                        buildings.Add(
                            new HSEBuildingData
                            {
                                Name = items[0].Split(',').ToList(),
                                Address = items[1],
                                Latitude = decimal.Parse(items[2]),
                                Longitude = decimal.Parse(items[3]),    
                                SubwayStation = items[6].Split(',').ToList()
                            });
                    }
                }
                return buildings;
            }
        }

        public List<SubwayStationData> LoadSubwayStationData(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(filename))
            {
                var subway = new List<SubwayStationData>();
                using (var sr = new StreamReader(stream))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var items = line.Split(';');
                        subway.Add(
                            new SubwayStationData
                            {
                                Name = items[0],
                                Latitude = decimal.Parse(items[1]),
                                Longitude = decimal.Parse(items[2])
                            });
                    }
                }
                return subway;
            }
        }
    }
}
