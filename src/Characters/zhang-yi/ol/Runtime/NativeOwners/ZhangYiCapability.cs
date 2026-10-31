#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhangYiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangYiOlRuntime>> getCharacterZhangYiOlRuntime) : IZhangYiProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangYiOlRuntime().Value.BeginZhangYi(f, op);
}
