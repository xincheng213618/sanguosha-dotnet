#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiXieHost(Func<RuntimeEntries.CharacterShiXieMobileEntries> getCharacterShiXieMobileRuntime) : IMobileShiXieHost
{
    public SkillProgramStepOutcome Change(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterShiXieMobileRuntime().ChangeMobileShiXieDistance(frame, op);
}
