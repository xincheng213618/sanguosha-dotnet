#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhouQunCapability(Func<RuntimeEntries.CharacterZhouQunOlEntries> getCharacterZhouQunOlRuntime) : IZhouQunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterZhouQunOlRuntime().BeginZhouQun(frame, op);
}
