#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangGaiMouEntries
{
    internal bool AdvanceMouHgChildren() => _resolve().AdvanceMouHgChildren();
    internal void IssueMouHgSlash(ProgramSkillFrame f, int targetSeat) => _resolve().IssueMouHgSlash(f, targetSeat);
    internal int MouHgDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().MouHgDamageBonus(attack);
    internal bool MouHgDying() => _resolve().MouHgDying();
    internal bool MouHgFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouHgFirstChild(f, child);
    internal bool MouHgLiejiParent(ProgramSkillFrame f) => _resolve().MouHgLiejiParent(f);
    internal ProgramSkillFrame? MouHgObserverRoot() => _resolve().MouHgObserverRoot();
    internal IReadOnlyList<int> MouHgTargets(ProgramSkillFrame f) => _resolve().MouHgTargets(f);
    internal bool MouHgUseRidesOn(ProgramSkillFrame f, CardUseFrame u) => _resolve().MouHgUseRidesOn(f, u);
    internal bool ReturnMouHgAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnMouHgAttack(completion);
    internal void ReturnMouHgUse(CardUseFrame use) => _resolve().ReturnMouHgUse(use);
    internal bool ValidMouHg(ProgramSkillFrame f) => _resolve().ValidMouHg(f);
    internal void ValidateMouHgUse(CardUseFrame use) => _resolve().ValidateMouHgUse(use);
    internal MouHuangGaiRuntime MouHuangGaiRules { get => _resolve().MouHuangGaiRules; }
}
