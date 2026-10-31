#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundOnceCardlessNativeUsesEntries
{
    internal void BeginRoundOnceCardlessForcedSlash(CharacterState actor, CharacterState target, CardKind kind, CardConversionSource source, NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit) => _resolve().BeginRoundOnceCardlessForcedSlash(actor, target, kind, source, borrowed, countsTowardLimit);
    internal void ExecuteRoundOnceCardlessPlay(CharacterState actor, LegalAction action) => _resolve().ExecuteRoundOnceCardlessPlay(actor, action);
    internal bool IsRoundOnceCardlessBasicUse(long id) => _resolve().IsRoundOnceCardlessBasicUse(id);
    internal void IssueRoundOnceCardlessNativeUse(long id, CardConversionSource source) => _resolve().IssueRoundOnceCardlessNativeUse(id, source);
    internal IReadOnlyList<PromptChoice> RoundOnceCardlessForcedSlashChoices(CharacterState actor, CharacterState target, long originalId, bool qinglong) => _resolve().RoundOnceCardlessForcedSlashChoices(actor, target, originalId, qinglong);
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule)> RoundOnceCardlessForcedSlashRules(CharacterState actor, CharacterState target, bool qinglong) => _resolve().RoundOnceCardlessForcedSlashRules(actor, target, qinglong);
    internal Card RoundOnceCardlessNativeRepresentation(long id) => _resolve().RoundOnceCardlessNativeRepresentation(id);
    internal IReadOnlyList<LegalAction> RoundOnceCardlessPlayActions(CharacterState actor) => _resolve().RoundOnceCardlessPlayActions(actor);
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule)> RoundOnceCardlessRules(CharacterState actor, CardKind? output = default, bool forResponse = false) => _resolve().RoundOnceCardlessRules(actor, output, forResponse);
    internal bool TryExecuteRoundOnceCardlessPlay(CharacterState actor, LegalAction action) => _resolve().TryExecuteRoundOnceCardlessPlay(actor, action);
    internal bool TryResolveRoundOnceCardlessForcedSlash(PromptChoice selected, bool advance) => _resolve().TryResolveRoundOnceCardlessForcedSlash(selected, advance);
    internal bool TrySubmitRoundOnceCardlessPlay(PlayCardCommand command, scoped out CommandResult result) => _resolve().TrySubmitRoundOnceCardlessPlay(command, out result);
}
