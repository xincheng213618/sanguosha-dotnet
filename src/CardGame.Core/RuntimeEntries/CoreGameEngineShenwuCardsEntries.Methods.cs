#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShenwuCardsEntries
{
    internal void AddShenwuTrickActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> playableCards) => _resolve().AddShenwuTrickActions(actions, actor, playableCards);
    internal bool AllowsShenwuTransplantActivation(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsShenwuTransplantActivation(gate);
    internal void AssertShenwuCardState() => _resolve().AssertShenwuCardState();
    internal bool HasShenwuTrickPrompt() => _resolve().HasShenwuTrickPrompt();
    internal bool IsShenwuTransplantAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsShenwuTransplantAttackConsistent(attack, processing);
    internal bool IsShenwuTransplantProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsShenwuTransplantProgram(frame);
    internal void RequireShenwuTrickAction(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card, global::CardGame.Core.LegalActionKind kind, int? target) => _resolve().RequireShenwuTrickAction(source, card, kind, target);
    internal void ResolveShenwuSowBeans(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card) => _resolve().ResolveShenwuSowBeans(source, card);
    internal void ResolveShenwuSowBeansEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveShenwuSowBeansEffect(pending);
    internal void ResolveShenwuTransplantWood(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card) => _resolve().ResolveShenwuTransplantWood(source, target, card);
    internal void ResolveShenwuTransplantWoodEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveShenwuTransplantWoodEffect(pending);
    internal string ShenwuTransplantSource(long id) => _resolve().ShenwuTransplantSource(id);
    internal bool ShenwuTrickFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => _resolve().ShenwuTrickFrameRidesOn(ride, beneath);
    internal bool TryAdvanceShenwuTransplantAi() => _resolve().TryAdvanceShenwuTransplantAi();
    internal bool TryCompleteShenwuTransplantProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().TryCompleteShenwuTransplantProgram(frame, completed);
}
