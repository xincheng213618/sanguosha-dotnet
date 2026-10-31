#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TianYuProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterTianYuOlRuntime>> getCharacterTianYuOlRuntime) : ITianYuProgramHost
{
    public SkillProgramStepOutcome SaodiExpandTargets(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTianYuOlRuntime().Value.SaodiProgramExpandTargets(frame, effect);
    public SkillProgramStepOutcome ZhuitaoMarkTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTianYuOlRuntime().Value.ZhuitaoProgramMarkTarget(frame, effect);
    public SkillProgramStepOutcome ZhuitaoRevoke(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTianYuOlRuntime().Value.ZhuitaoProgramRevoke(frame, effect);
}
