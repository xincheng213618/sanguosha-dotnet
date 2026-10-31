#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryFuRongFuQianCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterFuRongFuQianAnniversaryEntries> getCharacterFuRongFuQianAnniversaryRuntime) : IAnniversaryFuRongFuQianHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterFuRongFuQianAnniversaryRuntime().BeginAnniversaryFuPair(f, op);
}
