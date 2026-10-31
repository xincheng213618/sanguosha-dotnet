#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangGaiMouEntries
{
    internal bool AdvanceMouHgChildren() => _resolve().AdvanceMouHgChildren();
    internal void IssueMouHgSlash(global::CardGame.Core.ProgramSkillFrame f, int targetSeat) => _resolve().IssueMouHgSlash(f, targetSeat);
    internal int MouHgDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MouHgDamageBonus(attack);
    internal bool MouHgDamageObserver(long id) => _resolve().MouHgDamageObserver(id);
    internal bool MouHgDying() => _resolve().MouHgDying();
    internal bool MouHgFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouHgFirstChild(f, child);
    internal bool MouHgLiejiParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouHgLiejiParent(f);
    internal bool MouHgMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouHgMovement(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? MouHgObserverRoot() => _resolve().MouHgObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<int> MouHgTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouHgTargets(f);
    internal bool MouHgUseRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame u) => _resolve().MouHgUseRidesOn(f, u);
    internal bool ReturnMouHgAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnMouHgAttack(completion);
    internal void ReturnMouHgUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouHgUse(use);
    internal bool ValidMouHg(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouHg(f);
    internal void ValidateMouHgUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateMouHgUse(use);
    internal global::CardGame.Core.MouHuangGaiRuntime MouHuangGaiRules { get => _resolve().MouHuangGaiRules; }
}
