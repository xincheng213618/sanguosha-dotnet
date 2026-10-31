#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiHuanJieHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHuanJieMobileShiRuntime>> getCharacterHuanJieMobileShiRuntime) : IMobileShiHuanJieHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterHuanJieMobileShiRuntime().Value.BeginShiHj(frame, op);
}
