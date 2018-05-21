using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Neurotec.Biometrics;
using Neurotec.Gui;

namespace FingerScaner1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        Nffv _engine;
        string _userDatabaseFile;

        public MainForm()
        {
            InitializeComponent();
        }


        internal class EnrollmentResult
        {
            public NffvStatus engineStatus;
            public NffvUser engineUser;
        };

        private void doEnroll(object sender, DoWorkEventArgs args)
        {
            EnrollmentResult enrollmentResults = new EnrollmentResult();
            enrollmentResults.engineUser = _engine.Enroll(20000, out enrollmentResults.engineStatus);
            args.Result = enrollmentResults;
        }

        internal class VerificationResult
        {
            public NffvStatus engineStatus;
            public int score;
        };

        private void doVerify(object sender, DoWorkEventArgs args)
        {
            VerificationResult verificationResult = new VerificationResult();
            verificationResult.score = _engine.Verify((NffvUser)args.Argument, 20000, out verificationResult.engineStatus);
            args.Result = verificationResult;
        }

        private void CancelScanningHandler(object sender, EventArgs e)
        {
            _engine.Cancel();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            RunWorkerCompletedEventArgs taskResult = BusyForm.RunLongTask("Waiting for fingerprint ...", new DoWorkEventHandler(doEnroll), false, null, new EventHandler(CancelScanningHandler));
            EnrollmentResult enrollmentResult = (EnrollmentResult)taskResult.Result;
            if (enrollmentResult.engineStatus == NffvStatus.TemplateCreated)
            {
                NffvUser engineUser = enrollmentResult.engineUser;
                string userName = "1";
                if (userName.Length <= 0)
                {
                    userName = engineUser.Id.ToString();
                }

                //_userDB.Add(new UserRecord(engineUser.Id, userName));
                try
                {
                    //_userDB.WriteToFile(_userDatabaseFile);
                }
                catch { }

                System.IntPtr a1;
                System.Windows.Forms.PictureBox po = new PictureBox();
                a1 = engineUser.GetHBitmap();
                

                //pbExtractedImage.Image = engineUser.GetBitmap();

                //lbDatabase.Items.Add(new CData(engineUser, userName));
                //lbDatabase.SelectedIndex = lbDatabase.Items.Count - 1;
            }
            else
            {
                NffvStatus engineStatus = enrollmentResult.engineStatus;
                //MessageBox.Show(string.Format("Enrollment was not finished. Reason: {0}", engineStatus));
            }
        }

    }

    class CData : IDisposable
    {
        private NffvUser _engineUser;
        private Bitmap _image;
        private string _name;

        public CData(NffvUser engineUser, string name)
        {
            _engineUser = engineUser;
            _image = engineUser.GetBitmap();
            _name = name;
        }

        public NffvUser EngineUser
        {
            get
            {
                return _engineUser;
            }
        }

        public Bitmap Image
        {
            get
            {
                return _image;
            }
        }

        public int ID
        {
            get
            {
                return _engineUser.Id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_image != null)
            {
                _image.Dispose();
                _image = null;
            }
        }

        #endregion
    }
}
