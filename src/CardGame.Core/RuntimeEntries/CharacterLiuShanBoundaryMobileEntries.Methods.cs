#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuShanBoundaryMobileEntries
{
    internal RuleQueryEvaluation ApplyBoundaryLiuShanHandLimit(CharacterState player, RuleQueryEvaluation ordinary) => _resolve().ApplyBoundaryLiuShanHandLimit(player, ordinary);
    internal bool HasBoundaryLiuShanMaximumHandLimit(CharacterState player) => _resolve().HasBoundaryLiuShanMaximumHandLimit(player);
    internal bool QualifyBoundaryLiuShanRuoyu(ProgramTriggerLegalityContext gate) => _resolve().QualifyBoundaryLiuShanRuoyu(gate);
}
