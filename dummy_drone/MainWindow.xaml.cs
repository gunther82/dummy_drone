using NadLibrary;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Threading;

namespace dummy_drone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        DispatcherTimer timer = new DispatcherTimer();
        NadSystem nad = new NadSystem();
        
        List<string> remoteIp = null;
        double latitude_f = 43.719259015524976;
        double longitude_f = 10.421048893480233;
        double latitude_up;
        double longitude_up;
        int altitude;
        double a = (Math.PI / 180.0) * 45.0;
        Random r = new Random();

        public void initializeTimer() 
        {
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {

            double lat0 = Math.Cos(Math.PI / 180.0 * latitude_f);
            
            latitude_up = latitude_f + (180.0 / Math.PI) * (10.0 / 6378137.0) * Math.Sin(a);
            longitude_up = longitude_f + (180 / Math.PI) * (10.0 / 6378137.0) / Math.Cos(lat0) * Math.Cos(a);
            altitude = r.Next(10, 60);

            latitude_f = latitude_up;
            longitude_f = longitude_up;
            showmsg();
            nad.NADPopulateListCoordinate(latitude_f.ToString(), longitude_f.ToString(), altitude.ToString());

            //nad.NADUpdateShipCoord(latitude_f.ToString(), longitude_f.ToString());

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            nad.NADOpenConnection(IPTextBox.Text, PortTextBox.Text);
            nad.NADPopulateListCoordinate("43.71967438461098", "10.420797021964137", "40");
            nad.NADPopulateListCoordinate("43.71968213866057", "10.421413929976033", "30");
            nad.NADPopulateListCoordinate("43.72014350280539", "10.4228730515346", "20");
            nad.NADPopulateListCoordinate("43.720825039296045", "10.424706602137107", "20");
            nad.NADPopulateListCoordinate("43.72266628185771", "10.422610492949717", "20");
            //initializeTimer();
            //showmsg();
            //Thread thread = new Thread(showmsg);
            //thread.IsBackground = true;
            //thread.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            //showmsg(MSGTextBox.Text);
            //nad.Send_tcp_msg(MSGTextBox.Text + "\r\n");
            //nad.NADGetDroneStatus();
            nad.NADCloseConnection();
            
        }

        private void btnSend_ConfigWP(object sender, RoutedEventArgs e) 
        {
            //showmsg(MSGSetSpeed.Text);
            //nad.NADUpdateShip(MSGSetSpeed.Text);
            nad.NADSetSpeed(MSGSetSpeed.Text);
            
        }

        private void btnSend_CreateWP(object sender, RoutedEventArgs e)
        {
            //nad.NADSendCoordinate(SetLatitudeWP.Text, SetLongitudeWP.Text, SetAltitudeWP.Text);
            //nad.NADSendListCoord();
        }

        private void btnSend_UploadWP(object sender, RoutedEventArgs e)
        {
            //nad.NADUploadMobMission();
        }

        private void btnSend_startWP(object sender, RoutedEventArgs e)
        {
            nad.NADStartMobMission();
            
      
        }

        private void btnGet_Info(object sender, RoutedEventArgs e)
        {
            string msg = nad.NADGetDroneStatus();
            System.Diagnostics.Trace.WriteLine("Message from client " + msg);
        }

        private void btnSend_pauseWP(object sender, RoutedEventArgs e)
        {
            nad.NADPauseMobMission();
        }

        private void btnSend_resumeWP(object sender, RoutedEventArgs e)
        {
            nad.NADResumeMobMission();
        }

        private void btnSend_stopWP(object sender, RoutedEventArgs e)
        {
            nad.NADStopMobMission();
        }

        private void btnSend_delWP(object sender, RoutedEventArgs e)
        {
            nad.NADDelPos();
        }

        private void btnSend_startHP(object sender, RoutedEventArgs e)
        {
            nad.NADSearchAtPos(SetLatitudeHP.Text, SetLongitudeHP.Text, SetAltitudeHP.Text, SetRadiusHP.Text);
        }

        private void btnSend_warning(object sender, RoutedEventArgs e)
        {
            nad.NADSendWarning("Battery low");
        }

        private void btnSend_setInterdictionRadius(object sender, RoutedEventArgs e)
        {
            nad.NADSetInterdictionArea(SetInterdictionRadius.Text);
        }

        //private void btnSend_stopHP(object sender, RoutedEventArgs e)
        //{
        //    nad.NADStopSearchAt();
        //}

        private void btnSend_startFM(object sender, RoutedEventArgs e) 
        {
            
            latitude_f = double.Parse(SetLatitudeFM.Text, System.Globalization.CultureInfo.InvariantCulture);
            longitude_f = double.Parse(SetLongitudeFM.Text, System.Globalization.CultureInfo.InvariantCulture);
            System.Diagnostics.Trace.WriteLine("Longitude init " + longitude_f);
            System.Diagnostics.Trace.WriteLine("Latitude init " + latitude_f);
            nad.NADFollowShip(SetLatitudeFM.Text, SetLongitudeFM.Text, SetAltitudeFM.Text);
            initializeTimer();
        }

        private void btnSend_stopFM(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            nad.NADStopFollowShip();
        }


        private void showmsg()
        {
            string p = nad.GetMSG();
            System.Diagnostics.Trace.WriteLine("Message from client " + p);
            if (string.IsNullOrEmpty(p)) 
            {
                p = "Nessun messaggio";
            }
            System.Diagnostics.Trace.WriteLine("Message from client " + p);
          
       
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //rtbx.AppendText(p + "\r\n");
                MSGTextBox.Text = p;
             }));
            

        }
    }
}