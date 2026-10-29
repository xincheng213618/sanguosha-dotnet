# 山河图林中鹘：来源2476

资料来自官方OL [Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs)。冻结外包 SHA256 为 `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`；解码 `hd_roguelike.sgs` 为 `97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`，`cha_spell.sgs` 为 `e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`，`character.sgs` 为 `92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de`。

`Fight.fightID=2476,Ggroup=2476,text=247600` 只有一名完整成员：`General.a=2476,b=4871,c=林中鹘,d=1000301,f=0,h=100`。基础为 `k=4,l=4,p=2,q=4`，即4/4、摸牌2、初始牌4；`m=0,n=0,o=5,r=0,otherad=0`。该成员没有固定牌、装备、`att_*`、阶段转换或额外隐藏成员字段。保留 `r=0` 的普通属性含义，不把它解释成隐藏标志，也不把 `pvpspell=3189` 追加为固有技能。

原身体4871是“将领”。本包使用独立 `character:shanhe-source-general-4871` 和印刷4/4，不借祝融或李严的身体。此行明确 `d=1000301`，PVE皮肤1000301的 `seatpic=generalface_1000301` 已核实为[官方原卡](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_1000301.png)，146×172、51,886字节，SHA-256为 `8c4a64da009c40a14bc1d7d11321626c27874323be4dacf03533c13e65c4db79`。原PNG保存在 `src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000301.png`，美术记录汇入[山河NPC素材清单](../shanhe-npc-art-sources.json)。两个难度身份均按精确运行时ID绑定原卡；内容定义的 `PortraitKey="supporter"`、`VariantId`、身体及技能不变。

本行 `g` 缺省为false，`hide=false`、`startChapter=1`、`Generalshow=0`。林中鹘不是主帅，介绍保留完整卡与全部技能，不因载体4871名为“将领”而改为主帅标记。

| 来源难度 | GeneralId | 原始技能 |
| --- | --- | --- |
| 1–15 | `shanhe:source-linzhonggu-2476-base` | 984长标 |
| 16–25 | `shanhe:source-linzhonggu-2476-nightmare` | 984长标、765腹鳞 |

984 `ChangBiaoPo` 与现有 `boundary:changbiao-current` 主文一致：出牌阶段限一次，将任意张手牌当无距离限制的杀使用，计入正常使用次数；若此杀对目标造成伤害，在该出牌阶段结束时摸等量牌。直接绑定该现有程序，它保留真实手牌付款、转换来源、阶段用量和阶段结束返回，不附带祝融的巨象或烈刃。

765 `FuLin` 与现有 `classic:fulin` 主文一致：弃牌阶段，本回合获得的牌不计入手牌上限。它直接复用原生的本回合取得实体记录和弃牌排除策略，不附带李严的督粮。

两个技能已经在 `standard-classic-generals` 的角色清单内声明，本包只绑定技能ID，不再次注册技能或加载原角色身体。长标规则按实际技能实例及其转换绑定识别来源；腹鳞按实际持有者的策略资格识别，没有祝融、李严或原GeneralId门槛。

原组由8个普通池引用：14、16、18、19、20、21、22、26。对应第二章的大谷关、函谷关、轘辕关、伊阙关、颍川郡、孟津、小平津、荥阳各7座普通城市，全开放为56城。来源难度1、6、11、16、21、25时分别开放21、35、49、56、56、56座相关城市。仅保存并使用已核池关系；服务端 `premisegroup` 条件和概率未在本地复刻，实际本地投放仍是明确编排。

原始身体、摸牌和起手数量交由征程入口的既有难度、日期、章节及实际人数成长投影；本组不额外叠加另一份章节身体增长。这里只提供完整1–25档来源身体与技能绑定，地图和工厂沿统一入口接线。
