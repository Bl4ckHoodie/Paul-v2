using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using AIMLbot;

namespace paul_v1
{
    public partial class PAUL : Form
    {
        Bot mrrobot = new Bot(); // Create bot 
        public int count = 0;
        private SpeechRecognitionEngine reEngine = new SpeechRecognitionEngine();
        private SpeechSynthesizer respnd = new SpeechSynthesizer(); 
        public PAUL()
        {
            InitializeComponent();
        }

       

        private void label1_Click(object sender, EventArgs e)//Menu Label
        {
         respnd.SpeakAsync("Goodbye");
         // Goodbye 
         Application.Exit();
        }

        private void PAUL_Load(object sender, EventArgs e) //load_up 
        {
            respnd.Dispose();
            respnd = new SpeechSynthesizer();
            int comp = DateTime.Now.ToShortTimeString().CompareTo("6:00 PM");
            int comp2 = DateTime.Now.ToShortTimeString().CompareTo("12:00 AM");
            int comp3 = DateTime.Now.ToShortTimeString().CompareTo("12:00 PM");
            if ((comp >= 0) && (comp3 < 0))
            {
                respnd.SpeakAsync("Good Evening Mr Hawk");
                time.Enabled = true;
            }
            else
            if ((comp2 <= 0) && (comp3 < 0))
            {
                respnd.SpeakAsync("Good Morning Mr Hawk");
                time.Enabled = true;
            }
            else
            if ((comp3 <= 0) && (comp <= 0))
            {
                respnd.SpeakAsync("Good Afternoon Mr Hawk");
                time.Enabled = true;
            }

            try
            {
                mrrobot.isAcceptingUserInput = false;
                mrrobot.loadSettings();
                mrrobot.loadAIMLFromFiles();
                mrrobot.isAcceptingUserInput = true;
            }
            catch (Exception)
            {
                MessageBox.Show("WARNING COULD LOAD SCRIPT");
            }
           
            LoadSpeech();
        }
        //Reply Function 

        public void LoadSpeech()
        {

            try
            {
                reEngine = new SpeechRecognitionEngine(); //Create the instance 
                reEngine.SetInputToDefaultAudioDevice();
                reEngine.LoadGrammar(new DictationGrammar());
                reEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(reco);
                reEngine.RecognizeAsync(RecognizeMode.Multiple);

            }
            catch (Exception ex)
            {


            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// 
        private void stopL(object s, SpeechRecognizedEventArgs e)
        {
            label2.Text = e.Result.Text;
            string user = label2.Text;
            if (user.ToUpper() != "START LISTENING")
                stopL(s, e);
            else
                reco(s, e);
        }
        private void reco(object s, SpeechRecognizedEventArgs e)
        {
            label2.Text = e.Result.Text;
            string user = label2.Text;
            if (user.ToUpper() == "STOP LISTENING")
            {
                stopL(s, e);
            }
            if (user != "") {
                Output.AppendText("You : " + user + Environment.NewLine);
                String paul = GetReply(user);
                Output.AppendText("Mr Robot : " + paul + Environment.NewLine);
                Reply.Text = "Paul: " + paul;
                respnd.SpeakAsync(paul);
                Input.Clear();
                label2.Text = "";
            }
            label2.Text = "";
            Reply.Text = "";
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public string GetReply(string quest)
        {
            User myUser = new User("Username", mrrobot);
            Request req = new Request(quest, myUser, mrrobot);
            Result res = new Result(myUser, mrrobot, req);
            res = mrrobot.Chat(req);
            return res.Output;
        }
        //End of Get reply function
        private void timer1_Tick(object sender, EventArgs e)
        {
            string tme = DateTime.Now.ToShortTimeString();
            if (tme == "12:00 AM")
            {
                respnd.Dispose();
                respnd = new SpeechSynthesizer();
                respnd.SpeakAsync("Its time to go to sleep Mr Hawk");
                time.Enabled = false;

            }
            if (tme == "05:30 AM")
            {
                respnd.Dispose();
                respnd = new SpeechSynthesizer();
                respnd.SpeakAsync("Good Morning Mr Hawk, would you like me to give you the latest news?");
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string user = Input.Text;
            Output.AppendText("You : " + user+Environment.NewLine);
            String paul = GetReply(user);
            label2.Text = "You: " + user;
            Output.AppendText("Mr Robot : " + paul + Environment.NewLine);
            respnd.SpeakAsync(paul);
            Reply.Text = "Paul: " + paul;
            Input.Clear();
           

        }

        private void Output_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (count == 0)
            {
                Output.Visible = true;
                Input.Visible = true;
                button1.Visible = true;
                label2.Visible = true;
                Reply.Visible = true;
                count = 1;
            }
            else
            {
                if (count == 1)
                {

                    Output.Visible = false;
                    Input.Visible = false;
                    button1.Visible = false;
                    label2.Visible = false;
                    Reply.Visible = false;
                    count = 0;
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Input_TextChanged(object sender, EventArgs e)
        {

        }
    }
}