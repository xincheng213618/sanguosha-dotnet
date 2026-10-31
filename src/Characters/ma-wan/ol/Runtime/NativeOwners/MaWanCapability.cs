#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MaWanCapability(Func<RuntimeEntries.CharacterMaWanOlEntries> getCharacterMaWanOlRuntime) : IMaWanProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveHunjiangSimultaneousChoices ? getCharacterMaWanOlRuntime().BeginMaWanVotes(f) : getCharacterMaWanOlRuntime().BeginMaWanTargets(f);
}
