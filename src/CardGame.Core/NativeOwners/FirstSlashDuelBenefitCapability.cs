#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class FirstSlashDuelBenefitCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFirstSlashDuelBenefitsEntries> getCoreGameEngineFirstSlashDuelBenefitsRuntime) : IFirstSlashDuelBenefitHost
{
    public SkillProgramStepOutcome ChooseFirstSlashDuelDiamondBenefit(ProgramSkillFrame frame) => getCoreGameEngineFirstSlashDuelBenefitsRuntime().BeginFirstSlashDuelBenefit(frame);
}
