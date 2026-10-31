#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDiscardObserversEntries
{
    internal bool IsSequentialDiscardProgramDying() => _resolve().IsSequentialDiscardProgramDying();
    internal bool SequentialDiscardDescendantsRide(int rootIndex) => _resolve().SequentialDiscardDescendantsRide(rootIndex);
    internal bool SequentialDiscardFirstMovementChild(ResolutionFrame child, ProgramSkillFrame root, ProgramSequentialDiscardPayment paid) => _resolve().SequentialDiscardFirstMovementChild(child, root, paid);
    internal ProgramSkillFrame? SequentialDiscardPaymentObserverRoot() => _resolve().SequentialDiscardPaymentObserverRoot();
    internal ProgramSkillFrame? SequentialDiscardTopPaymentObserverRoot() => _resolve().SequentialDiscardTopPaymentObserverRoot();
}
