#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecordedSuitSlashEntries
{
    internal void AssertRecordedSuitSlashReceipts() => _resolve().AssertRecordedSuitSlashReceipts();
    internal void ClearRecordedSuitSlash(global::CardGame.Core.CardUseFrame use) => _resolve().ClearRecordedSuitSlash(use);
    internal bool IsRecordedSuitResponseAllowed(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind) => _resolve().IsRecordedSuitResponseAllowed(responder, cards, kind);
    internal int RecordedSuitSlashDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().RecordedSuitSlashDamage(attack);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramRecordedSuitSlashReceipt> RecordedSuitSlashReceipts(long frameId, int target) => _resolve().RecordedSuitSlashReceipts(frameId, target);
    internal void RevealRecordedSuitSlash(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().RevealRecordedSuitSlash(frame, effect);
}
