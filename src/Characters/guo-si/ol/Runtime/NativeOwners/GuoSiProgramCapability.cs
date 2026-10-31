#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GuoSiProgramCapability(Func<RuntimeEntries.CharacterGuoSiOlEntries> getCharacterGuoSiOlRuntime) : IGuoSiProgramHost
{
    public SkillProgramStepOutcome ExecuteGuoSi(SkillProgramEffectOp op, ProgramSkillFrame f) => getCharacterGuoSiOlRuntime().ExecuteGuoSi(op, f);
}
