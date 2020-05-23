using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using RestSharp;
using APIClasses;
using System.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient client;
        private DataIntermed temp;  // this field holds data for the currently displayed user
        public MainWindow()
        {
            InitializeComponent();

            temp = new DataIntermed();
            string endpoint = "https://localhost:44385/";
            client = new RestClient(endpoint);

            RestRequest req = new RestRequest("api/values");
            IRestResponse res = client.Get(req);
            NumRecordsTextBox.Content = res.Content;
        }

        // Click handler for search button
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DataIntermed result = new DataIntermed();
            int index;
            bool searchIsIndex = Int32.TryParse(QueryTextBox.Text, out index);
            if(!string.IsNullOrWhiteSpace(QueryTextBox.Text))
            {
                if (searchIsIndex)
                {
                    LockUI();
                    result = await GetValuesFromIndex(index);
                    UnlockUI();
                    StatusLabel.Text = "STATUS: Ready to edit";
                    StatusLabel.Background = new SolidColorBrush(Colors.AliceBlue);
                }
                else
                {
                    LockUI();
                    string query = QueryTextBox.Text;
                    result = await GetValuesFromLName(query);
                    UnlockUI();
                    StatusLabel.Text = "STATUS: Ready to edit";
                    StatusLabel.Background = new SolidColorBrush(Colors.AliceBlue);
                }
            }

            this.temp = result;
            IndexTextBox.Text = result.index.ToString();
            FNameTextBox.Text = result.fName;
            LNameTextBox.Text = result.lName;
            BalanceTextBox.Text = result.bal.ToString();
            AcctNoTextBox.Text = result.acct.ToString();
            PinTextBox.Text = result.pin.ToString("D4");
            if(result.profileImg != null)
                ProfileImage.Source = BitmapToImageSource(result.profileImg);
        }

        // Click handler for Submit changes button
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            LockUI();
            await EditValuesFromIndex();
            UnlockUI();
        }

        // Click handler for Add/Edit Image button
        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("blah");
        }

        // Make a request to the server to find a user based on their index in the database
        private async Task<DataIntermed> GetValuesFromIndex(int index)
        {
            RestRequest req = new RestRequest(String.Format("/api/getall/{0}", index));
            IRestResponse res = await client.ExecuteGetAsync(req);
            DataIntermed result = JsonConvert.DeserializeObject<DataIntermed>(res.Content);

            return result;
        }

        // Make a request to the server to find a user based on last name
        private async Task<DataIntermed> GetValuesFromLName(string query)
        {
            SearchData searchQuery = new SearchData();
            searchQuery.searchStr = query;
            RestRequest req = new RestRequest("api/search");
            req.AddJsonBody(searchQuery);
            IRestResponse res = await client.ExecutePostAsync(req);
            DataIntermed result = JsonConvert.DeserializeObject<DataIntermed>(res.Content);
            return result;
        }

        // Make a request to the server to edit a user's details
        private async Task<IRestResponse> EditValuesFromIndex()
        {
            IRestResponse res = null;
            try
            {
                RestRequest req = new RestRequest(String.Format("/api/edit/{0}", temp.index));
                updateValues();
                req.AddJsonBody(temp);
                res = await client.ExecutePostAsync(req);
                if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    StatusLabel.Text = "ERROR: " + res.Content;
                    StatusLabel.Background = new SolidColorBrush(Colors.Crimson);
                } else
                {
                    StatusLabel.Text = "STATUS: Changes submitted";
                    StatusLabel.Background = new SolidColorBrush(Colors.AliceBlue);
                }
            }
            catch (FormatException e)
            {
                String info = "Field(s) contains invalid characters";
                if (e.Message.Length > 0)
                {
                    info = e.Message;
                }
                StatusLabel.Text = "ERROR: " + info;
                StatusLabel.Background = new SolidColorBrush(Colors.Crimson);
            }
            catch (ArgumentNullException e)
            {
                String info = "Field(s) cannot be left empty";
                if (e.Message.Length > 0)
                {
                    info = e.Message;
                }
                StatusLabel.Text = "ERROR: " + info;
                StatusLabel.Background = new SolidColorBrush(Colors.Crimson);
            }
            return res;
        }

        // Update temp to reflect the currently inputted values
        private void updateValues()
        {
            temp.index = Int32.Parse(IndexTextBox.Text);
            temp.acct = (uint)Int32.Parse(AcctNoTextBox.Text);
            if (Int32.Parse(AcctNoTextBox.Text) < 0)
            {
                throw new FormatException("Account No. cannot be a negative number");
            }
            temp.bal = Int32.Parse(BalanceTextBox.Text);
            if (Int32.Parse(PinTextBox.Text) < 0)
            {
                throw new FormatException("PIN cannot be a negative number");
            }
            if (PinTextBox.Text.Length > 4)
            {
                throw new FormatException("PIN cannot consist of more than 4 numbers");
            }
            temp.pin = (uint)Int32.Parse(PinTextBox.Text);
            if(FNameTextBox.Text.Length < 1)
            {
                throw new ArgumentNullException("First name cannot be empty");
            }
            temp.fName = FNameTextBox.Text;
            if (LNameTextBox.Text.Length < 1)
            {
                throw new ArgumentNullException("Last name cannot be empty");
            }
            temp.lName = LNameTextBox.Text;            
        }

        // Lock the UI (i.e. Show a loading bar, and lock text fields)
        private void LockUI()
        {
            QueryTextBox.IsReadOnly = true;
            AcctNoTextBox.IsReadOnly = true;
            BalanceTextBox.IsReadOnly = true;
            PinTextBox.IsReadOnly = true;
            FNameTextBox.IsReadOnly = true;
            LNameTextBox.IsReadOnly = true;
            StatusLabel.Text = "STATUS: Loading";
            StatusLabel.Background = new SolidColorBrush(Colors.Fuchsia);
            SearchButton.IsEnabled = false;
            EditButton.IsEnabled = false;
            ProgressIndicator.IsIndeterminate = true;
        }

        // Unlock the UI
        private void UnlockUI()
        {
            QueryTextBox.IsReadOnly = false;
            AcctNoTextBox.IsReadOnly = false;
            BalanceTextBox.IsReadOnly = false;
            PinTextBox.IsReadOnly = false;
            FNameTextBox.IsReadOnly = false;
            LNameTextBox.IsReadOnly = false;
            SearchButton.IsEnabled = true;
            EditButton.IsEnabled = true;
            ProgressIndicator.IsIndeterminate = false;
        }

        /*
         *  Converts the Bitmap image into something that can be displayed easily in WPF
            SOURCE(s):
                - https://stackoverflow.com/questions/22499407/how-to-display-a-bitmap-in-a-wpf-image
        */
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

    }
}
