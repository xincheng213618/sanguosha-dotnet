#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpLossSlashMaterialsEntries
{
    internal void AddHpLossSlashEquipmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState owner) => _resolve().AddHpLossSlashEquipmentActions(actions, owner);
    internal void BeginHpLossMaterialSlash(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> seats, global::CardGame.Core.CardConversionSource? conversion, global::CardGame.Core.ProgramSkillFrame? parent) => _resolve().BeginHpLossMaterialSlash(owner, cards, kind, seats, conversion, parent);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHpLossSelectedSlash(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().BeginHpLossSelectedSlash(f, selection);
    internal global::CardGame.Core.HpLossSlashSelection? FreezeHpLossSlashSelection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().FreezeHpLossSlashSelection(owner, plan, targets);
    internal bool HasExactHpLossMaterialReturn(global::CardGame.Core.CardUseFrame use) => _resolve().HasExactHpLossMaterialReturn(use);
    internal bool HasExactHpLossSlashSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().HasExactHpLossSlashSelection(f, plan);
    internal bool HasHpLossSlashTargets(global::CardGame.Core.CharacterState owner) => _resolve().HasHpLossSlashTargets(owner);
    internal long? HpLossMaterialSlashParent(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().HpLossMaterialSlashParent(attack);
    internal int HpLossSlashMaximum(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, int original = 1) => _resolve().HpLossSlashMaximum(owner, kind, original);
    internal (global::CardGame.Core.CardConversionSource Source, string Hash)? HpLossSlashTargetSource(global::CardGame.Core.CharacterState owner) => _resolve().HpLossSlashTargetSource(owner);
    internal bool MatchesHpLossMaterialSlash(global::CardGame.Core.CardUseFrame use) => _resolve().MatchesHpLossMaterialSlash(use);
    internal void ResolveHpLossSlashZhangba(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().ResolveHpLossSlashZhangba(owner, targets, cards);
    internal global::CardGame.Core.CommandError? ValidateHpLossSlashZhangba(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ValidateHpLossSlashZhangba(owner, ids, targets);
}
