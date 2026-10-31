namespace CardGame.Core;

internal static class HappyGuanYuComposition
{
    internal const string Wushen="happy:wushen",Wuhun="happy:wuhun";
    internal static bool NightmareJudgment(SkillProgramTrigger t)=>!t.Optional&&t.Subject==SkillProgramTriggerSubject.Owner&&
        t.Condition.Kind==SkillProgramTriggerConditionKind.Always&&t.UsageScope is null&&t.MarkerCost is null&&
        t.Effects is[{Op:SkillProgramEffectOp.SelectTarget,Target:SkillProgramEffectTarget.Owner,TargetKind:SkillProgramTargetKind.MaximumOtherAttributedMarker,Marker:PlayerMarkerKind.Nightmare},
            {Op:SkillProgramEffectOp.StartJudgment,Target:SkillProgramEffectTarget.SelectedTarget,JudgmentReason:"skill.happy-wuhun",ResultBind:"nightmare-judgment",Visibility:SkillProgramCardSetVisibility.Public},
            {Op:SkillProgramEffectOp.LoseHp,Target:SkillProgramEffectTarget.SelectedTarget,Amount:5,Condition:{Kind:SkillProgramConditionKind.Not,Children:[{Kind:SkillProgramConditionKind.BoundCardsMatchKinds,SourceBind:"nightmare-judgment"}safe]}},
            {Op:SkillProgramEffectOp.MoveBoundCards,Target:SkillProgramEffectTarget.Owner,SourceBind:"nightmare-judgment",ExceptBind:null,Destination:SkillProgramCardDestination.DiscardPile,Condition.Kind:SkillProgramConditionKind.Always}]&&
        safe.CardKinds.SequenceEqual([CardKind.Peach,CardKind.PeachGarden])&&t.Effects.Take(2).All(e=>e.Condition.Kind==SkillProgramConditionKind.Always);
    [ProgramCatalogValidation(41880)]internal static void Validate(string path,SkillProgram p)
    {
        if(p.Id is not(Wushen or Wuhun))return;
        var valid=p.Id==Wushen?
            p.ViewAs is[{Id:"heart-hand-as-slash",InputCount:1,OutputKind:CardKind.Slash,ForPlay:true,ForResponse:true,UseOnly:false,Condition.Kind:SkillProgramConditionKind.Always}v]&&v.SourceZones.SequenceEqual([CardZoneKind.Hand])&&
            v.InputKinds.Count==0&&v.InputSuits.SequenceEqual([Suit.Heart])&&p.CardIdentities.Count==0&&p.Triggers.Count==0&&p.CardPolicies is[{Id:"heart-slash-distance",Kind:SkillProgramCardPolicyKind.IgnoreSlashUseDistanceBySuit,InputSuit:Suit.Heart,Condition.Kind:SkillProgramConditionKind.Always}d]&&
            d.CardKinds.SequenceEqual([CardKind.Slash,CardKind.FireSlash,CardKind.ThunderSlash]):
            p.ViewAs.Count==0&&p.CardPolicies.Count==0&&p.CardIdentities.Count==0&&p.Triggers is[
                {Id:"damage-nightmare",Window:SkillProgramTriggerWindow.DamageAppliedBeforeDying,Subject:SkillProgramTriggerSubject.Owner,DamageOccurrence:SkillProgramDamageOccurrence.PerDamagePoint,Optional:false,
                    Effects:[{Op:SkillProgramEffectOp.ChangeAttributedMarker,Target:SkillProgramEffectTarget.Owner,TargetReference.Kind:ProgramParticipantRef.EventSource,Marker:PlayerMarkerKind.Nightmare,Amount:1}]},
                {Id:"survived-nightmare",Window:SkillProgramTriggerWindow.DyingExited}exited,
                {Id:"death-nightmare",Window:SkillProgramTriggerWindow.OwnerDied}died]&&NightmareJudgment(exited)&&NightmareJudgment(died);
        if(!valid||p.Activations.Count!=0||p.Modifiers.Count!=0||p.BooleanStates.Count!=0||p.Contributions.Count!=0)
            throw new InvalidOperationException(path+": Happy Guan Yu requires voluntary hand-only heart conversions, heart Slash distance and exact survived/death five-HP nightmare judgments without clearing markers.");
    }
}
