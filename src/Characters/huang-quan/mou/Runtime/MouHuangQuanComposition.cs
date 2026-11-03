namespace CardGame.Core;
internal static class MouHuangQuanComposition
{
    [ProgramCatalogValidation(1790)]internal static void Validate(string path,SkillProgram p)
    {
        static bool Reserved(SkillProgramEffect e)=>e.Op==SkillProgramEffectOp.ResolveMouHuangQuanQiaodui;
        var ours=p.Id=="mou:qiaodui";
        if(!ours&&!p.Triggers.SelectMany(t=>t.Effects).Concat(p.Activations.SelectMany(a=>a.Effects)).Any(Reserved))return;
        if(!ours||p.Activations.Count!=0||p.ViewAs.Count!=0||p.CardPolicies.Count!=0||p.Modifiers.Count!=0||p.DamageModifiers.Count!=0)throw new InvalidOperationException(path+": Huang Quan requires his original trigger bindings.");
        var valid=p.Triggers is[{Id:"give-for-original-effect",Window:SkillProgramTriggerWindow.CardUseTargetsFinalized,OwnerRelation:SkillProgramCardActionOwnerRelation.Observer,SingleActionInstance:false,IncludeResponseUses:false,Optional:false,Effects:[{Op:SkillProgramEffectOp.ResolveMouHuangQuanQiaodui}]}];
        if(!valid)throw new InvalidOperationException(path+": Qiaodui changed its original-use trigger.");
    }
}
