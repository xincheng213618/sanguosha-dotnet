#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDiscardObserversEntries
{
    internal bool IsSequentialDiscardProgramDying() => _resolve().IsSequentialDiscardProgramDying();
    internal bool SequentialDiscardDescendantsRide(int rootIndex) => _resolve().SequentialDiscardDescendantsRide(rootIndex);
    internal bool SequentialDiscardFirstMovementChild(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ProgramSequentialDiscardPayment paid) => _resolve().SequentialDiscardFirstMovementChild(child, root, paid);
    internal global::CardGame.Core.ProgramSkillFrame? SequentialDiscardPaymentObserverRoot() => _resolve().SequentialDiscardPaymentObserverRoot();
    internal global::CardGame.Core.ProgramSkillFrame? SequentialDiscardTopPaymentObserverRoot() => _resolve().SequentialDiscardTopPaymentObserverRoot();
}
