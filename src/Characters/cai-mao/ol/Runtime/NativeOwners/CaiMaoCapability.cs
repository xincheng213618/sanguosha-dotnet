#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaiMaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaiMaoOlRuntime>> getCharacterCaiMaoOlRuntime) : ICaiMaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaiMaoOlRuntime().Value.BeginCaiMao(f, op);
}
