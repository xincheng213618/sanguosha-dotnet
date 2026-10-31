#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SunHaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunHaoOlRuntime>> getCharacterSunHaoOlRuntime) : ISunHaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunHaoOlRuntime().Value.BeginSunHao(f, op);
}
