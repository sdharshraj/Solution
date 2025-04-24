using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
	public class CreateProductDtoValidator : AbstractValidator<ProductDto>
	{
		public CreateProductDtoValidator()
		{
			RuleFor(x => x.ProductName)
				.NotEmpty().WithMessage("Product name is required.")
				.MaximumLength(255);

			RuleFor(x => x.CreatedBy)
				.NotEmpty().WithMessage("CreatedBy is required.")
				.MaximumLength(100);

			RuleFor(x => x.CreatedOn)
				.NotEmpty().WithMessage("CreatedOn is required.");
		}
	}
}
