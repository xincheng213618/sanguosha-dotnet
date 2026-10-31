#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LiWanCapability(Func<RuntimeEntries.CharacterLiWanOlEntries> getCharacterLiWanOlRuntime) : ILiWanProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterLiWanOlRuntime().BeginLiWan(frame, op);
}
