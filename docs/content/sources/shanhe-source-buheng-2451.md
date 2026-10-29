# 山河图来源2451：步珩

## 同版来源

官方配置入口：[Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs)。本批读取既有冻结客户端及配置，在内存解密、解压；没有另行下载或解包目录。配置外包 SHA-256：`d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`；客户端外包：`96b7131953f758a30da44c9bdbbcd8a429174050ef7c16961216b90cd923ec31`。解码成员：

- `hd_roguelike.sgs`：`97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`。
- `cha_spell.sgs`：`e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`。

`Root.General` 2451 的完整行：

```json
{"a":2451,"startChapter":1,"b":4871,"c":"步珩","d":"1000333","f":0,"h":100,"k":4,"l":4,"m":0,"n":0,"o":5,"p":2,"q":4,"r":0,"otherad":0,"v":"826","getspell_KN":"2118","getspell_EM":"993","pvpspell":"3189"}
```

这组只有一个4871泛用将领身体，基础体力/上限4/4，摸牌2、起手4；本行没有额外 `att_*`、固定装备或隐藏随从字段。运行时保留 `supporter` 回退键，桌面按下述精确来源皮肤与运行时ID绑定原卡，不使用郝昭、刘封等既有角色的身体或肖像。势力沿同类本地泛用将领采用群；本行没有足够字段证明原服务器势力。

源普通池14、16、18、19、20、21、22、26包含2451及其12451、22451候选编号；对应地图区域1、3、5、6、7、8、9、13，全部开放时覆盖56个普通城市。各区域何时出现仍由原难度门槛控制，不能把候选定义数量当作每场敌将数。

## 累计档位与版本

| 来源难度 | 本地身体 | 完整技能 |
| --- | --- | --- |
| 1–10 | `shanhe:source-buheng-2451-base` | 826镇骨 |
| 11–15 | `shanhe:source-buheng-2451-hard` | 826镇骨、2118生息 |
| 16–25 | `shanhe:source-buheng-2451-nightmare` | 826镇骨、2118生息、993恪守 |

来源826（`ZhenGu`）：

> 结束阶段，你可以选择一名其他角色，本回合结束时和其下回合结束时，其将手牌调整至与你手牌数相同（至多摸至五张）。

与 `classic:zhengu` 现有主文一致，复用 `classic-hao-zhao.rules.json` 的实际回合结束延后调整。原生 `GameEngine.DeferredHandAlignments.cs` 按拥有者、所选目标、技能实例和真实结束帧工作，没有郝昭将领身份门槛。此处只复用技能。

来源2118（`GuoZhanShengXi`）：

> 弃牌阶段开始时，若你本回合未造成伤害，你可以摸两张牌。

独立技能 `shanhe:source-shengxi-2118` 保持弃牌阶段开始的可选摸二。条件复用当前实际回合的原生正伤害历史，计入该回合所有阶段并排除无来源伤害；不新建仅观察持有期间的状态位，也不改成结束阶段发动。

来源993（`GuoZhanKeShou`）完整主文：

> 当你受到伤害时，你可以弃置两张颜色相同的牌，令此伤害-1，若没有与你势力相同的其他角色，你判定，若结果为红色，你摸一张牌。

同版提示 `v` 要求选择两张同色牌弃置、令伤害减一；`w` 为是否发动恪守进行判定。同版客户端注册 `GuoZhanKeShou → EGi`：`cardCountMin=cardCountMax=2`、手牌和装备两区、`CardUseType=DISCARD`、判定成功花色为红桃/方块；其选择器 `pGi` 检查两牌 `EffectColor` 相同。

这些客户端字段证明同色HE付款及红色判定，不能单凭两条提示证明服务端把判定安排为免费分支或付费后的分支。本地依据当前主文的连续效果解释，采用“真实弃两牌 → 原伤害减一 → 此时若没有同势力的其他存活角色，判定；红色摸一”。这是明确的本地时序解释，不是服务端执行顺序的实证；不额外新增免费独势发动。旧版公告未包含此附句，不能代替当前993文本。

## 实际接线

`StandardShanheSourceKeshouPackage` 单独注册 `shanhe:source-keshou-993`，供步珩、羽林军等来源配方共用，避免重复注册或复制同名程序。

新增62061仅承担原伤害帧内的同色HE选择和已付费后的势力分支。真正的逻辑弃牌、失装/移牌孩子、减伤、判定、摸牌和判定牌清理沿已有程序节点；卡牌实体、原位置、技能实例、伤害窗口和实际弃牌记录绑定原帧。已付成本允许原实例失效后的当前结算继续，不重付。独势使用当前有效势力与其他存活角色，不把山河图敌我队伍当作势力。

新的 `ShanheSourceKeshouChecks` 用真实同色手牌与装备付款，在失装移牌孩子暂停，返回原伤害后确认2点减至1点；分别验证独势红判摸一、同势力敌人不判，以及没有同色两牌时不发动。生息的弃牌入口及实际伤害门槛由本批既有来源阶段检查覆盖。

## 2026-10-29 原生席位图与预览身份

完整链为 `Fight2451.Ggroup=2451 → General raw0,b=4871,c=步珩,d=1000333 → root.pveskin.skinID=1000333`。皮肤行明确指定 `seatpic=generalface_1000333`、`headpic=1000333`、`diagrampic=100033300`。人物表4871是“将领”、类 `ShiBing71`，没有 `au/av` 头像重定向；本行的PVE皮肤覆盖载体默认图，不按同名、共享载体或相似面孔借图。

冻结人物表解码SHA-256为 `92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de`，皮肤表解码SHA-256为 `00ee5f994401976563e241571d3e5618f573ad2a4b85336ff132afb262f1e8d4`。当前另行核实的公网Config/client包装SHA已变化，但Character解码字节仍与该冻结值完全一致；旧包装与当前包装证据分别记录于[山河NPC美术来源](../shanhe-npc-art-sources.json)，本批不重抓配置或用当前包装SHA覆盖冻结General/Fight/皮肤资料。

同版客户端 `q6t.getSeatSkinUrl → hA.SkinSeatUrl` 经 `N2t.SeatImgUrl=general/seat/static/` 和桌面 `res/runtime/pc/` 得到[官方原PNG](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_1000333.png)。已取得HTTP200、`image/png`，并目视核实原生卡面。原图146×172 RGBA、52881字节，原字节导入 `src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000333.png`；文件SHA-256为 `fbca32aa255f3f5caf398450446169a58621ee3d104b0b10406743e9d4f6fdb1`，RGBA像素SHA-256为 `04660d47e3384b47de46beb17b5bbf13b370109830ef93b8c34b9cf5cf488ba1`。未裁剪、缩放、换色或重编码。

此组只有上述一名源成员，`startChapter=1,f=0`；原行没有 `g/hide/generalShow`，按源默认false/false/0，无主帅、隐藏成员或后续章节成员。完整单人预览保留大卡，不从首席推断主帅。下列运行时ID与上轮全25档实际发布配方盘点一致；美术绑定不改变PortraitKey、VariantId、内容指纹、印刷身体、技能、装备或成长。

| 来源难度 | 精确运行时ID |
| --- | --- |
| 1–10 | `shanhe:source-buheng-2451-base` |
| 11–15 | `shanhe:source-buheng-2451-hard` |
| 16–25 | `shanhe:source-buheng-2451-nightmare` |
