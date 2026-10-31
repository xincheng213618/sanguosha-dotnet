#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PrivateOfferProgramCapability(Func<RuntimeEntries.CoreGameEnginePrivateCardOffersEntries> getCoreGameEnginePrivateCardOffersRuntime, Func<RuntimeEntries.CoreGameEngineGameTargetHandHpChoicesEntries> getCoreGameEngineGameTargetHandHpChoicesRuntime) : IPrivateOfferProgramHost
{
    public SkillProgramStepOutcome DepositPrivateCardOffer(ProgramSkillFrame f, string bind, string resolver) => getCoreGameEnginePrivateCardOffersRuntime().DepositPrivateCardOffer(f, bind, resolver);
    public SkillProgramStepOutcome ResolvePrivateCardOffer(ProgramSkillFrame f) => getCoreGameEnginePrivateCardOffersRuntime().BeginDeferredPrivateOffer(f);
    public SkillProgramStepOutcome ResolveGameTargetHandHp(ProgramSkillFrame f, string state) => getCoreGameEngineGameTargetHandHpChoicesRuntime().BeginGameTargetHandHp(f, state);
}
