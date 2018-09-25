using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Emailing
{
    /// <summary>
	/// Represents a message that can be 
	/// send using the emailing service
	/// </summary>
	public class EmailingMessage
    {
        public string FromName
        {
            get { return _fromName; }
            set { _fromName = value; }
        }

        public string FromEmail
        {
            get { return _fromEmail; }
            set { _fromEmail = value; }
        }

        public string ToEmail
        {
            get { return _toEmail; }
            set { _toEmail = value; }
        }

        public string toName
        {
            get { return _toName; }
            set { _toName = value; }
        }

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public EmailFormat Format
        {
            get { return _format; }
            set { _format = value; }
        }


        string _fromName,
                _fromEmail,
                _toName,
                _toEmail,
                _subject,
                _body;
        EmailFormat _format;

    }
}
