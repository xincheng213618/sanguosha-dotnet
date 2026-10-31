#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpLossSlashMaterialsEntries
{
    internal void AddHpLossSlashEquipmentActions(List<LegalAction> actions, CharacterState owner) => _resolve().AddHpLossSlashEquipmentActions(actions, owner);
    internal void BeginHpLossMaterialSlash(CharacterState owner, IReadOnlyList<Card> cards, CardKind kind, IReadOnlyList<int> seats, CardConversionSource? conversion, ProgramSkillFrame? parent) => _resolve().BeginHpLossMaterialSlash(owner, cards, kind, seats, conversion, parent);
    internal SkillProgramStepOutcome BeginHpLossSelectedSlash(ProgramSkillFrame f, ProgramMultiCardViewAsSelection selection) => _resolve().BeginHpLossSelectedSlash(f, selection);
    internal HpLossSlashSelection? FreezeHpLossSlashSelection(CharacterState owner, ProgramExecutionPlan plan, IReadOnlyList<int> targets) => _resolve().FreezeHpLossSlashSelection(owner, plan, targets);
    internal bool HasExactHpLossMaterialReturn(CardUseFrame use) => _resolve().HasExactHpLossMaterialReturn(use);
    internal bool HasExactHpLossSlashSelection(ProgramSkillFrame f, ProgramExecutionPlan plan) => _resolve().HasExactHpLossSlashSelection(f, plan);
    internal bool HasHpLossSlashTargets(CharacterState owner) => _resolve().HasHpLossSlashTargets(owner);
    internal long? HpLossMaterialSlashParent(NativeOwners.CardAttackHandle attack) => _resolve().HpLossMaterialSlashParent(attack);
    internal int HpLossSlashMaximum(CharacterState owner, CardKind kind, int original = 1) => _resolve().HpLossSlashMaximum(owner, kind, original);
    internal (CardConversionSource Source, string Hash)? HpLossSlashTargetSource(CharacterState owner) => _resolve().HpLossSlashTargetSource(owner);
    internal bool MatchesHpLossMaterialSlash(CardUseFrame use) => _resolve().MatchesHpLossMaterialSlash(use);
    internal void ResolveHpLossSlashZhangba(CharacterState owner, IReadOnlyList<int> targets, IReadOnlyList<Card> cards) => _resolve().ResolveHpLossSlashZhangba(owner, targets, cards);
    internal CommandError? ValidateHpLossSlashZhangba(CharacterState owner, IReadOnlyList<int> ids, IReadOnlyList<int> targets) => _resolve().ValidateHpLossSlashZhangba(owner, ids, targets);
}
