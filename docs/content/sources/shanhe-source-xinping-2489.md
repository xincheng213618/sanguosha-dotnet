# 寓练于乐：辛评（来源2489）

来源为 [三国杀OL官方客户端配置](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs) 的 `hd_roguelike.sgs.Root.Fight/General/UniversalGroup` 与 `cha_spell.sgs`，沿用 `tools/sync_wushuang_reference.py` 的解码方法。

- 配置归档 SHA-256：`d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`。
- 解码 `hd_roguelike.sgs` SHA-256：`97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`。
- 解码 `cha_spell.sgs` SHA-256：`e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`。

## 完整来源配方

`Fight2489` 的名称文本为“寓练于乐”，`Ggroup=2489`，原组仅一个成员“辛评”。`NewbieGroup=22489` 是另一个新手配方，不是隐藏随从。原General行如下：

```json
{"a":2489,"startChapter":1,"b":4871,"c":"辛评","d":"1000261","f":0,"h":100,"k":4,"l":4,"m":0,"n":0,"o":5,"p":2,"q":4,"r":0,"otherad":0,"v":"3252","pvpspell":"3189"}
```

`b=4871` 是来源泛用“将领”身体，基础体力/上限4/4、摸牌2、起手4、护甲0、护甲恢复0、护甲上限5、额外杀及额外伤害均0。没有追加技能、固定装备、固定手牌、`att_*` 数值档位、隐藏成员或二阶段；1–25档共用该完整身体，实战差异由真实山河图来源成长计算器投影。`startChapter=1` 不替代实际普通池的城市关联，`r=0` 不被解释为隐藏人数标志。

本地身份 `shanhe:source-xinping-2489` 仅持有来源穿屋，不将同名普通武将或张华的整套技能接进此配方。运行时保留 `supporter` 回退键，原 `d=1000261` 已按下述独立源行和PVE皮肤链核实，桌面只绑定此精确来源ID。

## 精确技能复用

`cha_spell3252` 为 `ChuanWu`，主文：

> 锁定技，当你造成或受到伤害后，你失去武将牌上前X个技能直到回合结束（X为你的攻击范围），然后摸与失去技能数等量张牌。

该主文与现有 `ol:chuanwu` 逐字一致，复用 `src/Characters/zhang-hua/ol/Skills/ordinary-zhang-hua.rules.json` 中的真实程序，不另外注册近似同名技能。

共享 `GameEngine.OrderedPrintedSkillLoss.cs` 的资格取决于正数实际伤害、伤害参与者、原伤害父帧和仍在进行的真实回合；其身体模板从该技能实例的主将/副将来源取得，依据当前实际持有的印刷技能顺序与攻击范围确定失去数量。它不限定 `ol:zhang-hua`、角色势力或普通身份模式。这里的来源身体印刷技能只有穿屋，因此不会误把张华的弼昏、剑合纳入失去或授予范围。

失去、摸牌孩子及实际回合结束后的恢复沿现有印刷技能暂失机制完成；纯内容复用没有新增挂起状态、共享机制或重复定义检查。

## 普通城市投放边界

Season5的普通池14、16、18、19、20、21、22、26均引用2489，关联第二章大谷关、函谷关、轘辕关、伊阙关、颍川郡、孟津、小平津、荥阳的56座普通城市，并随各地区最低来源难度开放。原 `premisegroup` 分别为1850、1834、1849、1844、1830、1844、1844、1830。

这些引用证明实际候选归属，不证明服务器选择概率；在本地加入候选仍应明确保留未执行原条件的投放边界。原Fight奖励为 `reward=101`、`reward1=401`，不能将本地铜币奖励冒称其官方现金表。

## 2026-10-29 原生席位图与预览身份

完整链为 `Fight2489.Ggroup=2489 → General raw0,b=4871,c=辛评,d=1000261 → root.pveskin.skinID=1000261`。皮肤行明确指定 `seatpic=generalface_1000261`、`headpic=1000261`、`diagrampic=100026100`。人物表4871是“将领”、类 `ShiBing71`，没有 `au/av` 头像重定向；本行的PVE皮肤覆盖载体默认图，不按同名、共享载体或相似面孔借图。

冻结人物表解码SHA-256为 `92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de`，皮肤表解码SHA-256为 `00ee5f994401976563e241571d3e5618f573ad2a4b85336ff132afb262f1e8d4`。当前另行核实的公网Config/client包装SHA已变化，但Character解码字节仍与该冻结值完全一致；旧包装与当前包装证据分别记录于[山河NPC美术来源](../shanhe-npc-art-sources.json)，本批不重抓配置或用当前包装SHA覆盖冻结General/Fight/皮肤资料。

同版客户端 `q6t.getSeatSkinUrl → hA.SkinSeatUrl` 经 `N2t.SeatImgUrl=general/seat/static/` 和桌面 `res/runtime/pc/` 得到[官方原PNG](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_1000261.png)。已取得HTTP200、`image/png`，并目视核实原生卡面。原图146×172 RGBA、53617字节，原字节导入 `src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000261.png`；文件SHA-256为 `8a198fd00dba05716fec8327a087d5daa16d8907838d7b4a7f72b825127d033d`，RGBA像素SHA-256为 `9b535b325a4bd6d410520d89680e3611ad92f8df4ec4bc64c8807f911693babb`。未裁剪、缩放、换色或重编码。

此组只有上述一名源成员，`startChapter=1,f=0`；原行没有 `g/hide/generalShow`，按源默认false/false/0，无主帅、隐藏成员或后续章节成员。完整单人预览保留大卡，不从首席推断主帅。下列运行时ID与上轮全25档实际发布配方盘点一致；美术绑定不改变PortraitKey、VariantId、内容指纹、印刷身体、技能、装备或成长。

| 来源难度 | 精确运行时ID |
| --- | --- |
| 1–25 | `shanhe:source-xinping-2489` |
