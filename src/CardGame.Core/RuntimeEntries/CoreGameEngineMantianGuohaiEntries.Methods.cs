#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMantianGuohaiEntries
{
    internal void AddMantianGuohaiActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> playableCards) => _resolve().AddMantianGuohaiActions(actions, actor, playableCards);
    internal bool AllowsMantianGuohaiGiftActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsMantianGuohaiGiftActivation(gate);
    internal IReadOnlyList<int> CreateMobileShiMantianGuohaiCards(int seat) => _resolve().CreateMobileShiMantianGuohaiCards(seat);
    internal bool IsMantianGuohaiGiftProgram(ProgramSkillFrame frame) => _resolve().IsMantianGuohaiGiftProgram(frame);
    internal bool IsMantianGuohaiUse(long id) => _resolve().IsMantianGuohaiUse(id);
    internal bool KeepMantianGuohaiMaterial(long id) => _resolve().KeepMantianGuohaiMaterial(id);
    internal bool MantianGuohaiFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _resolve().MantianGuohaiFrameRidesOn(child, parent);
    internal string MantianGuohaiGiftSource(long useId, int index) => _resolve().MantianGuohaiGiftSource(useId, index);
    internal void PrepareMantianGuohaiEffectRound(long id) => _resolve().PrepareMantianGuohaiEffectRound(id);
    internal void ResolveMantianGuohai(CharacterState source, Card card, LegalAction action) => _resolve().ResolveMantianGuohai(source, card, action);
    internal void ResolveMantianGuohaiEffect(NullificationWindowFrame pending) => _resolve().ResolveMantianGuohaiEffect(pending);
    internal bool TryCompleteMantianGuohaiGift(ProgramSkillFrame frame, bool completed) => _resolve().TryCompleteMantianGuohaiGift(frame, completed);
    internal bool TryContinueMantianGuohaiGifts(long id) => _resolve().TryContinueMantianGuohaiGifts(id);
}
