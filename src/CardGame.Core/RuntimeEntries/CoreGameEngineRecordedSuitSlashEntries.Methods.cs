#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecordedSuitSlashEntries
{
    internal void ClearRecordedSuitSlash(CardUseFrame use) => _resolve().ClearRecordedSuitSlash(use);
    internal bool IsRecordedSuitResponseAllowed(CharacterState responder, IReadOnlyList<Card> cards, CardKind kind) => _resolve().IsRecordedSuitResponseAllowed(responder, cards, kind);
    internal int RecordedSuitSlashDamage(GameEngine.IDamageAttempt attack) => _resolve().RecordedSuitSlashDamage(attack);
    internal IEnumerable<ProgramRecordedSuitSlashReceipt> RecordedSuitSlashReceipts(long frameId, int target) => _resolve().RecordedSuitSlashReceipts(frameId, target);
    internal void RevealRecordedSuitSlash(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().RevealRecordedSuitSlash(frame, effect);
}
