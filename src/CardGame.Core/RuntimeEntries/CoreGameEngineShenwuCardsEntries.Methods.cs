#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShenwuCardsEntries
{
    internal void AddShenwuTrickActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> playableCards) => _resolve().AddShenwuTrickActions(actions, actor, playableCards);
    internal bool AllowsShenwuTransplantActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsShenwuTransplantActivation(gate);
    internal bool IsShenwuTransplantProgram(ProgramSkillFrame frame) => _resolve().IsShenwuTransplantProgram(frame);
    internal void RequireShenwuTrickAction(CharacterState source, Card card, LegalActionKind kind, int? target) => _resolve().RequireShenwuTrickAction(source, card, kind, target);
    internal void ResolveShenwuSowBeans(CharacterState source, Card card) => _resolve().ResolveShenwuSowBeans(source, card);
    internal void ResolveShenwuSowBeansEffect(NullificationWindowFrame pending) => _resolve().ResolveShenwuSowBeansEffect(pending);
    internal void ResolveShenwuTransplantWood(CharacterState source, CharacterState target, Card card) => _resolve().ResolveShenwuTransplantWood(source, target, card);
    internal void ResolveShenwuTransplantWoodEffect(NullificationWindowFrame pending) => _resolve().ResolveShenwuTransplantWoodEffect(pending);
    internal string ShenwuTransplantSource(long id) => _resolve().ShenwuTransplantSource(id);
    internal bool ShenwuTrickFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => _resolve().ShenwuTrickFrameRidesOn(ride, beneath);
    internal bool TryAdvanceShenwuTransplantAi() => _resolve().TryAdvanceShenwuTransplantAi();
    internal bool TryCompleteShenwuTransplantProgram(ProgramSkillFrame frame, bool completed) => _resolve().TryCompleteShenwuTransplantProgram(frame, completed);
}
