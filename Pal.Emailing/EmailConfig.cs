using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;

namespace Pal.Emailing
{
    public class EmailConfig
    {
        /// <summary>
        /// CA1053: Static holder types should not have constructors
        /// </summary>
        private void NoInstancesNeeded() { }

        /// <summary>
        /// Authentication user name needed to send mails on
        /// the SMTP Server
        /// </summary>       

        public static string PALEmailSMTPServerAuthPassword
        {
            get
            {
                NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("PALEmailSettings");
                if (config == null)
                {
                    config = ConfigurationManager.AppSettings;
                }
                return config["PALEmailSMTPServerAuthPassword"];
            }
        }

        /// <summary>
        /// Authentication user name needed to send mails on
        /// the SMTP Server
        /// </summary>
        public static string PALEmailSMTPServerAuthUserName
        {
            get
            {
                NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("PALEmailSettings");
                if (config == null)
                {
                    config = ConfigurationManager.AppSettings;
                }
                return config["PALEmailSMTPServerAuthUserName"];
            }
        }

		public static string PALEmailSubject
		{
			get
			{
				NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("PALEmailSettings");
				if (config == null)
				{
					config = ConfigurationManager.AppSettings;
				}
				return config["PALEmailSubject"];
			}
		}

		/// <summary>
		/// SMTP Server used to send emails out
		/// </summary>
		public static string SmtpServer
        {
            get
            {
                NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("PALEmailSettings");
                if (config == null)
                {
                    config = ConfigurationManager.AppSettings;
                }
                return config["PALEmailSMTPServer"];
            }
        }

        /// <summary>
        /// Port of SMTP Server used to send emails out
        /// </summary>
        public static string SmtpServerPort
        {
            get
            {
                NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("PALEmailSettings");
                if (config == null)
                {
                    config = ConfigurationManager.AppSettings;
                }
                return config["PALEmailSMTPServerPort"];
            }
        }

        /// <summary>
        /// Enable SSL for SMTP server
        /// </summary>
        public static string SmtpServerEnableSSL
        {
            get
            {
                NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("PALEmailSettings");
                if (config == null)
                {
                    config = ConfigurationManager.AppSettings;
                }
                return config["PALEmailSMTPServerEnableSsl"];
            }
        }

		public static string PALEmailMode
		{
			get
			{
				NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("PALEmailSettings");
				if (config == null)
				{
					config = ConfigurationManager.AppSettings;
				}
				return config["PALEmailMode"];
			}
		}

		public static string PALEmailTo
		{
			get
			{
				NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("PALEmailSettings");
				if (config == null)
				{
					config = ConfigurationManager.AppSettings;
				}
				return config["PALEmailTo"];
			}
		}

	}
}

