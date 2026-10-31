#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenCaoPiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoPiAnniversaryGodEntries> getCharacterCaoPiAnniversaryGodRuntime) : IShenCaoPiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterCaoPiAnniversaryGodRuntime().BeginShenCaoPi(frame, op);
}
