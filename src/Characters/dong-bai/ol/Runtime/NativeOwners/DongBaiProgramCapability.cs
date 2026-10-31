#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DongBaiProgramCapability(Func<RuntimeEntries.CharacterDongBaiOlEntries> getCharacterDongBaiOlRuntime) : IDongBaiProgramHost
{
    public SkillProgramStepOutcome BeginLianZhu(ProgramSkillFrame f, int recipient) => getCharacterDongBaiOlRuntime().BeginLianZhu(f, recipient);
}
