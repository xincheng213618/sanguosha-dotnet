#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRequestedDeckBasicsEntries
{
    internal PendingDecision? AddRequestedDeckBasicChoices(PendingDecision? p) => _resolve().AddRequestedDeckBasicChoices(p);
    internal IReadOnlyList<Card> AppendRequestedDeckBasicCards(CharacterState actor, IReadOnlyList<Card> original) => _resolve().AppendRequestedDeckBasicCards(actor, original);
    internal void BeginRequestedDeckBasicView(PromptChoice choice) => _resolve().BeginRequestedDeckBasicView(choice);
    internal void CaptureRequestedDeckBasicPaid(Card card, CardLocation from, CardLocation to) => _resolve().CaptureRequestedDeckBasicPaid(card, from, to);
    internal void DispatchRequestedDeckBasicMaterial(RequestedDeckBasicIntent intent, Card card, IReadOnlyList<int> targets) => _resolve().DispatchRequestedDeckBasicMaterial(intent, card, targets);
    internal void DispatchRequestedDeckProgramSlash(RequestedDeckBasicIntent intent, Card card, IReadOnlyList<int> targets) => _resolve().DispatchRequestedDeckProgramSlash(intent, card, targets);
    internal bool HasRequestedDeckBasicSource(CharacterState actor, CardKind required) => _resolve().HasRequestedDeckBasicSource(actor, required);
    internal bool IsRequestedDeckBasicAnswer(ChoiceId id) => _resolve().IsRequestedDeckBasicAnswer(id);
    internal bool IsSelectedRequestedDeckBasicMaterial(int actor, int id) => _resolve().IsSelectedRequestedDeckBasicMaterial(actor, id);
    internal void PublishRequestedDeckBasicView(RequestedDeckBasicFrame view) => _resolve().PublishRequestedDeckBasicView(view);
    internal bool RequestedDeckBasicCardMatches(RequestedDeckBasicFrame view, Card card) => _resolve().RequestedDeckBasicCardMatches(view, card);
    internal IReadOnlyList<PromptChoice> RequestedDeckBasicChoices(RequestedDeckBasicFrame view) => _resolve().RequestedDeckBasicChoices(view);
    internal (RequestedDeckBasicIntent Intent, long OwnerId, long RequestId, int Cursor)? RequestedDeckBasicContext(PendingDecision p, ResolutionFrame? suppliedTop = default) => _resolve().RequestedDeckBasicContext(p, suppliedTop);
    internal CardKind RequestedDeckBasicKind(PendingDecision p, RequestedDeckBasicIntent intent) => _resolve().RequestedDeckBasicKind(p, intent);
    internal CardSnapshot[] RequestedDeckBasicPrivateCards(int viewer) => _resolve().RequestedDeckBasicPrivateCards(viewer);
    internal IEnumerable<(CardConversionSource Source, SkillProgramCardPolicy Policy)> RequestedDeckBasicSources(CharacterState actor, CardKind required) => _resolve().RequestedDeckBasicSources(actor, required);
    internal int[] RequestedDeckBasicTargets(RequestedDeckBasicFrame view, Card card) => _resolve().RequestedDeckBasicTargets(view, card);
    internal void ResolveRequestedDeckBasicAnswer(PromptChoice c) => _resolve().ResolveRequestedDeckBasicAnswer(c);
    internal IEnumerable<Card> SelectedRequestedDeckBasicCards(CharacterState actor) => _resolve().SelectedRequestedDeckBasicCards(actor);
    internal CommandResult SubmitRequestedDeckBasicAnswer(int actor, PromptId prompt, ChoiceId choice) => _resolve().SubmitRequestedDeckBasicAnswer(actor, prompt, choice);
    internal bool TryAdvanceRequestedDeckBasicAi() => _resolve().TryAdvanceRequestedDeckBasicAi();
}
