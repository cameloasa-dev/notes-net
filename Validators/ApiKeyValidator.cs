using FluentValidation;
using MyOwnApi.Repositories;

namespace MyOwnApi.Validators;

    public class ApiKeyValidator : AbstractValidator<string>
    {
        public ApiKeyValidator(IApiKeyRepository apiKeyRepository)
        {
            RuleFor(userId => userId)
                .NotEmpty().WithMessage("UserId is required.")
                .MustAsync(async (userId, _) =>
                {
                    var existingKey = await apiKeyRepository.GetByUserIdAsync(userId);

                    // if user does not have an API Key → valid
                    if (existingKey == null)
                        return true;

                    // if API Key exists and is ACTIVE → invalid
                    return false;
                })
                .WithMessage("User already has an active API key.");
        }
    }

