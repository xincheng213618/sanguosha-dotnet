#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordMetadata(Func<RuntimeEntries.CoreProgramActualPlaySlashLossEntries> getCoreProgramActualPlaySlashLossRuntime, Func<RuntimeEntries.CoreGameEngineGainGiftProgramsEntries> getCoreGameEngineGainGiftProgramsRuntime, Func<RuntimeEntries.CharacterHuaTuoHappyGodEntries> getCharacterHuaTuoHappyGodRuntime, Func<RuntimeEntries.CharacterGuoZhaoAnniversaryEntries> getCharacterGuoZhaoAnniversaryRuntime, Func<RuntimeEntries.CharacterZhangHengAnniversaryEntries> getCharacterZhangHengAnniversaryRuntime) : IMovementRecordMetadata
{
    public ActualPlayPhysicalSlashLossStamp? CaptureActualPlaySlashLoss(Card card, CardLocation from, CardLocation to) => getCoreProgramActualPlaySlashLossRuntime().CaptureActualPlaySlashLoss(card, from, to);
    public RedOwnedLossMaterial? CaptureRedOwnedLoss(Card card, CardLocation from, CardLocation to, long sequence) => getCoreGameEngineGainGiftProgramsRuntime().CaptureRedOwnedLoss(card, from, to, sequence);
    public HappyMedicineHandLoss? CaptureHappyHuaTuoHandLoss(Card card, CardLocation from, CardLocation to, long sequence) => getCharacterHuaTuoHappyGodRuntime().CaptureHappyHuaTuoHandLoss(card, from, to, sequence);
    public AnniversaryGuoZhaoLoss? CaptureAnniversaryGuoZhaoLoss(Card card, CardLocation from, CardLocation to, int sequence) => getCharacterGuoZhaoAnniversaryRuntime().CaptureAnniversaryGuoZhaoLoss(card, from, to, sequence);
    public AnniversaryZhangHengMovement? CaptureAnniversaryZhangHengMovement(Card card, CardLocation from, CardLocation to) => getCharacterZhangHengAnniversaryRuntime().CaptureAnniversaryZhangHengMovement(card, from, to);
}
