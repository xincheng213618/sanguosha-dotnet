#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PrivateOfferProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateCardOffersEntries> getCoreGameEnginePrivateCardOffersRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGameTargetHandHpChoicesEntries> getCoreGameEngineGameTargetHandHpChoicesRuntime) : IPrivateOfferProgramHost
{
    public SkillProgramStepOutcome DepositPrivateCardOffer(ProgramSkillFrame f, string bind, string resolver) => getCoreGameEnginePrivateCardOffersRuntime().DepositPrivateCardOffer(f, bind, resolver);
    public SkillProgramStepOutcome ResolvePrivateCardOffer(ProgramSkillFrame f) => getCoreGameEnginePrivateCardOffersRuntime().BeginDeferredPrivateOffer(f);
    public SkillProgramStepOutcome ResolveGameTargetHandHp(ProgramSkillFrame f, string state) => getCoreGameEngineGameTargetHandHpChoicesRuntime().BeginGameTargetHandHp(f, state);
}
