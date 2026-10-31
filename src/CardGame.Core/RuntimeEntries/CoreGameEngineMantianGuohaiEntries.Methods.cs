#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMantianGuohaiEntries
{
    internal void AddMantianGuohaiActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> playableCards) => _resolve().AddMantianGuohaiActions(actions, actor, playableCards);
    internal bool AllowsMantianGuohaiGiftActivation(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsMantianGuohaiGiftActivation(gate);
    internal void AssertMantianGuohaiState() => _resolve().AssertMantianGuohaiState();
    internal global::System.Collections.Generic.IReadOnlyList<int> CreateMobileShiMantianGuohaiCards(int seat) => _resolve().CreateMobileShiMantianGuohaiCards(seat);
    internal bool HasMantianGuohaiPrompt() => _resolve().HasMantianGuohaiPrompt();
    internal bool IsMantianGuohaiGiftProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsMantianGuohaiGiftProgram(frame);
    internal bool IsMantianGuohaiUse(long id) => _resolve().IsMantianGuohaiUse(id);
    internal bool KeepMantianGuohaiMaterial(long id) => _resolve().KeepMantianGuohaiMaterial(id);
    internal bool MantianGuohaiFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().MantianGuohaiFrameRidesOn(child, parent);
    internal string MantianGuohaiGiftSource(long useId, int index) => _resolve().MantianGuohaiGiftSource(useId, index);
    internal void PrepareMantianGuohaiEffectRound(long id) => _resolve().PrepareMantianGuohaiEffectRound(id);
    internal void ResolveMantianGuohai(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => _resolve().ResolveMantianGuohai(source, card, action);
    internal void ResolveMantianGuohaiEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveMantianGuohaiEffect(pending);
    internal bool TryCompleteMantianGuohaiGift(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().TryCompleteMantianGuohaiGift(frame, completed);
    internal bool TryContinueMantianGuohaiGifts(long id) => _resolve().TryContinueMantianGuohaiGifts(id);
}
