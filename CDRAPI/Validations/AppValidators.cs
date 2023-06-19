using CDRAPI.DTOs;
using FluentValidation;

namespace CDRAPI.Validations
{
    public class AverageCallValidator : AbstractValidator<AverageCall>
    {
        public AverageCallValidator()
        {
            RuleFor(x => x.FromDate).LessThanOrEqualTo(x => x.ToDate);
            RuleFor(x => x.FromDate).NotEmpty();
            RuleFor(x => x.ToDate).NotEmpty();
        }
    }

    public class SingleDateValidator : AbstractValidator<SingleDate>
    {
        public SingleDateValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
        }
    }

    public class CallerValidator : AbstractValidator<Caller>
    {
        public CallerValidator()
        {
            RuleFor(x => x.CallerId).NotEmpty();
        }
    }
}
