#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5GongsunCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheS5GongsunZanRuntime>> getCoreGameEngineShanheS5GongsunZanRuntime) : IShanheS5GongsunProgramHost
{
    public SkillProgramStepOutcome DiscardAttackRangeCardsFromSlashTarget(ProgramSkillFrame f) => getCoreGameEngineShanheS5GongsunZanRuntime().Value.BeginShanheS5Qiaomeng(f);
}
