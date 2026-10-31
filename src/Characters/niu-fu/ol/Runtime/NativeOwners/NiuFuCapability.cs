#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class NiuFuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterNiuFuOlRuntime>> getCharacterNiuFuOlRuntime) : INiuFuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterNiuFuOlRuntime().Value.BeginNiuFu(f, op);
}
