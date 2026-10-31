#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTieredRoundZeroUsesEntries
{
    internal void AssertTieredRoundConversionUses() => _resolve().AssertTieredRoundConversionUses();
    internal void BeginTieredRoundForcedZeroSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit) => _resolve().BeginTieredRoundForcedZeroSlash(actor, target, kind, source, borrowed, countsTowardLimit);
    internal bool ContinueTieredRoundZeroSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueTieredRoundZeroSimpleUse(id, continuation);
    internal void ExecuteTieredRoundZeroPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().ExecuteTieredRoundZeroPlay(actor, action);
    internal bool FinishTieredRoundZeroAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishTieredRoundZeroAttack(attack, out paused);
    internal bool IsIssuedTieredRoundUse(global::CardGame.Core.CardUseFrame use, bool zeroOnly = false) => _resolve().IsIssuedTieredRoundUse(use, zeroOnly);
    internal bool IsTieredRoundZeroUse(long id) => _resolve().IsTieredRoundZeroUse(id);
    internal void IssueTieredRoundConversionUse(global::CardGame.Core.CardUseFrame use) => _resolve().IssueTieredRoundConversionUse(use);
    internal bool TieredRoundIssuedActorMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.TieredRoundConversionUseReceipt receipt) => _resolve().TieredRoundIssuedActorMatches(use, receipt);
    internal global::CardGame.Core.CardActionContext? TieredRoundIssuedOriginalOutputAction(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.TieredRoundConversionUseReceipt receipt) => _resolve().TieredRoundIssuedOriginalOutputAction(use, receipt);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong) => _resolve().TieredRoundZeroForcedSlashChoices(actor, target, originalId, qinglong);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> TieredRoundZeroForcedSlashRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, bool qinglong) => _resolve().TieredRoundZeroForcedSlashRules(actor, target, qinglong);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> TieredRoundZeroPlayActions(global::CardGame.Core.CharacterState actor) => _resolve().TieredRoundZeroPlayActions(actor);
    internal global::CardGame.Core.Card TieredRoundZeroRepresentation(long id) => _resolve().TieredRoundZeroRepresentation(id);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> TieredRoundZeroRules(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind? output = default, bool responseUse = false, bool dyingUse = false) => _resolve().TieredRoundZeroRules(owner, output, responseUse, dyingUse);
    internal bool TryExecuteTieredRoundZeroPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().TryExecuteTieredRoundZeroPlay(actor, action);
    internal bool TryResolveTieredRoundZeroForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance) => _resolve().TryResolveTieredRoundZeroForcedSlash(selected, advance);
    internal bool TrySubmitTieredRoundZeroPlay(global::CardGame.Core.PlayCardCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().TrySubmitTieredRoundZeroPlay(command, out result);
}
