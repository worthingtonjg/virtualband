using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace VirtualBand
{
    public class BandHelper
    {
        private IBandInfo[] pairedBands;
        private IBandClient bandClient;
        private UnityHelper unityHelper;

        public BandHelper(UnityHelper unityHelper)
        {
            this.unityHelper = unityHelper;
        }
        
        public async System.Threading.Tasks.Task<int> findBands()
        {
            int bandCount = 0;

            try
            {
                System.Diagnostics.Debug.WriteLine("Searching for Microsoft Bands");

                // Get the list of Microsoft Bands paired to the host device
                pairedBands = await BandClientManager.Instance.GetBandsAsync();
                bandCount = pairedBands.Length;
                if (pairedBands.Length < 1)
                {
                    System.Diagnostics.Debug.WriteLine("Can't find a Microsoft Band paired to this device");
                    unityHelper.SendMessage("FINDBANDS_ERROR1", null);
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Error communicating with the band, please move closer and remove the aluminium foil hulk hands");
                unityHelper.SendMessage("FINDBANDS_ERROR2", null);
            }

            System.Diagnostics.Debug.WriteLine(bandCount + " Bands found");
            unityHelper.SendMessage("FINDBANDS_SUCCESS", null);
            return bandCount;
        }

        public async Task<bool> connectBands()
        {
            try
            {
                bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Error communicating with the band, please stay close to the host device");
            }
            System.Diagnostics.Debug.WriteLine("Communicating with band");
            return true;
        }

        public async Task<bool> subscribeBandData()
        {
            try
            {
                if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    await bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
                }

                int samplesReceivedAcc = 0; // the number of Accelerometer samples received 
                int samplesReceivedGyro = 0; // the number of Gyroscope samples received 
                                             //double wait_time = 1;
                                             // Subscribe to Accelerometer data. 
                                             //bandClient.SensorManager.Accelerometer.ReportingInterval
                bandClient.SensorManager.Accelerometer.ReadingChanged += (s, args) =>
                {
                    samplesReceivedAcc++;
                    if ((samplesReceivedAcc % 20) == 0)
                    {
                        // Only report occasional Accelerometer data 
                        IBandAccelerometerReading readings = args.SensorReading;
                        CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            //if (Math.Abs(readings.AccelerationX) > 1 || Math.Abs(readings.AccelerationY) > 1 || Math.Abs(readings.AccelerationZ) > 1)
                            //{
                            //    unityHelper.SendMessage("MOVING", true);
                            //}
                            //else if (Math.Abs(readings.AccelerationX) < 1 && Math.Abs(readings.AccelerationY) < 1 && Math.Abs(readings.AccelerationZ) < 1)
                            //{
                            //    unityHelper.SendMessage("MOVING", false);
                            //}


                            //System.Diagnostics.Debug.WriteLine(readings.AccelerationX.ToString() + " " + readings.AccelerationY.ToString() + " " + readings.AccelerationZ.ToString());
                        });
                    }
                };
                await bandClient.SensorManager.Accelerometer.StartReadingsAsync();

                // Subscribe to Calories data. 
                bandClient.SensorManager.Calories.ReadingChanged += (s, args) =>
                {
                    IBandCaloriesReading readings = args.SensorReading;
                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        unityHelper.SendMessage("CALORIES_CHANGED", readings.Calories);
                    });
                };
                await bandClient.SensorManager.Calories.StartReadingsAsync();

                // Subscribe to Contact data. 
                bandClient.SensorManager.Contact.ReadingChanged += (s, args) =>
                {
                    IBandContactReading readings = args.SensorReading;
                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        //readings.State.ToString();
                    });
                };
                await bandClient.SensorManager.Contact.StartReadingsAsync();

                // Subscribe to Distance data. 
                bandClient.SensorManager.Distance.ReadingChanged += (s, args) =>
                {
                    IBandDistanceReading readings = args.SensorReading;
                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        unityHelper.SendMessage("DISTANCE_CHANGED", new SensorData
                            {
                                DistanceToday = readings.DistanceToday,
                                Pace = readings.Pace,
                                Speed = readings.Speed,
                                TotalDistance = readings.TotalDistance
                            });

                        System.Diagnostics.Debug.WriteLine(readings.CurrentMotion.ToString() + " " + readings.Pace.ToString() + " " + readings.Speed.ToString() + " " + readings.TotalDistance.ToString());
                    });
                };
                await bandClient.SensorManager.Distance.StartReadingsAsync();

                //// Subscribe to Gyroscope data. 
                //bandClient.SensorManager.Gyroscope.ReadingChanged += (s, args) =>
                //{
                //    samplesReceivedGyro++;
                //    if ((samplesReceivedGyro % 20) == 0)
                //    {
                //        // Only report occasional Gyroscope data 
                //        IBandGyroscopeReading readings = args.SensorReading;
                //        CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                //        {
                //            //readings.AccelerationX.ToString() + " " + readings.AccelerationY.ToString() + " " + readings.AccelerationX.ToString();
                //        });
                //    }
                //};
                //await bandClient.SensorManager.Gyroscope.StartReadingsAsync();

                // Subscribe to HeartRate data. 
                bandClient.SensorManager.HeartRate.ReadingChanged += (s, args) =>
                {
                    IBandHeartRateReading readings = args.SensorReading;
                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {

                        unityHelper.SendMessage("HEARTRATE_CHANGED", readings.HeartRate);

                        //readings.HeartRate.ToString() + " [" + readings.Quality.ToString() + "]";
                    });
                };
                await bandClient.SensorManager.HeartRate.StartReadingsAsync();

                // Subscribe to Pedometer data. 
                bandClient.SensorManager.Pedometer.ReadingChanged += (s, args) =>
                {
                    IBandPedometerReading readings = args.SensorReading;
                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        //readings.TotalSteps.ToString();
                    });
                };
                await bandClient.SensorManager.Pedometer.StartReadingsAsync();

                // Subscribe to SkinTemperature data. 
                bandClient.SensorManager.SkinTemperature.ReadingChanged += (s, args) =>
                {
                    IBandSkinTemperatureReading readings = args.SensorReading;
                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        //readings.Temperature.ToString();
                    });
                };
                await bandClient.SensorManager.SkinTemperature.StartReadingsAsync();

                // Subscribe to UV data. 
                bandClient.SensorManager.UV.ReadingChanged += (s, args) =>
                {
                    IBandUVReading readings = args.SensorReading;
                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        //readings.IndexLevel.ToString();
                    });
                };
                await bandClient.SensorManager.UV.StartReadingsAsync();

                System.Diagnostics.Debug.WriteLine("Gathering data from band");
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Unable to gather data from band, features are limited for the undead");
            }
            return true;
        }

        public async Task<bool> closeBands()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Stoping all Microsoft Band subscriptions");

                await bandClient.SensorManager.Accelerometer.StopReadingsAsync();
                await bandClient.SensorManager.Calories.StopReadingsAsync();
                await bandClient.SensorManager.Contact.StopReadingsAsync();
                await bandClient.SensorManager.Distance.StopReadingsAsync();
                await bandClient.SensorManager.Gyroscope.StopReadingsAsync();
                await bandClient.SensorManager.HeartRate.StopReadingsAsync();
                await bandClient.SensorManager.Pedometer.StopReadingsAsync();
                await bandClient.SensorManager.SkinTemperature.StopReadingsAsync();
                await bandClient.SensorManager.UV.StopReadingsAsync();

                System.Diagnostics.Debug.WriteLine("No longer collecting data");
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Unable to stop reading the band, seriously...i can't stop, it's not funny");
                return false;
            }

            return true;
        }

    }
}
