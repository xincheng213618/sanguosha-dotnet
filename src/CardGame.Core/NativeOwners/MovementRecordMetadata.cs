#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordMetadata(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreProgramActualPlaySlashLossRuntime>> getCoreProgramActualPlaySlashLossRuntime, Func<RuntimeEntries.CoreGameEngineGainGiftProgramsEntries> getCoreGameEngineGainGiftProgramsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHuaTuoHappyGodRuntime>> getCharacterHuaTuoHappyGodRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGuoZhaoAnniversaryRuntime>> getCharacterGuoZhaoAnniversaryRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangHengAnniversaryRuntime>> getCharacterZhangHengAnniversaryRuntime) : IMovementRecordMetadata
{
    public ActualPlayPhysicalSlashLossStamp? CaptureActualPlaySlashLoss(Card card, CardLocation from, CardLocation to) => getCoreProgramActualPlaySlashLossRuntime().Value.CaptureActualPlaySlashLoss(card, from, to);
    public RedOwnedLossMaterial? CaptureRedOwnedLoss(Card card, CardLocation from, CardLocation to, long sequence) => getCoreGameEngineGainGiftProgramsRuntime().CaptureRedOwnedLoss(card, from, to, sequence);
    public HappyMedicineHandLoss? CaptureHappyHuaTuoHandLoss(Card card, CardLocation from, CardLocation to, long sequence) => getCharacterHuaTuoHappyGodRuntime().Value.CaptureHappyHuaTuoHandLoss(card, from, to, sequence);
    public AnniversaryGuoZhaoLoss? CaptureAnniversaryGuoZhaoLoss(Card card, CardLocation from, CardLocation to, int sequence) => getCharacterGuoZhaoAnniversaryRuntime().Value.CaptureAnniversaryGuoZhaoLoss(card, from, to, sequence);
    public AnniversaryZhangHengMovement? CaptureAnniversaryZhangHengMovement(Card card, CardLocation from, CardLocation to) => getCharacterZhangHengAnniversaryRuntime().Value.CaptureAnniversaryZhangHengMovement(card, from, to);
}
