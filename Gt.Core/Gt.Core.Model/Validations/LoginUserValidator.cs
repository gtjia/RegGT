using FluentValidation;
using Gt.Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gt.Core.Model.Validations
{
	public class LoginUserValidator : AbstractValidator<LoginUserModel>
	{
		public LoginUserValidator()
		{
			RuleFor(vm => vm.UserName).NotEmpty().WithMessage("Username cannot be empty");
			RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
			RuleFor(vm => vm.Password).Length(6, 12).WithMessage("Password must be between 6 and 12 characters");
		}
	}
}
