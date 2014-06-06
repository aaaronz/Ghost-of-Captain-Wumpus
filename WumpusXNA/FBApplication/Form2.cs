using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facebook;
using System.IO;
using System.Reflection;

namespace FBApplication
{
    public partial class Form2 : Form
    {
        private readonly string _accessToken;
        public Form2(string accessToken)
        {
            _accessToken = accessToken;

            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            GraphApiExample();
        }

        private void GraphApiExample()
        {

            try
            {
                var fb = new FacebookClient(_accessToken);


                object result = fb.Get("me");

                Type tresult = result.GetType();

                string id = tresult.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, result, new object[] { "id" }).ToString();
                string objname = tresult.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, result, new object[] { "name" }).ToString();

                MDIParent1 frmmdi;
                frmmdi = (MDIParent1)this.MdiParent;
                frmmdi.textBox1.Text = id;


                string firstName = tresult.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, result, new object[] { "first_name" }).ToString();
                string lastName = tresult.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, result, new object[] { "last_name" }).ToString();

                frmmdi.textBox2.Text = firstName;
                frmmdi.textBox3.Text = lastName;
                //var localeExists = result.ContainsKey("locale");
                //var dictionary = (IDictionary<string, object>)result;
                //localeExists = dictionary.ContainsKey("locale");
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       


    }
}