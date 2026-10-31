#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MaYuanYiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterMaYuanYiOlRuntime>> getCharacterMaYuanYiOlRuntime) : IMaYuanYiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaYuanYiOlRuntime().Value.BeginMaYuanYi(f, op);
}
