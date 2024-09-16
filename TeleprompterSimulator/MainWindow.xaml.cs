using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Threading;

namespace TeleprompterSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer scrollTimer;
        private double scrollSpeed = 1.0; // scroll-hastigheden
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["TeleprompterDB"].ConnectionString;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string connectionString = GetConnectionString();
            string query = "SELECT TeleprompterText FROM YourTable WHERE Id = 1"; // Laves når DB er klar....         
            // string teleprompterText = await GetTextFromDatabaseAsync(connectionString, query);

            string teleprompterText = "Dansk Boldspil-Union (DBU) drømmer om et nationalstadion med plads til 50.000 tilskuere.\n\n" +
                                       "Det har unionen faktisk drømt om helt siden 2019, men det har indtil nu ikke været muligt at finde midler og opbakning til det.\n\n" +
                                       "Spørger man regeringens kulturordførere, er det dog ingen dum idé at udvide Parken for at få plads til langt flere rødhvide fodboldfans.\n\n" +
                                       "DBU og Parken er uenige om fremtidens nationalstadion: 'Det er altså ikke bare så enkelt'\n\n" +
                                       "Mogens Jensen (S), Jan E. Jørgensen (V) og Tobias Grotkjær Elmstrøm (M), fortæller til Berlingske, at de alle er åbne for en udvidelse af Parken, og at det måske endda kan ske via statslig støtte.\n\n" +
                                       "- Defineres Parken som vores \"nationalstadion\" på samme måde som Det Kongelige Teater, synes jeg ikke, at et statsligt engagement kan udelukkes, siger Mogens Jensen til Berlingske.\n\n";

           
            TeleprompterText.Text = teleprompterText;
            StartAutoScrolling();
        }

        // autoscroll er bare en pseudo funktion, for at få det til at se sjovt ud, at teksten flytter sig.
        // Teleprompteren styres af studievært og/eller producer i et rigtigt TV-studie.
        private void StartAutoScrolling()
        {
            scrollTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20) // Tidsinterval mellem hver scrollopdatering
            };
            scrollTimer.Tick += ScrollTimer_Tick;
            scrollTimer.Start();
        }

        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            // Få den aktuelle offset og tilføj scrollSpeed
            double currentOffset = scrollViewer.VerticalOffset;

            // Hvis vi ikke er nået til bunden, fortsæt med at scrolle
            if (currentOffset < scrollViewer.ScrollableHeight)
            {
                scrollViewer.ScrollToVerticalOffset(currentOffset + scrollSpeed); // Scroll lidt længere ned
            }
            else
            {
                // Hvis vi har nået bunden, start forfra
                scrollViewer.ScrollToVerticalOffset(0);
            }
        }

        private async Task<string> GetTextFromDatabaseAsync(string connectionString, string query)
        {
            string result = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            result = reader["TeleprompterText"].ToString();
                        }
                    }
                }
            }

            return result;
        }
    }
}