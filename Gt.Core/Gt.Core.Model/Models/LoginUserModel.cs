using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.Attributes;
using Gt.Core.Model.Validations;

namespace Gt.Core.Model.Models
{
	[Validator(typeof(LoginUserValidator))]
	public class LoginUserModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
