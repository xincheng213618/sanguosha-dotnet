using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum HpDifferencePileDamageStage { Target=0, Cards=1, Movement=2, Damage=3 }
public sealed record HpDifferencePileDamageReceipt(PublicPersistentPileSource Ren,int RenCardId,bool IsRed)
{
    public HpDifferencePileDamageStage Stage { get; init; }
    public int TargetSeat { get; init; }=-1;
    public int Required { get; init; }
    public int OwnerHp { get; init; }
    public int TargetHp { get; init; }
    public long Before { get; init; }
    public long After { get; init; }
    private readonly IReadOnlyList<int> _ids=Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> CardIds { get=>_ids; init=>_ids=Array.AsReadOnly(value.ToArray()); }
}
public sealed record HpDifferencePileDamagePaidEvent(long FrameId,int OwnerSeat,int TargetSeat,int Required,int OwnerHp,int TargetHp,long Before,long After):IGameEvent;
public sealed record MobileLuZhiYinQualification(long ActionId,long ParentWindowId,SkillProgramTriggerWindow Window,int RenCardId,SkillProgramCardCategory Category,int TurnNumber,int TurnOwnerSeat);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)] public HpDifferencePileDamageReceipt? HpDifferencePileDamage { get; init; }
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)] public MobileLuZhiYinQualification? MobileLuZhiYin { get; init; }
}

internal static class MobileLuZhiComposition
{
    internal const string Mingren="mobile:mingren",Zhenliang="mobile:zhenliang";
    [ProgramCatalogValidation(81400)]
    private static void Validate(string path,SkillProgram p)
    {
        if(p.Id!=Zhenliang)return;
        if(p.Activations is not [{Id:"reserve-hp-difference-damage",MinCards:0,MaxCards:0,MinTargets:0,MaxTargets:0,UsesPerPhase:1,UsesPerTurn:null,
            Effects:[{Op:SkillProgramEffectOp.PublicPileColorDamage,Target:SkillProgramEffectTarget.Owner,PublicPileDamageCostPolicy:PublicPileColorDamageCostPolicy.HpDifferenceAtLeastOne} damage]}] ||
            !damage.SkillIds.SequenceEqual([Mingren]) || p.Triggers.Count!=2 || p.Triggers.Any(t=>
                t.Window!=(t.Id=="yin-use"?SkillProgramTriggerWindow.CardUseCompleted:SkillProgramTriggerWindow.CardResponseCompleted) ||
                t.Id is not("yin-use" or "yin-played") || t.OwnerRelation!=SkillProgramCardActionOwnerRelation.Actor || !t.Optional || t.RequiredPolarity!=SkillPolarity.Yin ||
                t.IncludeResponseUses!=(t.Id=="yin-use") || t.SingleActionInstance!=(t.Id=="yin-use") ||
                t.Effects is not [{Op:SkillProgramEffectOp.CommitConversionPolarity,Target:SkillProgramEffectTarget.Owner},
                    {Op:SkillProgramEffectOp.SelectTarget,TargetKind:SkillProgramTargetKind.AnyLiving},
                    {Op:SkillProgramEffectOp.Draw,Target:SkillProgramEffectTarget.SelectedTarget,Amount:1},
                    {Op:SkillProgramEffectOp.AwaitBoundCardMovements,Target:SkillProgramEffectTarget.Owner}]))
            throw new InvalidOperationException(path+": mobile Zhenliang requires exact HP-difference Yang and outside-turn native category Use/Played completion Yin.");
    }
}
