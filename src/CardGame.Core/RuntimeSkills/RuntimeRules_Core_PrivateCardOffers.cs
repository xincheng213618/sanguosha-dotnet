
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_PrivateCardOffers
{
    internal static string PrivateOfferReason(ProgramSkillFrame f, string segment) => $"skill-program.{f.SkillId}.{SkillProgramEffectOp.ResolveDeferredPrivateCardOffer}.{segment}";

    internal static CardConversionSource PrivateOfferSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
}
