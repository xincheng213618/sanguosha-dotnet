namespace CardGame.Core;
internal sealed partial class MouJuShouRuntime
{
 [ProgramRuleObserver(ProgramRulePhase.BeforeEvent,36900)]private void ObserveTurn(IGameEvent payload)
 {
  if(payload is TurnStartedEvent start)foreach(var seat in facts.Seats)if(facts.Losses(seat).Any(l=>l.Turn!=start.TurnNumber))native.SetLosses(seat,facts.Losses(seat).Where(l=>l.Turn==start.TurnNumber).ToArray());
 }
 internal void ObserveUse(int actor,IReadOnlyList<int> materials)
 {
  foreach(var f in frames.OfType<ProgramSkillFrame>().Where(f=>f.MouJuShou is{Operation:SkillProgramEffectOp.ResolveMouJuShouZuoJun}r&&r.Actor==actor&&r.Bound.Intersect(materials).Any()).ToArray())frames.Replace(f.Id,f with{MouJuShou=f.MouJuShou! with{Bound=f.MouJuShou.Bound.Except(materials).ToArray()}});
 }
}
