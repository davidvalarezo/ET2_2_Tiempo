using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace ET2_2_Tiempo
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Consultar_Click(object sender, RoutedEventArgs e)
        {

            var permiso = await Geolocator.RequestAccessAsync();
            if (permiso != GeolocationAccessStatus.Allowed)
            {
                InfoMeteo.Text = "Sin acceso a localización";
            }

            var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
            var position = await geolocator.GetGeopositionAsync();
                                     
            OpenWheatherMapProxy miTiempo = await OpenWheatherMapProxy.RecuperaTiempo(position.Coordinate.Point.Position.Latitude, position.Coordinate.Point.Position.Longitude);
            InfoMeteo.Text = miTiempo.Name + " - " + miTiempo.Main.Temp+ "ºC" + " - " + miTiempo.Weather[0].Description;
            string urlicon;
            if (switchImagen.IsOn)
            {
                urlicon = "http://openweathermap.org/img/wn/" + miTiempo.Weather[0].Icon + "@2x.png";
            }
            else
            {
                urlicon = "ms-appx:///Assests/Weather/" + miTiempo.Weather[0].Icon + ".png";
            }

            Icono.Source = new BitmapImage(new Uri(urlicon, UriKind.Absolute));

            
        }

        private async void RegistrarTileLive_Click(object sender, RoutedEventArgs e)
        {

            
            var permiso = await Windows.Devices.Geolocation.Geolocator.RequestAccessAsync();
            if (permiso != Windows.Devices.Geolocation.GeolocationAccessStatus.Allowed)
            {
                InfoMeteo.Text = "Sin acceso a localización";
            }

            var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
            var position = await geolocator.GetGeopositionAsync();
            //var position = await Geolocator.RequestAccessAsync();
            double lat = position.Coordinate.Point.Position.Latitude;
            double lon = position.Coordinate.Point.Position.Longitude;

            string url = "http://servicioopenweather.azurewebsites.net?lat=" + lat.ToString() + "&long=" + lon.ToString();
            Uri tileContent = new Uri(url);
            var actualizador = TileUpdateManager.CreateTileUpdaterForApplication();
            actualizador.StartPeriodicUpdate(tileContent, PeriodicUpdateRecurrence.HalfHour);


        }
    }
}
