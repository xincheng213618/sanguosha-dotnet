#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5GongsunCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5GongsunZanEntries> getCoreGameEngineShanheS5GongsunZanRuntime) : IShanheS5GongsunProgramHost
{
    public SkillProgramStepOutcome DiscardAttackRangeCardsFromSlashTarget(ProgramSkillFrame f) => getCoreGameEngineShanheS5GongsunZanRuntime().BeginShanheS5Qiaomeng(f);
}
