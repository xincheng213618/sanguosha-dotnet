#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramShanheSourceWarElephantEntries
{
    internal bool RestoreShanheWarElephantCounterSlashParent(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().RestoreShanheWarElephantCounterSlashParent(completion);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateShanheWarElephantCounterParentProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().SeparateShanheWarElephantCounterParentProcessing(attack, processing);
    internal bool ShanheWarElephantCannotTurnOver(global::CardGame.Core.CharacterState owner) => _resolve().ShanheWarElephantCannotTurnOver(owner);
    internal bool ShanheWarElephantHas(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().ShanheWarElephantHas(owner, skillId);
    internal bool ShanheWarElephantUnlimitedCardDistance(global::CardGame.Core.ProgramCardDistanceQuery query) => _resolve().ShanheWarElephantUnlimitedCardDistance(query);
    internal global::CardGame.Core.SkillProgramStepOutcome UseShanheWarElephantCounterSlash(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().UseShanheWarElephantCounterSlash(supplied);
    internal bool IsShanheWarElephantMode { get => _resolve().IsShanheWarElephantMode; }
}
