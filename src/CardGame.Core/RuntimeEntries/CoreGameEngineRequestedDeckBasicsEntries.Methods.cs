#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRequestedDeckBasicsEntries
{
    internal global::CardGame.Core.PendingDecision? AddRequestedDeckBasicChoices(global::CardGame.Core.PendingDecision? p) => _resolve().AddRequestedDeckBasicChoices(p);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> AppendRequestedDeckBasicCards(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> original) => _resolve().AppendRequestedDeckBasicCards(actor, original);
    internal void BeginRequestedDeckBasicView(global::CardGame.Core.PromptChoice choice) => _resolve().BeginRequestedDeckBasicView(choice);
    internal void CaptureRequestedDeckBasicPaid(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().CaptureRequestedDeckBasicPaid(card, from, to);
    internal void DispatchRequestedDeckBasicMaterial(global::CardGame.Core.RequestedDeckBasicIntent intent, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().DispatchRequestedDeckBasicMaterial(intent, card, targets);
    internal void DispatchRequestedDeckProgramSlash(global::CardGame.Core.RequestedDeckBasicIntent intent, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().DispatchRequestedDeckProgramSlash(intent, card, targets);
    internal bool HasRequestedDeckBasicSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required) => _resolve().HasRequestedDeckBasicSource(actor, required);
    internal bool IsRequestedDeckBasicAnswer(global::CardGame.Core.ChoiceId id) => _resolve().IsRequestedDeckBasicAnswer(id);
    internal bool IsSelectedRequestedDeckBasicMaterial(int actor, int id) => _resolve().IsSelectedRequestedDeckBasicMaterial(actor, id);
    internal void PublishRequestedDeckBasicView(global::CardGame.Core.RequestedDeckBasicFrame view) => _resolve().PublishRequestedDeckBasicView(view);
    internal bool RequestedDeckBasicCardMatches(global::CardGame.Core.RequestedDeckBasicFrame view, global::CardGame.Core.Card card) => _resolve().RequestedDeckBasicCardMatches(view, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RequestedDeckBasicChoices(global::CardGame.Core.RequestedDeckBasicFrame view) => _resolve().RequestedDeckBasicChoices(view);
    internal (global::CardGame.Core.RequestedDeckBasicIntent Intent, long OwnerId, long RequestId, int Cursor)? RequestedDeckBasicContext(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ResolutionFrame? suppliedTop = default) => _resolve().RequestedDeckBasicContext(p, suppliedTop);
    internal global::CardGame.Core.CardKind RequestedDeckBasicKind(global::CardGame.Core.PendingDecision p, global::CardGame.Core.RequestedDeckBasicIntent intent) => _resolve().RequestedDeckBasicKind(p, intent);
    internal global::CardGame.Core.CardSnapshot[] RequestedDeckBasicPrivateCards(int viewer) => _resolve().RequestedDeckBasicPrivateCards(viewer);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> RequestedDeckBasicSources(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required) => _resolve().RequestedDeckBasicSources(actor, required);
    internal int[] RequestedDeckBasicTargets(global::CardGame.Core.RequestedDeckBasicFrame view, global::CardGame.Core.Card card) => _resolve().RequestedDeckBasicTargets(view, card);
    internal void ResolveRequestedDeckBasicAnswer(global::CardGame.Core.PromptChoice c) => _resolve().ResolveRequestedDeckBasicAnswer(c);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> SelectedRequestedDeckBasicCards(global::CardGame.Core.CharacterState actor) => _resolve().SelectedRequestedDeckBasicCards(actor);
    internal global::CardGame.Core.CommandResult SubmitRequestedDeckBasicAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId choice) => _resolve().SubmitRequestedDeckBasicAnswer(actor, prompt, choice);
    internal bool TryAdvanceRequestedDeckBasicAi() => _resolve().TryAdvanceRequestedDeckBasicAi();
}
