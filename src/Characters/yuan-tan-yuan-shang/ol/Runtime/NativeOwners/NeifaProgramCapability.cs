#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NeifaProgramCapability(Func<RuntimeEntries.CharacterYuanTanYuanShangOlEntries> getCharacterYuanTanYuanShangOlRuntime) : INeifaProgramHost
{
    public SkillProgramStepOutcome BeginNeifa(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterYuanTanYuanShangOlRuntime().BeginNeifa(f, e);
}
