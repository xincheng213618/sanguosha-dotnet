#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramShanheSourceWarElephantEntries
{
    internal bool RestoreShanheWarElephantCounterSlashParent(GameEngine.AttackCompletionReceipt completion) => _resolve().RestoreShanheWarElephantCounterSlashParent(completion);
    internal bool ShanheWarElephantCannotTurnOver(CharacterState owner) => _resolve().ShanheWarElephantCannotTurnOver(owner);
    internal bool ShanheWarElephantHas(CharacterState owner, string skillId) => _resolve().ShanheWarElephantHas(owner, skillId);
    internal bool ShanheWarElephantUnlimitedCardDistance(ProgramCardDistanceQuery query) => _resolve().ShanheWarElephantUnlimitedCardDistance(query);
    internal SkillProgramStepOutcome UseShanheWarElephantCounterSlash(ProgramSkillFrame supplied) => _resolve().UseShanheWarElephantCounterSlash(supplied);
    internal bool IsShanheWarElephantMode { get => _resolve().IsShanheWarElephantMode; }
}
