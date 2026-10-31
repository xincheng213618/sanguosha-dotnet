#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class YanRouCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYanRouOlRuntime>> getCharacterYanRouOlRuntime) : IYanRouHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYanRouOlRuntime().Value.BeginYanRou(f, op);
}
