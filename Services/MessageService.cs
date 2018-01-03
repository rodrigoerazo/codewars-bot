﻿using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Codewars_Bot.Services
{
	public class MessageService
	{
		public async Task<string> MessageHandler(string message)
		{
			var databaseConnectionService = new DatabaseConnectionService();
			databaseConnectionService.AuditMessageInDatabase(message);

			switch (message)
			{
				case "/weekly_rating":
					return GetWeeklyRating();
				case "/total_rating":
					return GetGeneralRating();
				case "/start":
					return ShowFaq();
				case "/show_faq":
					return ShowFaq();
				default:
					return await SaveNewUser(message);
			}

		}

		private async Task<string> SaveNewUser(string message)
		{
			var databaseConnectionService = new DatabaseConnectionService();
			var codewarsConnectionService = new CodewarsConnectionService();

			var userinfo = message.Split(' ');

			if (userinfo.Length != 2)
			{
				return string.Empty;
			}

			var user = new UserModel
			{
				CodewarsUsername = message.Split(' ')[0],
				TelegramUsername = message.Split(' ')[1].Replace("@", "")
			};

			if (databaseConnectionService.CheckIfUserExists(user) || user.CodewarsUsername == "start" || user.TelegramUsername == "/sign_up")
			{
				return $"Користувач {user.CodewarsUsername} вже зареєстрований";
			}

			var codewarsUser = await codewarsConnectionService.GetCodewarsUser(user.CodewarsUsername);

			if (codewarsUser.Name == "NOT EXIST")
			{
				return $"Користувач {user.CodewarsUsername} не зареєстрований на codewars.com";
			}
			else
			{
				user.CodewarsFullname = codewarsUser.Name;
				user.Points = codewarsUser.Honor;
			}

			return databaseConnectionService.SaveUserToDatabase(user);
		}

		private string GetWeeklyRating()
		{
			var databaseConnectionService = new DatabaseConnectionService();
			return databaseConnectionService.GetWeeklyRating();
		}

		private string GetGeneralRating()
		{
			var databaseConnectionService = new DatabaseConnectionService();
			return databaseConnectionService.GetGeneralRating();
		}

		private string ShowFaq()
		{
			var response = new StringBuilder();

			response.Append("Вітаємо в клані ІТ КРІ на Codewars! <br/><br/>codewars.com -- це знаменитий сайт з задачами для програмістів, за розв'язок яких нараховуються бали. ");
			response.Append("От цими балами ми і будемо мірятись в кінці кожного тижня. <br/><br/>Цей бот створений для того, щоб зробити реєстрацію в клані максимально швидкою і приємною. ");
			response.Append("Щоб долучитись до рейтингу треба: <br/>1) Зареєструватись на codewars.com <br/>2) Надіслати сюди ваші нікнейми в Codewars і Telegram одним повідомленням, розділені пробілом. Приклад: username username");
			response.Append("<br/><br/>Бали оновлюються раз на годину. Також доступні дві команди: <br/>1) /weekly_rating показує поточний рейтинг за цей тиждень. <br/>2) /total_rating відображає загальну кількість балів в кожного користувача");
			response.Append("<br/><br/>Запрошуйте друзів в клан і гайда рубитись!");
			response.Append("<br/><br/>P.S: якщо знайшли багу або маєте зауваження -- пишіть йому @maksim36ua");

			response.Append("");
			return response.ToString();
		}
	}
}