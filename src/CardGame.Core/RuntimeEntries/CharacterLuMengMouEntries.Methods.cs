#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengMouEntries
{
    internal void AddGetHengyeGrowthAttackRangeContribution(global::CardGame.Core.CharacterState player, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryContribution> contributions) => _resolve().AddGetHengyeGrowthAttackRangeContribution(player, contributions);
    internal void ApplyHengyeGrowth(long damageFrameId, global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int amount) => _resolve().ApplyHengyeGrowth(damageFrameId, source, target, amount);
    internal void AssertYingboInvariant() => _resolve().AssertYingboInvariant();
    internal int GetHengyeGrowth(global::CardGame.Core.CharacterState player) => _resolve().GetHengyeGrowth(player);
    internal void HumanYingboCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => _resolve().HumanYingboCore(selected, advanceToHumanBoundary);
    internal bool IsAiYingboPending() => _resolve().IsAiYingboPending();
    internal bool IsYingboRepeated(long resolutionId) => _resolve().IsYingboRepeated(resolutionId);
    internal bool IsYingboUnrespondable(long resolutionId) => _resolve().IsYingboUnrespondable(resolutionId);
    internal void RecordYingboCardUse(long resolutionId, int sourceSeat, global::CardGame.Core.CardKind cardKind) => _resolve().RecordYingboCardUse(resolutionId, sourceSeat, cardKind);
    internal void ResetHengyeAfterKill(global::CardGame.Core.CharacterState? killer) => _resolve().ResetHengyeAfterKill(killer);
    internal void ResolveHengyeTurnStart(global::CardGame.Core.CharacterState player) => _resolve().ResolveHengyeTurnStart(player);
    internal void ResolvePendingAiYingbo() => _resolve().ResolvePendingAiYingbo();
    internal void ResolveYingboGiftChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveYingboGiftChoice(selected);
    internal global::CardGame.Core.CommandResult SubmitYingboPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitYingboPromptAnswer(selected);
    internal bool TryBeginYingboGift(long resolutionId, int sourceSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind cardKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.YingboGiftContinuation continuation, global::CardGame.Core.NativeOwners.CardAttackHandle? attack = default, global::CardGame.Core.NativeOwners.GroupCardHandle? group = default) => _resolve().TryBeginYingboGift(resolutionId, sourceSeat, card, cardKind, physicalCards, continuation, attack, group);
    internal global::CardGame.Core.NativeOwners.YingboGiftHandle? ActiveYingboGift { get => _resolve().ActiveYingboGift; set => _resolve().ActiveYingboGift = value; }
}
