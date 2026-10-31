#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundOnceCardlessNativeUsesEntries
{
    internal void BeginRoundOnceCardlessForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit) => _resolve().BeginRoundOnceCardlessForcedSlash(actor, target, kind, source, borrowed, countsTowardLimit);
    internal void ExecuteRoundOnceCardlessPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().ExecuteRoundOnceCardlessPlay(actor, action);
    internal bool IsRoundOnceCardlessBasicUse(long id) => _resolve().IsRoundOnceCardlessBasicUse(id);
    internal void IssueRoundOnceCardlessNativeUse(long id, global::CardGame.Core.CardConversionSource source) => _resolve().IssueRoundOnceCardlessNativeUse(id, source);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong) => _resolve().RoundOnceCardlessForcedSlashChoices(actor, target, originalId, qinglong);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule)> RoundOnceCardlessForcedSlashRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, bool qinglong) => _resolve().RoundOnceCardlessForcedSlashRules(actor, target, qinglong);
    internal global::CardGame.Core.Card RoundOnceCardlessNativeRepresentation(long id) => _resolve().RoundOnceCardlessNativeRepresentation(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> RoundOnceCardlessPlayActions(global::CardGame.Core.CharacterState actor) => _resolve().RoundOnceCardlessPlayActions(actor);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule)> RoundOnceCardlessRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind? output = default, bool forResponse = false) => _resolve().RoundOnceCardlessRules(actor, output, forResponse);
    internal bool TryExecuteRoundOnceCardlessPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().TryExecuteRoundOnceCardlessPlay(actor, action);
    internal bool TryResolveRoundOnceCardlessForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance) => _resolve().TryResolveRoundOnceCardlessForcedSlash(selected, advance);
    internal bool TrySubmitRoundOnceCardlessPlay(global::CardGame.Core.PlayCardCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().TrySubmitRoundOnceCardlessPlay(command, out result);
}
