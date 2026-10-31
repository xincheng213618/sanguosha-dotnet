#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ZhangJiProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangJiOlRuntime>> getCharacterZhangJiOlRuntime) : IZhangJiProgramHost
{
    public SkillProgramStepOutcome BeginZhangJiOperation(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangJiOlRuntime().Value.BeginZhangJiOperation(f, op);
}
