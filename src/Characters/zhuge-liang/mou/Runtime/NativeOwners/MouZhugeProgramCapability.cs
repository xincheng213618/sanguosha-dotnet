#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhugeProgramCapability(Func<RuntimeEntries.CharacterZhugeLiangMouEntries> getCharacterZhugeLiangMouRuntime) : IMouZhugeProgramHost
{
    public SkillProgramStepOutcome ExecuteMouZhuge(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhugeLiangMouRuntime().ExecuteMouZhuge(frame, effect);
}
