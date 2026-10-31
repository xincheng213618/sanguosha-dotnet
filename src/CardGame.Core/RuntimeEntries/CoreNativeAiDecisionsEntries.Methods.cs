#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeAiDecisionsEntries
{
    internal void AddGeneralThought(global::CardGame.Core.AiGeneralThought thought) => _resolve().AddGeneralThought(thought);
    internal void AddThought(global::CardGame.Core.AiThoughtRecord thought) => _resolve().AddThought(thought);
    internal void NotifyAiOfDeath(global::CardGame.Core.CharacterState? killer, global::CardGame.Core.CharacterState victim) => _resolve().NotifyAiOfDeath(killer, victim);
    internal void NotifyAiOfDuel(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target) => _resolve().NotifyAiOfDuel(source, target);
    internal void NotifyAiOfFireAttack(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target) => _resolve().NotifyAiOfFireAttack(source, target);
    internal void NotifyAiOfGroupAttack(global::CardGame.Core.CharacterState source, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().NotifyAiOfGroupAttack(source, targets);
    internal void NotifyAiOfNationalAttack(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target) => _resolve().NotifyAiOfNationalAttack(source, target);
    internal void NotifyAiOfPublicAttack(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.PublicAttackKind kind) => _resolve().NotifyAiOfPublicAttack(source, target, kind);
    internal void NotifyAiOfSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target) => _resolve().NotifyAiOfSlash(source, target);
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
    internal void RunOneAiPlayDecision(global::CardGame.Core.CharacterState player) => _resolve().RunOneAiPlayDecision(player);
}
