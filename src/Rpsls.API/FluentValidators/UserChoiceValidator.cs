using FluentValidation;

public class UserChoiceValidator : AbstractValidator<int>
{
    public UserChoiceValidator()
    {
        RuleFor(choiceId => choiceId)
           .InclusiveBetween(1, 5)
           .WithMessage("Invalid choice ID. Please select a number between 1 and 5.");
    }
}