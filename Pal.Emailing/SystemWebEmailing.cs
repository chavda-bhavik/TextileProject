using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Pal.Emailing
{
	public class SystemWebEmailing : IEmailing
	{

		/// <summary>
		/// Method used to strip the Freetextbox mailmessage from html code and create plain text message.
		/// http://www.codeproject.com/KB/HTML/HTML_to_Plain_Text.aspx 
		/// </summary>
		/// <param name="source">mailmessage source</param>
		/// <returns>result</returns>
		private string StripHTML(string source)
		{
			try
			{
				string result;

				// Remove HTML Development formatting
				// Replace line breaks with space
				// because browsers inserts space
				result = source.Replace("\r", " ");
				// Replace line breaks with space
				// because browsers inserts space
				result = result.Replace("\n", " ");
				// Remove step-formatting
				result = result.Replace("\t", string.Empty);
				// Remove repeating spaces because browsers ignore them
				result = System.Text.RegularExpressions.Regex.Replace(result,
																	  @"( )+", " ");

				// Remove the header (prepare first by clearing attributes)
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<( )*head([^>])*>", "<head>",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"(<( )*(/)( )*head( )*>)", "</head>",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 "(<head>).*(</head>)", string.Empty,
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// remove all scripts (prepare first by clearing attributes)
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<( )*script([^>])*>", "<script>",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"(<( )*(/)( )*script( )*>)", "</script>",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				//result = System.Text.RegularExpressions.Regex.Replace(result,
				//         @"(<script>)([^(<script>\.</script>)])*(</script>)",
				//         string.Empty,
				//         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"(<script>).*(</script>)", string.Empty,
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// remove all styles (prepare first by clearing attributes)
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<( )*style([^>])*>", "<style>",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"(<( )*(/)( )*style( )*>)", "</style>",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 "(<style>).*(</style>)", string.Empty,
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// insert tabs in spaces of <td> tags
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<( )*td([^>])*>", "\t",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// insert line breaks in places of <BR> and <LI> tags
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<( )*br( )*>", "\r",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<( )*br/( )*>", "\r",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<( )*li( )*>", "\r",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// insert line paragraphs (double line breaks) in place
				// if <P>, <DIV> and <TR> tags
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<( )*div([^>])*>", "\r\r",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<( )*tr([^>])*>", "\r\r",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<( )*p([^>])*>", "\r\r",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// Remove remaining tags like <a>, links, images,
				// comments etc - anything that's enclosed inside < >
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"<[^>]*>", string.Empty,
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// replace special characters:
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @" ", " ",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"&bull;", " * ",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"&lsaquo;", "<",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"&rsaquo;", ">",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"&trade;", "(tm)",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"&frasl;", "/",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"&lt;", "<",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"&gt;", ">",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"&copy;", "(c)",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"&reg;", "(r)",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				// Remove all others. More can be added, see
				// http://hotwired.lycos.com/webmonkey/reference/special_characters/
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 @"&(.{2,6});", string.Empty,
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// for testing
				//System.Text.RegularExpressions.Regex.Replace(result,
				//       this.txtRegex.Text,string.Empty,
				//       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// make line breaking consistent
				result = result.Replace("\n", "\r");

				// Remove extra line breaks and tabs:
				// replace over 2 breaks with 2 and over 4 tabs with 4.
				// Prepare first to remove any whitespaces in between
				// the escaped characters and remove redundant tabs in between line breaks
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 "(\r)( )+(\r)", "\r\r",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 "(\t)( )+(\t)", "\t\t",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 "(\t)( )+(\r)", "\t\r",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 "(\r)( )+(\t)", "\r\t",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				// Remove redundant tabs
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 "(\r)(\t)+(\r)", "\r\r",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				// Remove multiple tabs following a line break with just one tab
				result = System.Text.RegularExpressions.Regex.Replace(result,
						 "(\r)(\t)+", "\r\t",
						 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				// Initial replacement target string for line breaks
				string breaks = "\r\r\r";
				// Initial replacement target string for tabs
				string tabs = "\t\t\t\t\t";
				for (int index = 0; index < result.Length; index++)
				{
					result = result.Replace(breaks, "\r\r");
					result = result.Replace(tabs, "\t\t\t\t");
					breaks = breaks + "\r";
					tabs = tabs + "\t";
				}

				// That's it.
				return result;
			}
			catch
			{
				//MessageBox.Show("Error");
				return source;
			}
		}

		/// <summary>
		/// Sends the given message
		/// </summary>
		public void SendEmail(EmailingMessage mail)
		{
			try
			{

				MailMessage objMessage = new MailMessage();

				SmtpClient smtp = new SmtpClient(EmailConfig.SmtpServer);

				// Check if server requires authentication
				if (EmailConfig.PALEmailSMTPServerAuthUserName != null)
				{
					smtp.Credentials = new NetworkCredential(EmailConfig.PALEmailSMTPServerAuthUserName, EmailConfig.PALEmailSMTPServerAuthPassword);
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				}

				objMessage.To.Add(mail.ToEmail);
				objMessage.From = new MailAddress((mail.FromName == null) ? mail.FromEmail : "\"" + mail.FromName + "\" <" + mail.FromEmail + ">");
				objMessage.Subject = mail.Subject;

				ContentType htmlMimeType = new System.Net.Mime.ContentType("text/html");
				mail.Format = EmailFormat.Html;
				// Construct the body as HTML.
				string bodyHtml = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>";
				bodyHtml += "<html><head><meta http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
				bodyHtml += "</head><body><div><font size=\"2\" face=\"arial\">" + mail.Body;
				bodyHtml += "</font></div></body></html>";

				//AlternateView alternateHtml = AlternateView.CreateAlternateViewFromString(bodyHtml, null, "text/html");
				//alternateHtml.TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;
				//objMessage.AlternateViews.Add(alternateHtml);

				objMessage.Body = bodyHtml;//.Replace("<br />", "");
				objMessage.IsBodyHtml = true;
				//ContentType mimeType = new System.Net.Mime.ContentType("text/plain");
				//// Construct the body as TEXT.
				//string bodyPlain="";
				//if(mail.Format!= EmailFormat.Html)
				//bodyPlain = StripHTML(mail.Body);
				//bodyPlain = mail.Body.Replace("<br />", "");
				//AlternateView alternatePlain = AlternateView.CreateAlternateViewFromString(bodyPlain, null, "text/plain");
				//alternatePlain.TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;
				//objMessage.AlternateViews.Add(alternatePlain);	

				objMessage.BodyEncoding = System.Text.Encoding.UTF8;

				smtp.Port = Int32.Parse(EmailConfig.SmtpServerPort);
				smtp.EnableSsl = Boolean.Parse(EmailConfig.SmtpServerEnableSSL);
				smtp.Send(objMessage);
				System.Threading.Thread.Sleep(500);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}
