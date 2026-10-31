#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTieredRoundZeroUsesEntries
{
    internal void BeginTieredRoundForcedZeroSlash(CharacterState actor, CharacterState target, CardKind kind, CardConversionSource source, NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit) => _resolve().BeginTieredRoundForcedZeroSlash(actor, target, kind, source, borrowed, countsTowardLimit);
    internal bool ContinueTieredRoundZeroSimpleUse(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueTieredRoundZeroSimpleUse(id, continuation);
    internal void ExecuteTieredRoundZeroPlay(CharacterState actor, LegalAction action) => _resolve().ExecuteTieredRoundZeroPlay(actor, action);
    internal bool FinishTieredRoundZeroAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishTieredRoundZeroAttack(attack, out paused);
    internal bool IsIssuedTieredRoundUse(CardUseFrame use, bool zeroOnly = false) => _resolve().IsIssuedTieredRoundUse(use, zeroOnly);
    internal bool IsTieredRoundZeroUse(long id) => _resolve().IsTieredRoundZeroUse(id);
    internal void IssueTieredRoundConversionUse(CardUseFrame use) => _resolve().IssueTieredRoundConversionUse(use);
    internal bool TieredRoundIssuedActorMatches(CardUseFrame use, TieredRoundConversionUseReceipt receipt) => _resolve().TieredRoundIssuedActorMatches(use, receipt);
    internal CardActionContext? TieredRoundIssuedOriginalOutputAction(CardUseFrame use, TieredRoundConversionUseReceipt receipt) => _resolve().TieredRoundIssuedOriginalOutputAction(use, receipt);
    internal IReadOnlyList<PromptChoice> TieredRoundZeroForcedSlashChoices(CharacterState actor, CharacterState target, long originalId, bool qinglong) => _resolve().TieredRoundZeroForcedSlashChoices(actor, target, originalId, qinglong);
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> TieredRoundZeroForcedSlashRules(CharacterState actor, CharacterState target, bool qinglong) => _resolve().TieredRoundZeroForcedSlashRules(actor, target, qinglong);
    internal IReadOnlyList<LegalAction> TieredRoundZeroPlayActions(CharacterState actor) => _resolve().TieredRoundZeroPlayActions(actor);
    internal Card TieredRoundZeroRepresentation(long id) => _resolve().TieredRoundZeroRepresentation(id);
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> TieredRoundZeroRules(CharacterState owner, CardKind? output = default, bool responseUse = false, bool dyingUse = false) => _resolve().TieredRoundZeroRules(owner, output, responseUse, dyingUse);
    internal bool TryExecuteTieredRoundZeroPlay(CharacterState actor, LegalAction action) => _resolve().TryExecuteTieredRoundZeroPlay(actor, action);
    internal bool TryResolveTieredRoundZeroForcedSlash(PromptChoice selected, bool advance) => _resolve().TryResolveTieredRoundZeroForcedSlash(selected, advance);
    internal bool TrySubmitTieredRoundZeroPlay(PlayCardCommand command, scoped out CommandResult result) => _resolve().TrySubmitTieredRoundZeroPlay(command, out result);
}
