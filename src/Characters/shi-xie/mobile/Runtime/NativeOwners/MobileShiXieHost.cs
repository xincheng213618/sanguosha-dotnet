#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiXieHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterShiXieMobileRuntime>> getCharacterShiXieMobileRuntime) : IMobileShiXieHost
{
    public SkillProgramStepOutcome Change(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterShiXieMobileRuntime().Value.ChangeMobileShiXieDistance(frame, op);
}
