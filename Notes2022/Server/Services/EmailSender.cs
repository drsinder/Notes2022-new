/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: EmailSender.cs
    **
    ** Description:
    **      Send Email sometimes with attachment
    **
    ** This program is free software: you can redistribute it and/or modify
    ** it under the terms of the GNU General Public License version 3 as
    ** published by the Free Software Foundation.   
    **
    ** This program is distributed in the hope that it will be useful,
    ** but WITHOUT ANY WARRANTY; without even the implied warranty of
    ** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    ** GNU General Public License version 3 for more details.
    **
    **  You should have received a copy of the GNU General Public License
    **  version 3 along with this program in file "license-gpl-3.0.txt".
    **  If not, see<http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/


using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notes2022.Server.Services
{
    public class EmailSender : IEmailSender
    {
        //public StreamWriter StreamWriter { get; private set; }

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public EmailSender()
        {
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var apiKey = Globals.SendGridApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Globals.SendGridEmail, Globals.SendGridName);
            var to = new EmailAddress(email);
            var htmlStart = "<!DOCTYPE html>";
            var isHtml = message.StartsWith(htmlStart);

            SendGridMessage msg;

            if (email.Contains(';')) // multiple targets
            {
                string[] who = email.Split(';');

                List<EmailAddress> addresses = new List<EmailAddress>();
                foreach (string a in who)
                {
                    addresses.Add(new EmailAddress(a.Trim()));
                }
                msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, addresses, subject, isHtml ? "See Html Attachment." : message, isHtml ? "See Html Attachment." : message);
            }
            else // single target
            {
                msg = MailHelper.CreateSingleEmail(from, to, subject, isHtml ? "See Html Attachment." : message, isHtml ? "See Html Attachment." : message);
            }

            if (isHtml)
            {
                MemoryStream ms = new();
                StreamWriter sw = new(ms);
                await sw.WriteAsync(message);
                await sw.FlushAsync();
                ms.Seek(0, SeekOrigin.Begin);
                await msg.AddAttachmentAsync(subject + ".html", ms);
                ms.Dispose();
            }

            await client.SendEmailAsync(msg);
        }


    }

    public class AuthMessageSenderOptions
    {
        public string SendGridKey { get; set; }
    }
}
