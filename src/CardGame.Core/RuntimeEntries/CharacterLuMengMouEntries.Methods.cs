#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengMouEntries
{
    internal void AddGetHengyeGrowthAttackRangeContribution(CharacterState player, List<RuleQueryContribution> contributions) => _resolve().AddGetHengyeGrowthAttackRangeContribution(player, contributions);
    internal void ApplyHengyeGrowth(long damageFrameId, CharacterState source, CharacterState target, int amount) => _resolve().ApplyHengyeGrowth(damageFrameId, source, target, amount);
    internal int GetHengyeGrowth(CharacterState player) => _resolve().GetHengyeGrowth(player);
    internal void HumanYingboCore(PromptChoice selected, bool advanceToHumanBoundary) => _resolve().HumanYingboCore(selected, advanceToHumanBoundary);
    internal bool IsAiYingboPending() => _resolve().IsAiYingboPending();
    internal bool IsYingboRepeated(long resolutionId) => _resolve().IsYingboRepeated(resolutionId);
    internal bool IsYingboUnrespondable(long resolutionId) => _resolve().IsYingboUnrespondable(resolutionId);
    internal void RecordYingboCardUse(long resolutionId, int sourceSeat, CardKind cardKind) => _resolve().RecordYingboCardUse(resolutionId, sourceSeat, cardKind);
    internal void ResetHengyeAfterKill(CharacterState? killer) => _resolve().ResetHengyeAfterKill(killer);
    internal void ResolveHengyeTurnStart(CharacterState player) => _resolve().ResolveHengyeTurnStart(player);
    internal void ResolvePendingAiYingbo() => _resolve().ResolvePendingAiYingbo();
    internal void ResolveYingboGiftChoice(PromptChoice selected) => _resolve().ResolveYingboGiftChoice(selected);
    internal CommandResult SubmitYingboPromptAnswer(PromptChoice selected) => _resolve().SubmitYingboPromptAnswer(selected);
    internal bool TryBeginYingboGift(long resolutionId, int sourceSeat, Card card, CardKind cardKind, IReadOnlyList<Card> physicalCards, YingboGiftContinuation continuation, NativeOwners.CardAttackHandle? attack = default, NativeOwners.GroupCardHandle? group = default) => _resolve().TryBeginYingboGift(resolutionId, sourceSeat, card, cardKind, physicalCards, continuation, attack, group);
    internal NativeOwners.YingboGiftHandle? ActiveYingboGift { get => _resolve().ActiveYingboGift; set => _resolve().ActiveYingboGift = value; }
}
