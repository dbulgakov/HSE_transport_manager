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
    public class LoadFromCSV
    {
        public static List<DormitoryData> LoadDormitoryData(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(filename))
            {
                var dormitories = new List<DormitoryData>();
                using (var sr = new StreamReader(stream, Encoding.Default))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var items = line.Split(';');
                        dormitories.Add(
                            new DormitoryData
                            {
                                Name = items[0],
                                Region = items[1],
                                City = items[2],
                                Latitude = double.Parse(items[3]),
                                Longitude = double.Parse(items[4]),
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

        public static List<HSEBuildingData> LoadHSEBuildingData(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(filename))
            {
                var buildings = new List<HSEBuildingData>();
                using (var sr = new StreamReader(stream, Encoding.Default))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var items = line.Split(';');
                        buildings.Add(
                            new HSEBuildingData
                            {
                                Name = items[0],
                                Address = items[1],
                                Latitude = double.Parse(items[2]),
                                Longitude = double.Parse(items[3]),    
                                SubwayStation = items[4].Split(',').ToList()
                            });
                    }
                }
                return buildings;
            }
        }

        public static List<SubwayStationData> LoadSubwayStationData(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(filename))
            {
                var subway = new List<SubwayStationData>();
                using (var sr = new StreamReader(stream, Encoding.Default))
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
                                Latitude = double.Parse(items[1]),
                                Longitude = double.Parse(items[2])
                            });
                    }
                }
                return subway;
            }
        }

        public static List<DubkiBusData> LoadDubkiBusData(string filename)
        {
            int i = 0;
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(filename))
            {
                var dubki = new List<DubkiBusData>();
                using (var sr = new StreamReader(stream, Encoding.Default))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var items = line.Split(';');
                        if (items[0] != "" && items[1] != "" && items[3] != "")
                        {
                            i++;
                            dubki.Add(
                            new DubkiBusData
                            {
                                Trip = i,
                                DepartureTime = DateTime.Parse(items[0]),
                                DayOfWeek = items[2].Split(',').ToList(),
                                From = items[1],
                                To = items[3]
                            });
                        }
                        i++;
                        dubki.Add(
                            new DubkiBusData
                            {
                                Trip=i,
                                DepartureTime = DateTime.Parse(items[4]),
                                DayOfWeek = items[2].Split(',').ToList(),
                                From = items[5],
                                To = items[6]
                            });
                    }
                }
                return dubki;
            }
        }
    }
}
