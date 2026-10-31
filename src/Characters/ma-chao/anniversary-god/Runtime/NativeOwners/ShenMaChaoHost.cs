#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenMaChaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries> getCharacterMaChaoAnniversaryGodRuntime) : IShenMaChaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaChaoAnniversaryGodRuntime().BeginShenMaChao(f, op);
}
