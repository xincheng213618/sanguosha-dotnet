#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardSnapshotFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries> getCharacterWeiYanHappyGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries> getCharacterDiaoChanLeRuntime, global::System.Func<global::CardGame.Core.CardSkillMarkProjector> getCardSkillMarkProjector, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries> getCharacterDaQiaoLeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, global::System.Func<global::CardGame.Core.ProgramPipelineHooks> getProgramPipelineHooks) : ICardSnapshotFacts, ICardSkillMarkSources
{
    public EquipmentSlot? BoneSlot(Card card) => getCharacterWeiYanHappyGodRuntime().HappyBoneSlot(card);
    public int? BoneOwnerSeat(int cardId) => getCharacterWeiYanHappyGodRuntime().HappyBoneRecord(cardId)?.Source.OwnerSeat;
    public bool IsCarrier(int cardId) => getCharacterDiaoChanLeRuntime().IsDiouCarrier(cardId);
    public string? VisibleSkillMark(int cardId) => getCardSkillMarkProjector().Label(cardId);
    public CardKind? QinJudgmentSlot(int cardId) => getCharacterDaQiaoLeRuntime().ZixiQinJudgmentSlot(cardId);
    public bool IsQinJudgmentCard(int cardId) => getCharacterDaQiaoLeRuntime().IsZixiQinJudgmentCard(cardId);
    public CardKind JudgmentEffectiveKind(Card card) => getCoreActualTurnsRuntime().GetJudgmentEffectiveCardKind(card);
    public void Project(int cardId, ref CardSkillMarkProjection projection) => getProgramPipelineHooks().ProjectCardSkillMarks(cardId, ref projection);
}
