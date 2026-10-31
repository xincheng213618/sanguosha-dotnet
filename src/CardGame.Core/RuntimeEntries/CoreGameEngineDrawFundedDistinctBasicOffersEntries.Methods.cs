#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicOffersEntries
{
    internal PendingDecision? AddDrawFundedDistinctBasicChoices(PendingDecision? prompt) => _resolve().AddDrawFundedDistinctBasicChoices(prompt);
    internal GameEngine.DrawFundedBasicNeed? DrawFundedDistinctBasicContext(PendingDecision prompt, ResolutionFrame? suppliedTop = default) => _resolve().DrawFundedDistinctBasicContext(prompt, suppliedTop);
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> DrawFundedDistinctBasicForcedSlashRules(CharacterState actor, CharacterState target, bool qinglong) => _resolve().DrawFundedDistinctBasicForcedSlashRules(actor, target, qinglong);
    internal bool DrawFundedDistinctBasicNameAvailable(int actorSeat, string ledger, CardKind kind) => _resolve().DrawFundedDistinctBasicNameAvailable(actorSeat, ledger, kind);
    internal IReadOnlyList<LegalAction> DrawFundedDistinctBasicNativePlayActions(CharacterState actor, CardConversionSource source, CardKind kind) => _resolve().DrawFundedDistinctBasicNativePlayActions(actor, source, kind);
    internal bool DrawFundedDistinctBasicNativeResponseLegal(CharacterState actor, GameEngine.DrawFundedBasicNeed need, CardKind kind, int? targetSeat) => _resolve().DrawFundedDistinctBasicNativeResponseLegal(actor, need, kind, targetSeat);
    internal IReadOnlyList<LegalAction> DrawFundedDistinctBasicPlayActions(CharacterState actor) => _resolve().DrawFundedDistinctBasicPlayActions(actor);
    internal bool DrawFundedDistinctBasicQualified(CharacterState actor) => _resolve().DrawFundedDistinctBasicQualified(actor);
    internal IReadOnlyList<PromptChoice> DrawFundedDistinctBasicResponseChoices(PendingDecision prompt, GameEngine.DrawFundedBasicNeed need) => _resolve().DrawFundedDistinctBasicResponseChoices(prompt, need);
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> DrawFundedDistinctBasicRules(CharacterState actor, CardKind? output = default, bool responseUse = false, bool dyingUse = false) => _resolve().DrawFundedDistinctBasicRules(actor, output, responseUse, dyingUse);
    internal bool HasDrawFundedDistinctBasicDodge(CharacterState actor) => _resolve().HasDrawFundedDistinctBasicDodge(actor);
    internal bool HasDrawFundedDistinctBasicDyingResponse(CharacterState actor) => _resolve().HasDrawFundedDistinctBasicDyingResponse(actor);
    internal bool HasDrawFundedDistinctBasicProgramSlash(CharacterState actor, IEnumerable<int> targets) => _resolve().HasDrawFundedDistinctBasicProgramSlash(actor, targets);
    internal bool TryExecuteDrawFundedDistinctBasicPlay(CharacterState actor, LegalAction action) => _resolve().TryExecuteDrawFundedDistinctBasicPlay(actor, action);
    internal bool TrySubmitDrawFundedDistinctBasicPlay(PlayCardCommand command, scoped out CommandResult result) => _resolve().TrySubmitDrawFundedDistinctBasicPlay(command, out result);
}
