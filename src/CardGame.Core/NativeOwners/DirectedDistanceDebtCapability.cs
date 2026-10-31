#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DirectedDistanceDebtCapability(Func<RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries> getCoreGameEngineShortRangeSlashTargetsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime>> getCoreGameEngineFixedDistanceOneDebtRuntime) : IDirectedDistanceDebtHost
{
    public SkillProgramStepOutcome OfferShortRangeSlashTarget(ProgramSkillFrame frame) => getCoreGameEngineShortRangeSlashTargetsRuntime().OfferShortRangeSlashTarget(frame);
    public void GrantFixedDistanceOneTurnPolicy(ProgramSkillFrame frame, int target) => getCoreGameEngineFixedDistanceOneDebtRuntime().Value.GrantFixedDistanceOneTurnPolicy(frame, target);
    public SkillProgramStepOutcome SettleFixedDistanceOneEndingDebt(ProgramSkillFrame frame) => getCoreGameEngineFixedDistanceOneDebtRuntime().Value.SettleFixedDistanceOneEndingDebt(frame);
}
