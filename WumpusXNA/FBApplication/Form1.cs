using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facebook;
using System.Reflection;
using System.Net;
namespace FBApplication
{
    public partial class Form1 : Form
    {
        private const string AppId = "306443312846133";
        private readonly Uri _loginUrl;
        public FacebookOAuthResult _FacebookOAuthResult { get; private set; }
        private const string _ExtendedPermissions = "user_about_me,publish_stream,offline_access,publish_actions";

        public Form1()
        {
            if (string.IsNullOrEmpty(AppId))
                throw new ArgumentNullException("appId");
            var fbClient = new FacebookClient();

            IDictionary<string, object> _loginParameters = new Dictionary<string, object>();

            _loginParameters["client_id"] = AppId;
            _loginParameters["redirect_uri"] = @"https://www.facebook.com/connect/login_success.html";
            _loginParameters["response_type"] = "token";
            //_loginParameters["display"] = "popup";
            if (!string.IsNullOrEmpty(_ExtendedPermissions))
            {
                _loginParameters["scope"] = _ExtendedPermissions;
            }
            _loginUrl = fbClient.GetLoginUrl(_loginParameters);
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(_loginUrl.AbsoluteUri);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            FacebookOAuthResult _oauthResult = null;
            var fbClient = new FacebookClient();

            
            if (fbClient.TryParseOAuthCallbackUrl(e.Url, out _oauthResult))
            {
                this._FacebookOAuthResult = _oauthResult;
                this.DialogResult = _FacebookOAuthResult.IsSuccess ? DialogResult.OK : DialogResult.No;
                try
                {
                    var fb = new FacebookClient(_FacebookOAuthResult.AccessToken);
                    var result = (IDictionary<string, object>)fb.Get("me");
                    var id = (string)result["id"];
                    string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}", id, "square");
                    MDIParent1 frmmdi;
                    frmmdi = (MDIParent1)this.MdiParent;
                    frmmdi.pictureBox1.LoadAsync(profilePictureUrl);
                    try
                    {
                        Type tresult = result.GetType();
                        var objname = (string)result["name"];
                        var firstName = (string)result["first_name"];
                        var lastName = (string)result["last_name"];

                        frmmdi.textBox1.Text = id;
                        frmmdi.textBox2.Text = firstName;
                        frmmdi.textBox3.Text = lastName;
                        frmmdi.label2.Text = "Successfully connected";

                        // -------- START NEW CODE ----------

                        // This is the URL that the facebook documentation says we should use. 
                        // Notice that it contains /me/feed in there as well.  Basically, 
                        // everything that comes before the '?'.
                        var postUrl = string.Format(@"https://graph.facebook.com/me/feed");
                        var request = HttpWebRequest.Create(postUrl);

                        // The data that needs to be posted is everything that comes after the '?'
                        // In this case, there are only 2 parameters that we need to pass: the 
                        // message we want to post (for you the high score), and the access_token, 
                        // which we obtained during the authentication step. Remember, you can
                        // use the string.Format() method to format a string with arguments.

                        string highScore = "googleplex"; // replace with your actual high score variable.
                        string postData = string.Format(@"message=I just got a score of " + highScore + " in The Ghost of Captain Wumpus!\nThis game is awesome!&access_token={1}", highScore, _FacebookOAuthResult.AccessToken);

                        // Some complicated encoding crap...don't worry about what this means
                        // for now.
                        var encoding = new ASCIIEncoding();
                        var dataAsBytes = encoding.GetBytes(postData);
                        request.ContentLength = dataAsBytes.Length;

                        // The request type here is POST as opposed to GET.  We used GET when
                        // we wanted to access personal info (like name, profile pic, etc.).
                        request.Method = "POST";

                        // No idea what this means...but it needs to be there.
                        request.ContentType = @"application/x-www-form-urlencoded";

                        // Write the data to the buffer
                        var newStream = request.GetRequestStream();
                        newStream.Write(dataAsBytes, 0, dataAsBytes.Length);
                        newStream.Close();


                        // Here we make the actual POST request, and wait for a response from 
                        // facebook's web server.
                        var dataStream = request.GetRequestStream();

                        // We should do some error checking on the response we get from facebook,
                        // but no code is perfect.
                        var response = request.GetResponse();
                        response.Close();

                        // -------- END NEW CODE ----------
                    }
                    catch (FacebookApiException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                catch (FacebookApiException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                this._FacebookOAuthResult = null;
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
