#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeAiDecisionsEntries
{
    internal void AddGeneralThought(AiGeneralThought thought) => _resolve().AddGeneralThought(thought);
    internal void AddThought(AiThoughtRecord thought) => _resolve().AddThought(thought);
    internal void NotifyAiOfDeath(CharacterState? killer, CharacterState victim) => _resolve().NotifyAiOfDeath(killer, victim);
    internal void NotifyAiOfDuel(CharacterState source, CharacterState target) => _resolve().NotifyAiOfDuel(source, target);
    internal void NotifyAiOfFireAttack(CharacterState source, CharacterState target) => _resolve().NotifyAiOfFireAttack(source, target);
    internal void NotifyAiOfGroupAttack(CharacterState source, IReadOnlyList<int> targets) => _resolve().NotifyAiOfGroupAttack(source, targets);
    internal void NotifyAiOfNationalAttack(CharacterState source, CharacterState target) => _resolve().NotifyAiOfNationalAttack(source, target);
    internal void NotifyAiOfPublicAttack(CharacterState source, CharacterState target, PublicAttackKind kind) => _resolve().NotifyAiOfPublicAttack(source, target, kind);
    internal void NotifyAiOfSlash(CharacterState source, CharacterState target) => _resolve().NotifyAiOfSlash(source, target);
    internal void ResolvePendingAiBorrowedSword() => _resolve().ResolvePendingAiBorrowedSword();
    internal void ResolvePendingAiCixiongDoubleSwords() => _resolve().ResolvePendingAiCixiongDoubleSwords();
    internal void ResolvePendingAiDodge() => _resolve().ResolvePendingAiDodge();
    internal void ResolvePendingAiDuel() => _resolve().ResolvePendingAiDuel();
    internal void ResolvePendingAiFactionDefense() => _resolve().ResolvePendingAiFactionDefense();
    internal void ResolvePendingAiFactionSlash() => _resolve().ResolvePendingAiFactionSlash();
    internal void ResolvePendingAiFireAttack() => _resolve().ResolvePendingAiFireAttack();
    internal void ResolvePendingAiGroupAttack() => _resolve().ResolvePendingAiGroupAttack();
    internal void ResolvePendingAiHarvest() => _resolve().ResolvePendingAiHarvest();
    internal void ResolvePendingAiIceSword() => _resolve().ResolvePendingAiIceSword();
    internal void ResolvePendingAiNullification() => _resolve().ResolvePendingAiNullification();
    internal void ResolvePendingAiQilinBow() => _resolve().ResolvePendingAiQilinBow();
    internal void ResolvePendingAiQinglongCrescentBlade() => _resolve().ResolvePendingAiQinglongCrescentBlade();
    internal void ResolvePendingAiResponse() => _resolve().ResolvePendingAiResponse();
    internal void ResolvePendingAiSkipDiscardPolicy() => _resolve().ResolvePendingAiSkipDiscardPolicy();
    internal void ResolvePendingAiStoneAxe() => _resolve().ResolvePendingAiStoneAxe();
    internal void ResolvePendingAiTargetCardSelection() => _resolve().ResolvePendingAiTargetCardSelection();
    internal void ResolvePendingAiZhuqueFan() => _resolve().ResolvePendingAiZhuqueFan();
    internal void RunOneAiPlayDecision(CharacterState player) => _resolve().RunOneAiPlayDecision(player);
}
