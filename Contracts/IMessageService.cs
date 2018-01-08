﻿using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codewars_Bot.Contracts
{
	public interface IMessageService
	{
		Task<string> MessageHandler(Activity activity);
	}
}
