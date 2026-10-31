#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuoXianOlEntries
{
    internal bool AdvanceDailiSubtree() => _resolve().AdvanceDailiSubtree();
    internal bool CanOfferDaili(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferDaili(candidate, trigger, context);
    internal void CompleteDailiAfterPublicReveal(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteDailiAfterPublicReveal(frame);
    internal bool ContinueIssuedDaili(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueIssuedDaili(frame);
    internal long? CurrentHandEntry(int owner, int id) => _resolve().CurrentHandEntry(owner, id);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PublicHandEntityRevealedEvent> CurrentPublicHandReveals() => _resolve().CurrentPublicHandReveals();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardMovementRecord> DailiDrawnMovements(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DailiDrawnMovements(frame);
    internal bool DailiFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().DailiFirstChild(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? DailiObserverRoot() => _resolve().DailiObserverRoot();
    internal bool DrainDailiChildren(long id) => _resolve().DrainDailiChildren(id);
    internal bool HasDailiBeforeDamageObserver(long id) => _resolve().HasDailiBeforeDamageObserver(id);
    internal bool HasDailiDamageObserver(long id) => _resolve().HasDailiDamageObserver(id);
    internal bool IsDailiMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsDailiMovement(frame, effect, movement);
    internal void ObserveExplicitPublicHandReveal(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveExplicitPublicHandReveal(payload);
    internal int PubliclyRevealedHandCount(int owner) => _resolve().PubliclyRevealedHandCount(owner);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveDailiTurnEnd(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveDailiTurnEnd(supplied);
    internal bool ResumeDaili(long id) => _resolve().ResumeDaili(id);
    internal bool ReturnDailiMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnDailiMovement(frame);
    internal bool ValidDaili(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidDaili(frame);
    internal bool TracksPublicHandReveals { get => _resolve().TracksPublicHandReveals; }
}
