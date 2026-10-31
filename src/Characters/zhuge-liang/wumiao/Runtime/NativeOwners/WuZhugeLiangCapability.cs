#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WuZhugeLiangCapability(Func<RuntimeEntries.CharacterZhugeLiangWumiaoEntries> getCharacterZhugeLiangWumiaoRuntime) : IWuZhugeLiangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhugeLiangWumiaoRuntime().BeginWuZgl(f, op);
}
