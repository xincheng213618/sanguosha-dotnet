#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TianYuProgramCapability(Func<RuntimeEntries.CharacterTianYuOlEntries> getCharacterTianYuOlRuntime) : ITianYuProgramHost
{
    public SkillProgramStepOutcome SaodiExpandTargets(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTianYuOlRuntime().SaodiProgramExpandTargets(frame, effect);
    public SkillProgramStepOutcome ZhuitaoMarkTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTianYuOlRuntime().ZhuitaoProgramMarkTarget(frame, effect);
    public SkillProgramStepOutcome ZhuitaoRevoke(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTianYuOlRuntime().ZhuitaoProgramRevoke(frame, effect);
}
