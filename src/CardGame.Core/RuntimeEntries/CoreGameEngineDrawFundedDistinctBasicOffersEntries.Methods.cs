#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicOffersEntries
{
    internal global::CardGame.Core.PendingDecision? AddDrawFundedDistinctBasicChoices(global::CardGame.Core.PendingDecision? prompt) => _resolve().AddDrawFundedDistinctBasicChoices(prompt);
    internal global::CardGame.Core.GameEngine.DrawFundedBasicNeed? DrawFundedDistinctBasicContext(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ResolutionFrame? suppliedTop = default) => _resolve().DrawFundedDistinctBasicContext(prompt, suppliedTop);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> DrawFundedDistinctBasicForcedSlashRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, bool qinglong) => _resolve().DrawFundedDistinctBasicForcedSlashRules(actor, target, qinglong);
    internal bool DrawFundedDistinctBasicNameAvailable(int actorSeat, string ledger, global::CardGame.Core.CardKind kind) => _resolve().DrawFundedDistinctBasicNameAvailable(actorSeat, ledger, kind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> DrawFundedDistinctBasicNativePlayActions(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => _resolve().DrawFundedDistinctBasicNativePlayActions(actor, source, kind);
    internal bool DrawFundedDistinctBasicNativeResponseLegal(global::CardGame.Core.CharacterState actor, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need, global::CardGame.Core.CardKind kind, int? targetSeat) => _resolve().DrawFundedDistinctBasicNativeResponseLegal(actor, need, kind, targetSeat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> DrawFundedDistinctBasicPlayActions(global::CardGame.Core.CharacterState actor) => _resolve().DrawFundedDistinctBasicPlayActions(actor);
    internal bool DrawFundedDistinctBasicQualified(global::CardGame.Core.CharacterState actor) => _resolve().DrawFundedDistinctBasicQualified(actor);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DrawFundedDistinctBasicResponseChoices(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need) => _resolve().DrawFundedDistinctBasicResponseChoices(prompt, need);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> DrawFundedDistinctBasicRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind? output = default, bool responseUse = false, bool dyingUse = false) => _resolve().DrawFundedDistinctBasicRules(actor, output, responseUse, dyingUse);
    internal bool HasDrawFundedDistinctBasicDodge(global::CardGame.Core.CharacterState actor) => _resolve().HasDrawFundedDistinctBasicDodge(actor);
    internal bool HasDrawFundedDistinctBasicDyingResponse(global::CardGame.Core.CharacterState actor) => _resolve().HasDrawFundedDistinctBasicDyingResponse(actor);
    internal bool HasDrawFundedDistinctBasicProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => _resolve().HasDrawFundedDistinctBasicProgramSlash(actor, targets);
    internal bool TryExecuteDrawFundedDistinctBasicPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().TryExecuteDrawFundedDistinctBasicPlay(actor, action);
    internal bool TrySubmitDrawFundedDistinctBasicPlay(global::CardGame.Core.PlayCardCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().TrySubmitDrawFundedDistinctBasicPlay(command, out result);
}
