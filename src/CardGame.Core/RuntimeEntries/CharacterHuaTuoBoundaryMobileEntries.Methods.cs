#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoBoundaryMobileEntries
{
    internal SkillGrantPhaseExpiry? BoundaryHuaTuoCurrentPlay(int owner) => _resolve().BoundaryHuaTuoCurrentPlay(owner);
    internal IEnumerable<BoundaryHuaTuoQingnangPaidEvent> BoundaryHuaTuoPayments(int owner) => _resolve().BoundaryHuaTuoPayments(owner);
    internal void CaptureBoundaryHuaTuoPayment(CardMovedEvent moved) => _resolve().CaptureBoundaryHuaTuoPayment(moved);
    internal void CaptureBoundaryHuaTuoStart(ProgramSkillStartedEvent started) => _resolve().CaptureBoundaryHuaTuoStart(started);
    internal bool KeepPaidBoundaryHuaTuo(ProgramSkillFrame f) => _resolve().KeepPaidBoundaryHuaTuo(f);
    internal bool ReturnPaidBoundaryHuaTuoDiscard(ProgramSkillFrame f) => _resolve().ReturnPaidBoundaryHuaTuoDiscard(f);
    internal bool ValidBoundaryHuaTuoFrame(ProgramSkillFrame f) => _resolve().ValidBoundaryHuaTuoFrame(f);
    internal bool ValidBoundaryHuaTuoPayment(BoundaryHuaTuoQingnangPaidEvent e) => _resolve().ValidBoundaryHuaTuoPayment(e);
}
