#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DirectedDistanceDebtCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries> getCoreGameEngineShortRangeSlashTargetsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries> getCoreGameEngineFixedDistanceOneDebtRuntime) : IDirectedDistanceDebtHost
{
    public SkillProgramStepOutcome OfferShortRangeSlashTarget(ProgramSkillFrame frame) => getCoreGameEngineShortRangeSlashTargetsRuntime().OfferShortRangeSlashTarget(frame);
    public void GrantFixedDistanceOneTurnPolicy(ProgramSkillFrame frame, int target) => getCoreGameEngineFixedDistanceOneDebtRuntime().GrantFixedDistanceOneTurnPolicy(frame, target);
    public SkillProgramStepOutcome SettleFixedDistanceOneEndingDebt(ProgramSkillFrame frame) => getCoreGameEngineFixedDistanceOneDebtRuntime().SettleFixedDistanceOneEndingDebt(frame);
}
