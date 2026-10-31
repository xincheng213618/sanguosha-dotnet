#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheTrickResponseEntries
{
    internal bool IsShanheTrickResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => _resolve().IsShanheTrickResponseProhibited(sourceSeat, responderSeat, incomingKind, targetSeats);
    internal bool ShanheTrickNativeResponseForbidden(global::CardGame.Core.ProgramNativeResponseQuery query) => _resolve().ShanheTrickNativeResponseForbidden(query);
    internal bool ShanheTrickNullificationBanned(int responderSeat) => _resolve().ShanheTrickNullificationBanned(responderSeat);
}
