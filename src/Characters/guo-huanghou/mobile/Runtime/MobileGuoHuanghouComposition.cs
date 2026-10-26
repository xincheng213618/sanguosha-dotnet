namespace CardGame.Core;

internal static class MobileGuoHuanghouComposition
{
    [ProgramCatalogValidation(81700)]
    private static void Validate(string path,SkillProgram p)
    {
        if(p.Id!="mobile:jiaozhao")return;
        if(p.Triggers.Count!=0||p.Contributions.Count!=0||p.Modifiers.Count!=0||p.CardPolicies.Count!=0||p.BooleanStates.Count!=0||
            p.Activations is not [{Id:"declare-card",MinCards:1,MaxCards:1,MinTargets:0,MaxTargets:0,UsesPerPhase:1,UsesPerTurn:null,Condition.Kind:SkillProgramConditionKind.Always,
                Effects:[{Op:SkillProgramEffectOp.CaptureSelectedCards,Target:SkillProgramEffectTarget.Owner,ResultBind:"revealed-hand"},
                    {Op:SkillProgramEffectOp.RevealBoundCards,SourceBind:"revealed-hand"},
                    {Op:SkillProgramEffectOp.DeclareBoundCardNameUntilTurnEnd,SourceBind:"revealed-hand",StateId:"declared-card-tier",SelfDeclarationFromTier:2}]} activation]||
            !activation.SourceZones.SequenceEqual([CardZoneKind.Hand])||p.ViewAs.Count==0||p.ViewAs.Any(r=>
                !r.DeclaredEntity||r.ConversionStateId!="declared-card-tier"||r.MaximumTier!=2||r.MinimumTier!=(CardCatalog.Get(r.OutputKind).CategoryName=="基本牌"?0:1)||
                !r.SourceZones.SequenceEqual([CardZoneKind.Hand])||r.InputCount!=1||!r.UseOnly||!r.ExcludeOwnerEffects||r.ActivationUsageGroup is not null||r.UsesPerPhase is not null))
            throw new InvalidOperationException(path+": mobile Jiaozhao requires its revealed fixed hand entity and fixed declared name at every tier; only tier two declares for itself.");
    }
}
