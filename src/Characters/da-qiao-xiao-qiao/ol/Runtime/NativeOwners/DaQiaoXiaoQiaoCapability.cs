#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DaQiaoXiaoQiaoCapability(Func<RuntimeEntries.CharacterDaQiaoXiaoQiaoOlEntries> getCharacterDaQiaoXiaoQiaoOlRuntime) : IDaQiaoXiaoQiaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, bool maintain) => getCharacterDaQiaoXiaoQiaoOlRuntime().BeginDaQiaoXiaoQiao(f, maintain);
}
