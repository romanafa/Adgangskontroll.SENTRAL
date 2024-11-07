using Adgangskontroll.SENTRAL.Models;
using Adgangskontroll.SENTRAL.Repository;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adgangskontroll.SENTRAL.TCP_kortleser
{
    internal class TCP
    {
        // VelgerTCP/IP og adresser + portnummer
        Socket lytteSokkel = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Oppgir server sin IP-adresse og portnummer
        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

        bool harforbindelse = true;

        BrukerRepository db = new BrukerRepository();

        //static string kort_ID; //må sette inn riktige variabler
        //static string kortleser_ID;
        //static string startTid;
        //static string sluttTid;



        public void Start()
        {
            // Kobler til server
            lytteSokkel.Connect(serverEP);

            // Sender data til server
            byte[] data = Encoding.ASCII.GetBytes("Kortleser koblet til server");
            lytteSokkel.Send(data);

            // Mottar data fra server
            byte[] mottattData = new byte[1024];
            int antallBytes = lytteSokkel.Receive(mottattData);
            string mottattTekst = Encoding.ASCII.GetString(mottattData, 0, antallBytes);
            Console.WriteLine("Mottatt fra server: " + mottattTekst);

            // Lukker forbindelsen
            lytteSokkel.Shutdown(SocketShutdown.Both);
            lytteSokkel.Close();
        }
        public void Klientkommunikasjon(object o)
        {
            Socket kommSokkel = o as Socket;
            IPEndPoint klientEP = (IPEndPoint)kommSokkel.RemoteEndPoint;

            byte[] data = new byte[1024];
            string dataFraKortleser;
            string dataTilKortleser;

            bool harForbindelse = true;

            while (harForbindelse)
            {
                dataFraKortleser = MottaData(kommSokkel, out harForbindelse);

                if (harForbindelse)
                {
                    if (dataFraKortleser.Length == 20)  // String med denne lengden vil alltid være kort-ID, pin og kortleser-ID
                    {
                        // Deler opp tekststrengen vi mottar for å hente ut de ulike verdiene

                        int indeksKort = dataFraKortleser.IndexOf('K');
                        int indeksPin = dataFraKortleser.IndexOf('P');
                        int indeksLeser = dataFraKortleser.IndexOf('L');
                        string Kort_ID = dataFraKortleser.Substring(indeksKort + 2, 4);
                        string pin = dataFraKortleser.Substring(indeksPin + 2, 4);
                        string kortleser_id = dataFraKortleser.Substring(indeksLeser + 2, 4);

                        dataTilKortleser = db.Autentisering(Kort_ID, pin, kortleser_id); // må sette inn riktig metode

                        if (dataTilKortleser == "Godkjent") db.LeggTilLogg(0, kortleser_id, Kort_ID);   //Loggfører godkjent
                        else db.LeggTilLogg(1, kortleser_id, Kort_ID);  //loggfører ikke godkjent // må sette inn riktig metode
                    }
                    else if (dataFraKortleser.Length == 14)     // String med denne lengden vil alltid være kort-ID og kortleser-ID
                    {
                        // Deler opp tekststrengen vi mottar for å hente ut de ulike verdiene

                        int indeksKort = dataFraKortleser.IndexOf('K');
                        int indeksLeser = dataFraKortleser.IndexOf('L');
                        string kort_ID = dataFraKortleser.Substring(indeksKort + 2, 4);
                        string kortleser_ID = dataFraKortleser.Substring(indeksLeser + 2, 4);

                        db.LeggTilLogg(2, kortleser_ID, kort_ID);   // Loggfører at døren er åpent // må sette inn riktig metode
                        dataTilKortleser = "trash";                 // Blir ikke brukt til noe, har ikke behov for melding til bake til kortleser
                    }
                    else if (dataFraKortleser.Length == 17)     // alarm
                    {
                        // Deler opp tekststrengen vi mottar for å hente ut de ulike verdiene

                        int indeksKort = dataFraKortleser.IndexOf('K');
                        int indeksLeser = dataFraKortleser.IndexOf('L');
                        int indeksAlarm = dataFraKortleser.IndexOf('A');
                        string kort_ID = dataFraKortleser.Substring(indeksKort + 2, 4);
                        string kortleser_ID = dataFraKortleser.Substring(indeksLeser + 2, 4);
                        int alarm = Convert.ToInt32(dataFraKortleser.Substring(indeksAlarm + 2, 1));

                        db.LeggTilLogg(alarm, kortleser_ID, kort_ID);   // Loggfører alarm utløst // må sette inn riktig metode
                        dataTilKortleser = "trash";                     // Blir ikke brukt til noe, har ikke behov for melding til bake til kortleser   

                        Console.WriteLine($"Alarmtype {alarm} utløst!\nDør: {kortleser_ID}, Bruker: {kort_ID}");  // Viser melding i sentral om aktiv alarm
                    }
                    //else if (dataFraKortleser == "RequestID")
                    //{
                    //    dataTilKortleser = ;  // Sender kortleser-ID tilbake til kortleser, må implementere metode for å hente kortleser-ID
                    //}
                    else dataTilKortleser = "Retur: " + dataFraKortleser;           // Blir ikke brukt til annet enn testing

                    SendData(kommSokkel, dataTilKortleser, out harForbindelse);     // Sender aktuell data som har blitt generert ut ifra hvilken hendelse som har skjedd
                }
            }
            kommSokkel.Close();     // Lukker tilkoblingen når vi stenger en kortleser
        }

        // Metoder for å motta og sende data fra sentral og kortleser
        static string MottaData(Socket s, out bool gjennomført)
        {
            string svar = "";
            try
            {
                byte[] dataSomBytes = new byte[1024];
                int recv = s.Receive(dataSomBytes);
                if (recv > 0)
                {
                    svar = Encoding.ASCII.GetString(dataSomBytes, 0, recv);
                    gjennomført = true;
                }
                else
                    gjennomført = false;
            }
            catch (Exception)
            {
                throw;
            }
            return svar;
        }
        static void SendData(Socket s, string data, out bool gjennomført)
        {
            try
            {
                byte[] dataSomBytes = Encoding.ASCII.GetBytes(data);
                s.Send(dataSomBytes, dataSomBytes.Length, SocketFlags.None);
                gjennomført = true;
            }
            catch (Exception)
            {
                gjennomført = false;
            }
        }



        // Metode for å hente data fra database
        // Gjør det slik at denne hendelsen kun endrer på writable til feltene; lag en ny switch-case
        //private void CB_Alarmtype_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    dataGridView1.ClearSelection();
        //    TB_FraDato.Clear();
        //    TB_TilDato.Clear();
        //    TB_KortID.Clear();
        //    TB_KortleserID.Clear();

        //    switch (CB_Alarmtype.Text)
        //    {
        //        case "Alle kortlesere":
        //            TB_KortID.Enabled = false;
        //            TB_FraDato.Enabled = false;
        //            TB_TilDato.Enabled = false;
        //            TB_KortleserID.Enabled = false;
        //            break;
        //        case "Alle brukere":
        //            TB_KortID.Enabled = false;
        //            TB_FraDato.Enabled = false;
        //            TB_TilDato.Enabled = false;
        //            TB_KortleserID.Enabled = false;
        //            break;
        //        case "Alle adgangsforsøk for bruker i periode:":
        //            TB_KortID.Enabled = true;
        //            TB_FraDato.Enabled = true;
        //            TB_TilDato.Enabled = true;
        //            TB_KortleserID.Enabled = false;
        //            break;
        //        case "Alle ikke-godkjente adgangsforsøk for kortleser i periode:":
        //            TB_KortID.Enabled = false;
        //            TB_FraDato.Enabled = true;
        //            TB_TilDato.Enabled = true;
        //            TB_KortleserID.Enabled = true;
        //            break;
        //        case "Alle alarmer":
        //            TB_KortID.Enabled = false;
        //            TB_FraDato.Enabled = false;
        //            TB_TilDato.Enabled = false;
        //            TB_KortleserID.Enabled = false;
        //            break;
        //        case "Alle alarmer knyttet til bruker:":
        //            TB_KortID.Enabled = true;
        //            TB_FraDato.Enabled = false;
        //            TB_TilDato.Enabled = false;
        //            TB_KortleserID.Enabled = false;
        //            break;
        //        case "Alle alarmer knyttet til kortleser:":
        //            TB_KortID.Enabled = false;
        //            TB_FraDato.Enabled = false;
        //            TB_TilDato.Enabled = false;
        //            TB_KortleserID.Enabled = true;
        //            break;
        //        case "Alle alarmer i periode:":
        //            TB_KortID.Enabled = false;
        //            TB_FraDato.Enabled = true;
        //            TB_TilDato.Enabled = true;
        //            TB_KortleserID.Enabled = false;
        //            break;
        //        case "Alle logger":
        //            TB_KortID.Enabled = false;
        //            TB_FraDato.Enabled = false;
        //            TB_TilDato.Enabled = false;
        //            TB_KortleserID.Enabled = false;
        //            break;
        //        case "Alle logger kyttet til bruker:":
        //            TB_KortID.Enabled = true;
        //            TB_FraDato.Enabled = false;
        //            TB_TilDato.Enabled = false;
        //            TB_KortleserID.Enabled = false;
        //            break;
        //        case "Alle logger knyttet til kortleser:":
        //            TB_KortID.Enabled = false;
        //            TB_FraDato.Enabled = false;
        //            TB_TilDato.Enabled = false;
        //            TB_KortleserID.Enabled = true;
        //            break;
        //    }
        //}

    }
}
