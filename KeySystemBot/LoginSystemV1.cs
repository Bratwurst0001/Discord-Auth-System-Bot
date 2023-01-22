using System;
using System.Collections.Generic;
using System.Text;

namespace KeySystemBot
{
	using System;
	using System.IO;
	using System.Net.Sockets;

	public class LoginSystemV1
	{
		public enum Options
		{
			Register = 1,
			Login,
			License,
			GenerateLicense,
			Deletelicense
		}

		public TcpClient tcpClient = new TcpClient();

		public StreamReader reader;

		public StreamWriter writer;

		public string UserPasswort { get; set; }

		public string UserName { get; set; }

		public string License { get; set; }

		public string HWID { get; set; }

		public LoginSystemV1(string serverip)
		{
			if (tcpClient.Connected)
			{
				tcpClient.Close();
			}
			Console.Title = "Login System by Bratwurst001";
			Console.WriteLine("Easy Login-System by Bratwurst001");
			tcpClient.Connect(serverip, 6005);
			reader = new StreamReader(tcpClient.GetStream());
			writer = new StreamWriter(tcpClient.GetStream());
		}

		public bool Register(string Name, string Passwort, string Currenthwid, string license)
		{
			UserName = Name;
			UserPasswort = Passwort;
			HWID = Currenthwid;
			sendPacket(Options.Register.ToString() + " " + UserName + " " + UserPasswort + " " + HWID + " " + license);
			return true;
		}

		public string GenerateLicense()
		{
			string text = "ASDFGHJKLÖPOIUZTREQW1234567890";
			string text2 = "";
			for (int i = 0; i < 32; i++)
			{
				Random random = new Random();
				text2 += text[random.Next(1, text.Length)];
			}
			return text2;
		}

		public void DelLicense(Enum DEL, string license)
		{
			sendPacket(Options.Deletelicense.ToString() + " " + license);
		}

		public bool Login(string Name, string Passwort, string Currenthwid)
		{
			UserName = Name;
			UserPasswort = Passwort;
			HWID = Currenthwid;
			Console.WriteLine("Attempting login");
			sendPacket(Options.Login.ToString() + " " + UserName + " " + UserPasswort + " " + HWID);
			Console.WriteLine("Waiting for Server");
			string text = readPacket();
			Console.WriteLine("Login Hash: {0}", text);
			if (text == "False")
			{
				Console.WriteLine("Wrong Password");
				return false;
			}
			if (text == "True")
			{
				Console.WriteLine("Login Sucessfully");
				return true;
			}
			return false;
		}

		public bool Checklicense(Enum OPLicense, string license)
		{
			sendPacket(Options.License.ToString() + " " + license);
			string text = readPacket();
			Console.WriteLine("license Hash: {0}", text);
			if (text == "False")
			{
				Console.WriteLine("invalid license");
				return false;
			}
			Console.WriteLine("valid license");
			return true;
		}

		public string readPacket()
		{
			return reader.ReadLine();
		}

		public void sendPacket(string packet)
		{
			writer.WriteLine(packet);
			writer.Flush();
		}
	}

}
