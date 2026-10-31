#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryFuRongFuQianCapability(Func<RuntimeEntries.CharacterFuRongFuQianAnniversaryEntries> getCharacterFuRongFuQianAnniversaryRuntime) : IAnniversaryFuRongFuQianHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterFuRongFuQianAnniversaryRuntime().BeginAnniversaryFuPair(f, op);
}
