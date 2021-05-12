using FluentValidation;
using Nop.Plugin.Misc.IPQualityScore.Areas.Admin.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.IPQualityScore.Areas.Admin.Validators
{
    /// <summary>
    /// Represents a validator for <see cref="ConfigurationModel"/>
    /// </summary>
    public class ConfigurationModelValidator : BaseNopValidator<ConfigurationModel>
    {
        #region Ctor

        public ConfigurationModelValidator(ILocalizationService localizationService)
        {
            RuleFor(model => model.ApiKey)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.ApiKey.Required"));

            RuleFor(model => model.IPReputationFraudScoreForBlocking)
                .InclusiveBetween(0, 100)
                .WithMessage(localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.IPReputationFraudScoreForBlocking.FromZeroToOneHundred"))
                .When(model => model.IPReputationEnabled);

            RuleFor(model => model.IPReputationStrictness)
                .InclusiveBetween(0, 3)
                .WithMessage(localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.IPReputationStrictness.FromZeroToThree"))
                .When(model => model.IPReputationEnabled);

            RuleFor(model => model.TransactionStrictness)
                .InclusiveBetween(0, 2)
                .WithMessage(localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.TransactionStrictness.FromZeroToTwo"))
                .When(model => model.IPReputationEnabled && model.OrderScoringEnabled);

            RuleFor(model => model.RiskScoreForBlocking)
                .InclusiveBetween(0, 100)
                .WithMessage(localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.RiskScoreForBlocking.FromZeroToOneHundred"))
                .When(model => model.IPReputationEnabled && model.OrderScoringEnabled);

            RuleFor(model => model.EmailReputationFraudScoreForBlocking)
                .InclusiveBetween(0, 100)
                .WithMessage(localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.EmailReputationFraudScoreForBlocking.FromZeroToOneHundred"))
                .When(model => model.EmailValidationEnabled);

            RuleFor(model => model.EmailReputationStrictness)
                .InclusiveBetween(0, 2)
                .WithMessage(localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.EmailReputationStrictness.FromZeroToTwo"))
                .When(model => model.EmailValidationEnabled);

            RuleFor(model => model.AbuseStrictness)
                .InclusiveBetween(0, 2)
                .WithMessage(localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.AbuseStrictness.FromZeroToTwo"))
                .When(model => model.EmailValidationEnabled);

            RuleFor(model => model.DeviceFingerprintTrackingCode)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.DeviceFingerprintTrackingCode.Required"))
                .When(model => model.DeviceFingerprintEnabled);

            RuleFor(model => model.DeviceFingerprintFraudChance)
                .InclusiveBetween(0, 100)
                .WithMessage(localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.DeviceFingerprintFraudChance.FromZeroToOneHundred"))
                .When(model => model.DeviceFingerprintEnabled);
        }

        #endregion
    }
}
