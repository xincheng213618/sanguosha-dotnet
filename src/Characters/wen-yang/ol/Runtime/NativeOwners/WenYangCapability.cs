#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WenYangCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWenYangOlRuntime>> getCharacterWenYangOlRuntime) : IWenYangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWenYangOlRuntime().Value.BeginWenYang(f, op);
}
