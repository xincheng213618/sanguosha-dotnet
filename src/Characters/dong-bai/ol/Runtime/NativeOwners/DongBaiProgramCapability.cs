#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DongBaiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDongBaiOlEntries> getCharacterDongBaiOlRuntime) : IDongBaiProgramHost
{
    public SkillProgramStepOutcome BeginLianZhu(ProgramSkillFrame f, int recipient) => getCharacterDongBaiOlRuntime().BeginLianZhu(f, recipient);
}
