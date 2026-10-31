#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanOlMagicRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CardEntitySkillLabel, 1)]
    internal string? MoDcQingshiCardLabel(int cardId) => HasMoDcQingshiCard(cardId) ? "倾世" : null;
    [ProgramPipelineHook(ProgramPipelinePhase.SelectDiscardBatchIndexes, 1)]
    internal int[]? MoDcRecaptureBatchIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => features.HasOperation(SkillProgramEffectOp.ResolveMoDiaoChanQingshiDiscard) ? MoDcRecaptureIndexes(batch, candidate.OwnerSeat) : null;
    [ProgramPipelineHook(ProgramPipelinePhase.FilterPlayPhaseActions, 1)]
    internal IReadOnlyList<LegalAction> FilterMoDiaoChanHandSelectionPipeline(CharacterState actor, IReadOnlyList<LegalAction> actions) => FilterMoDiaoChanHandSelection(actor, actions);
}
