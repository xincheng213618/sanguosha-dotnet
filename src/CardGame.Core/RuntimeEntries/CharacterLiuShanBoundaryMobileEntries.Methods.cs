#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuShanBoundaryMobileEntries
{
    internal global::CardGame.Core.RuleQueryEvaluation ApplyBoundaryLiuShanHandLimit(global::CardGame.Core.CharacterState player, global::CardGame.Core.RuleQueryEvaluation ordinary) => _resolve().ApplyBoundaryLiuShanHandLimit(player, ordinary);
    internal bool HasBoundaryLiuShanMaximumHandLimit(global::CardGame.Core.CharacterState player) => _resolve().HasBoundaryLiuShanMaximumHandLimit(player);
    internal bool QualifyBoundaryLiuShanRuoyu(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().QualifyBoundaryLiuShanRuoyu(gate);
}
