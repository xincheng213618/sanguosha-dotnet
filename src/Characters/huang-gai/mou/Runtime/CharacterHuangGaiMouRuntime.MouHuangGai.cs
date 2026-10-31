#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangGaiMouRuntime
{
    internal MouHuangGaiRuntime MouHuangGaiRules => _mouHuangGaiOperations.Get();

    internal bool ValidMouHg(ProgramSkillFrame f) => MouHuangGaiRules.Valid(f);
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 35400)]
    internal int MouHgDamageBonus(GameEngine.IDamageAttempt attack) => MouHuangGaiRules.DamageBonus(attack.IsChainPropagation || attack is not NativeOwners.CardAttackHandle ? null : _characterHuangGaiMouOperations.CreateMouHuangGaiFacts().Use(attack.ResolutionId));
    internal bool MouHgLiejiParent(ProgramSkillFrame f)
    {
        if (f.SkillId != MouHuangGaiRuntime.Lieji || f.TriggerId != "after-trick" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } info } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramCardTriggerWindowFrame p || p.Id != w.ParentFrameId || p.Action.ActionId != info.CardActionId || p.Action.ActorSeat != f.OwnerSeat || CardUseCategoryCatalog.Get(p.Action.EffectiveKind)is not (CardUseCategories.InstantTrick or CardUseCategories.DelayedTrick) || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count)
            return false;
        var c = p.Candidates[p.CandidateIndex];
        return c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.TriggerId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
    }
}
