#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhouQunCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouQunOlEntries> getCharacterZhouQunOlRuntime) : IZhouQunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterZhouQunOlRuntime().BeginZhouQun(frame, op);
}
