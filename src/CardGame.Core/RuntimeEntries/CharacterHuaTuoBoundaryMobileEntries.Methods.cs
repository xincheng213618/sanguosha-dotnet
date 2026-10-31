#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoBoundaryMobileEntries
{
    internal global::CardGame.Core.SkillGrantPhaseExpiry? BoundaryHuaTuoCurrentPlay(int owner) => _resolve().BoundaryHuaTuoCurrentPlay(owner);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BoundaryHuaTuoQingnangPaidEvent> BoundaryHuaTuoPayments(int owner) => _resolve().BoundaryHuaTuoPayments(owner);
    internal void CaptureBoundaryHuaTuoPayment(global::CardGame.Core.CardMovedEvent moved) => _resolve().CaptureBoundaryHuaTuoPayment(moved);
    internal void CaptureBoundaryHuaTuoStart(global::CardGame.Core.ProgramSkillStartedEvent started) => _resolve().CaptureBoundaryHuaTuoStart(started);
    internal bool KeepPaidBoundaryHuaTuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidBoundaryHuaTuo(f);
    internal bool ReturnPaidBoundaryHuaTuoDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPaidBoundaryHuaTuoDiscard(f);
    internal bool ValidBoundaryHuaTuoFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBoundaryHuaTuoFrame(f);
    internal bool ValidBoundaryHuaTuoPayment(global::CardGame.Core.BoundaryHuaTuoQingnangPaidEvent e) => _resolve().ValidBoundaryHuaTuoPayment(e);
}
