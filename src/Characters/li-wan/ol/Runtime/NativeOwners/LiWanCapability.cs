#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LiWanCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiWanOlRuntime>> getCharacterLiWanOlRuntime) : ILiWanProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterLiWanOlRuntime().Value.BeginLiWan(frame, op);
}
