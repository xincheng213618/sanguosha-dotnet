#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DirectedDistanceDebtCapability(Func<RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries> getCoreGameEngineShortRangeSlashTargetsRuntime, Func<RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries> getCoreGameEngineFixedDistanceOneDebtRuntime) : IDirectedDistanceDebtHost
{
    public SkillProgramStepOutcome OfferShortRangeSlashTarget(ProgramSkillFrame frame) => getCoreGameEngineShortRangeSlashTargetsRuntime().OfferShortRangeSlashTarget(frame);
    public void GrantFixedDistanceOneTurnPolicy(ProgramSkillFrame frame, int target) => getCoreGameEngineFixedDistanceOneDebtRuntime().GrantFixedDistanceOneTurnPolicy(frame, target);
    public SkillProgramStepOutcome SettleFixedDistanceOneEndingDebt(ProgramSkillFrame frame) => getCoreGameEngineFixedDistanceOneDebtRuntime().SettleFixedDistanceOneEndingDebt(frame);
}
