#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenCaoPiCapability(Func<RuntimeEntries.CharacterCaoPiAnniversaryGodEntries> getCharacterCaoPiAnniversaryGodRuntime) : IShenCaoPiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterCaoPiAnniversaryGodRuntime().BeginShenCaoPi(frame, op);
}
