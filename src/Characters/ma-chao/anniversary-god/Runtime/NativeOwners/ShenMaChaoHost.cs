#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenMaChaoHost(Func<RuntimeEntries.CharacterMaChaoAnniversaryGodEntries> getCharacterMaChaoAnniversaryGodRuntime) : IShenMaChaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaChaoAnniversaryGodRuntime().BeginShenMaChao(f, op);
}
