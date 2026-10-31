#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DaQiaoXiaoQiaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoOlEntries> getCharacterDaQiaoXiaoQiaoOlRuntime) : IDaQiaoXiaoQiaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, bool maintain) => getCharacterDaQiaoXiaoQiaoOlRuntime().BeginDaQiaoXiaoQiao(f, maintain);
}
