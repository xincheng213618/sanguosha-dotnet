#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardSnapshotFactsRuntime
{
    EquipmentSlot? global::CardGame.Core.ICardSnapshotFacts.BoneSlot(Card card) => _characterWeiYanHappyGodOperations.HappyBoneSlot(card);
    int? global::CardGame.Core.ICardSnapshotFacts.BoneOwnerSeat(int cardId) => _characterWeiYanHappyGodOperations.HappyBoneRecord(cardId)?.Source.OwnerSeat;
    bool global::CardGame.Core.ICardSnapshotFacts.IsCarrier(int cardId) => _characterDiaoChanLeOperations.IsDiouCarrier(cardId);
    string? global::CardGame.Core.ICardSnapshotFacts.VisibleSkillMark(int cardId) => _cardSkillMarkProjectorOperations.Get().Label(cardId);
    CardKind? global::CardGame.Core.ICardSnapshotFacts.QinJudgmentSlot(int cardId) => _characterDaQiaoLeOperations.ZixiQinJudgmentSlot(cardId);
    bool global::CardGame.Core.ICardSnapshotFacts.IsQinJudgmentCard(int cardId) => _characterDaQiaoLeOperations.IsZixiQinJudgmentCard(cardId);
    CardKind global::CardGame.Core.ICardSnapshotFacts.JudgmentEffectiveKind(Card card) => _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(card);
    void global::CardGame.Core.ICardSkillMarkSources.Project(int cardId, ref CardSkillMarkProjection projection) => _programPipelineHooksOperations._programPipelineHooks.ProjectCardSkillMarks(cardId, ref projection);
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 70)]
    internal void ProjectRegisteredCardEntitySkillLabels(int cardId, ref CardSkillMarkProjection projection)
    {
        // Preserve the existing providers' insertion point and eager fan-out.
        foreach (var label in _programPipelineHooksOperations._programPipelineHooks.CardEntitySkillLabels(cardId))
            projection.Append(label);
    }
}
