#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class YuanHuanCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYuanHuanOlRuntime>> getCharacterYuanHuanOlRuntime) : IYuanHuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYuanHuanOlRuntime().Value.BeginYuanHuan(f, op);
}
