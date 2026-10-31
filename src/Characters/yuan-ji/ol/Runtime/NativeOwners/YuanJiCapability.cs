#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class YuanJiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYuanJiOlRuntime>> getCharacterYuanJiOlRuntime) : IYuanJiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYuanJiOlRuntime().Value.ResolveYuanJi(f, op);
}
