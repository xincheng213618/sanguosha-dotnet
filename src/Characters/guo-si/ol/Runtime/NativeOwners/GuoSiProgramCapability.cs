#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GuoSiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoSiOlEntries> getCharacterGuoSiOlRuntime) : IGuoSiProgramHost
{
    public SkillProgramStepOutcome ExecuteGuoSi(SkillProgramEffectOp op, ProgramSkillFrame f) => getCharacterGuoSiOlRuntime().ExecuteGuoSi(op, f);
}
