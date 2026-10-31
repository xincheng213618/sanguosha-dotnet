#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NeifaProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYuanTanYuanShangOlEntries> getCharacterYuanTanYuanShangOlRuntime) : INeifaProgramHost
{
    public SkillProgramStepOutcome BeginNeifa(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterYuanTanYuanShangOlRuntime().BeginNeifa(f, e);
}
