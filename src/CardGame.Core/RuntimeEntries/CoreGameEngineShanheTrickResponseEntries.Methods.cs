#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheTrickResponseEntries
{
    internal bool IsShanheTrickResponseProhibited(int sourceSeat, int responderSeat, CardKind incomingKind, IReadOnlyList<int> targetSeats) => _resolve().IsShanheTrickResponseProhibited(sourceSeat, responderSeat, incomingKind, targetSeats);
    internal bool ShanheTrickNativeResponseForbidden(ProgramNativeResponseQuery query) => _resolve().ShanheTrickNativeResponseForbidden(query);
    internal bool ShanheTrickNullificationBanned(int responderSeat) => _resolve().ShanheTrickNullificationBanned(responderSeat);
}
