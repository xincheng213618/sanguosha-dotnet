#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardSnapshotFactsRuntime
{
    EquipmentSlot? ICardSnapshotFacts.BoneSlot(Card card) => _characterWeiYanHappyGodOperations.HappyBoneSlot(card);
    int? ICardSnapshotFacts.BoneOwnerSeat(int cardId) => _characterWeiYanHappyGodOperations.HappyBoneRecord(cardId)?.Source.OwnerSeat;
    bool ICardSnapshotFacts.IsCarrier(int cardId) => _characterDiaoChanLeOperations.IsDiouCarrier(cardId);
    string? ICardSnapshotFacts.VisibleSkillMark(int cardId) => _cardSkillMarkProjectorOperations.Get().Label(cardId);
    CardKind? ICardSnapshotFacts.QinJudgmentSlot(int cardId) => _characterDaQiaoLeOperations.ZixiQinJudgmentSlot(cardId);
    bool ICardSnapshotFacts.IsQinJudgmentCard(int cardId) => _characterDaQiaoLeOperations.IsZixiQinJudgmentCard(cardId);
    CardKind ICardSnapshotFacts.JudgmentEffectiveKind(Card card) => _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(card);
    void ICardSkillMarkSources.Project(int cardId, ref CardSkillMarkProjection projection) => _programPipelineHooksOperations._programPipelineHooks.ProjectCardSkillMarks(cardId, ref projection);
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 70)]
    internal void ProjectRegisteredCardEntitySkillLabels(int cardId, ref CardSkillMarkProjection projection)
    {
        // Preserve the existing providers' insertion point and eager fan-out.
        foreach (var label in _programPipelineHooksOperations._programPipelineHooks.CardEntitySkillLabels(cardId))
            projection.Append(label);
    }
}
