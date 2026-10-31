#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouTianYuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterTianYuMouMobileRuntime>> getCharacterTianYuMouMobileRuntime) : IMouTianYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterTianYuMouMobileRuntime().Value.BeginMouTianYu(f, op);
}
