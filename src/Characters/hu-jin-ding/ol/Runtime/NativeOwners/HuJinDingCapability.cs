#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HuJinDingCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHuJinDingOlRuntime>> getCharacterHuJinDingOlRuntime) : IHuJinDingProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuJinDingOlRuntime().Value.BeginQingyuan(f, op);
}
