using E_CommerceApi.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Application.Validators.Products
{
    public class CreateProductValidator:AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator() {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                   .WithMessage("Please, product name cannot be empty")
                .MaximumLength(150)
                .MinimumLength(5)
                    .WithMessage("Please, enter the product name between 5-10 characters ");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Please, stock value cannot be empty")
                .Must(s => s >= 0)
                    .WithMessage("Please, stock value cannot be negatif ");

            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Please, price value cannot be empty")
                .Must(s => s >= 0)
                    .WithMessage("Please, price value cannot be negatif ");

        }

    }
}
