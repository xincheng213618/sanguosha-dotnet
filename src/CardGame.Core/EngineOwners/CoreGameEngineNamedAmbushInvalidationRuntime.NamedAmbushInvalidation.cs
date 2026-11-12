#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNamedAmbushInvalidationRuntime
{
    internal void InvalidateNamedAmbushUse(ProgramSkillFrame f)
    {
        var r = f.NamedAmbush!;
        var use = r.Use!;
        if (!_coreNamedAmbushOperations.TryGetAmbushUse(f.WindowContext!, out var live, out var action) || live != use)
            throw new InvalidOperationException("Paid ambush cannot invalidate a different actual use.");
        var fact = new NamedAmbushUseInvalidatedEvent(f.Id, r.Deposit!, use, r.MovementBefore, r.MovementAfter);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        if (!use.ResponseUse)
            _invalidations.IssueUse(use.ParentFrameId, action, new());
        else if (action.EffectiveKind == CardKind.Dodge)
            _invalidations.IssueDodge(use.ParentFrameId, action);
        else if (action.EffectiveKind == CardKind.Nullification)
            _invalidations.IssueCounterspell(use.ParentFrameId, action);
        else
            throw new InvalidOperationException("The invalidated response lost its exact typed native owner.");
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Name)}】，令【{(CardCatalog.Get(action.EffectiveKind).DisplayName)}】无效。", f.OwnerSeat, action.ActorSeat);
    }

}
