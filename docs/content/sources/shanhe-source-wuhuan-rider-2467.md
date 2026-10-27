# 乌桓贼匪：乌桓骑手（来源2467）

来源为 [三国杀OL客户端配置](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs) 的 `hd_roguelike.sgs.Root.Fight/General`、`cha_spell.sgs` 和对应客户端 `sgsGame_a.sgs`。沿用仓库 `tools/sync_wushuang_reference.py` 的配置解码方式。

- 配置归档 SHA-256：`d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`。
- 客户端归档 SHA-256：`96b7131953f758a30da44c9bdbbcd8a429174050ef7c16961216b90cd923ec31`。
- 解码 `hd_roguelike.sgs` SHA-256：`97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`。
- 解码 `cha_spell.sgs` SHA-256：`e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`。

## 完整配方与档位

`Fight2467` 的显示名为“乌桓贼匪”，`Ggroup=2467`；整组只有一个 `General.a=2467` 行，成员名“乌桓骑手”。`NewbieGroup=22467` 是另一个新手配方，不是追加成员。本配方没有隐藏成员、召唤技能或固定装备。

原General行：

```json
{"a":2467,"startChapter":1,"b":4861,"c":"乌桓骑手","d":"1000107","f":0,"h":100,"k":4,"l":4,"m":0,"n":0,"o":5,"p":2,"q":4,"r":0,"otherad":0,"v":"747","getspell_KN":"722","getspell_LY":"1600","pvpspell":"3189"}
```

`b=4861` 对应“士兵”，基础体力/上限4/4、摸牌2、起手4、护甲0、护甲恢复0、护甲上限5、额外杀和额外伤害均0。没有 `att_*`、`equip_*`、`carddesc_*` 档位行。实战成长交给现有山河图来源成长计算器；这里的4/4是原始身体，不能用本地驻军的章节起手加成再叠一次。`r=0` 不被解释为隐藏人数标志。

| 来源难度 | 累加技能 | 本地身份 |
| --- | --- | --- |
| 1–10 | 747怀异 | `shanhe:source-wuhuan-rider-2467-base` |
| 11–20 | 747怀异、722突袭 | `shanhe:source-wuhuan-rider-2467-hard` |
| 21–25 | 747怀异、722突袭、1600急袭 | `shanhe:source-wuhuan-rider-2467-hell` |

三种身份均使用 `supporter` 作为明确的本地肖像回退；未将 `d=1000107` 或 `h=100` 误认成已经核实的皮肤。它们不会借用公孙渊的角色身体。

## 技能身份和复用边界

**747怀异（HuaiYi）**复用 `classic:huaiyi` 的真实展示、弃色、逐人获得与失去体力流程。原主文有“每弃置一张牌，便可至少获得一名角色一张牌”，但同一行的选择提示明确写“请选择至多{TargetCount}名角色”；同一客户端注册 `HuaiYi → eWi`，其 `TargetCountMax` 返回实际 `discardCount`。因此采用“至多弃牌数名其他角色、各一张牌”的现有实现；这是根据同源提示和客户端行为消解文字冲突，没有把原行改写成不存在的版本。

**722突袭（TuXiPo）**原文：“摸牌阶段，你可以少摸任意张牌并获得等量其他角色的各一张手牌。”独立技能 `shanhe:source-tuxi-722` 复用 `plannedNormalDrawCount` 上界、减少选定人数的正常摸牌、每名目标获得一张真实手牌。目标仅需是有手牌的其他存活角色。现有 `boundary:tuxi` 的“目标手牌不少于自己”条件不在来源722中，因此没有直接授予该版本。

**1600急袭（JiXiBoss）**原文：“你可以将一张锦囊牌当【顺手牵羊】使用。”独立技能 `shanhe:source-jixi-1600` 将一张手牌锦囊作为真实材料，包括仍在手牌中的延时锦囊。它不是邓艾以田牌转换的急袭。客户端注册 `JiXiBoss → abi`，限制手牌锦囊一张、其他目标一名，并保留距离限制。该客户端的 `NORMAL_JINGNANG` 由 `GetHandTypeByBaseType(Rmt.JINNANG)` 产生，代表锦囊手牌基本类别，不是“非延时锦囊”子类。本地转换沿用原生顺手牵羊的目标、距离、无懈、材料付款、取牌和完结流程。

## 普通城市投放

Season5的普通池14、16、18、19、20、21、22、26均引用2467；关联大谷关、函谷关、轘辕关、伊阙关、颍川郡、孟津、小平津、荥阳的第二章普通城市，共56处。`startChapter=1` 不能替代实际城市关联。原 `premisegroup` 条件未在本地执行，因此在这些地区加入候选属于本地投放约定，不宣称还原服务器选择概率。原Fight奖励为 `reward=101`、`reward1=401`，不将本地铜币奖励冒称该来源的现金表。

## 定向行为检查

`ShanheSourceWuhuanRiderChecks` 仅用鲜活前向原生命令覆盖两项新增差异：真实4张手牌持有者向1张手牌目标发动来源突袭，获得原实体且只少摸一张；真实延时锦囊支付一次原生顺手牵羊，保留距离限制、原牌归属及一次完结。没有新增恢复状态或修改既有classic/boundary技能合同。
