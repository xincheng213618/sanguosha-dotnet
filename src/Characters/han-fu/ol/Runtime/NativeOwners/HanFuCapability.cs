#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HanFuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHanFuOlRuntime>> getCharacterHanFuOlRuntime) : IHanFuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHanFuOlRuntime().Value.BeginHanFu(f, op);
}
